CREATE DATABASE WarehouseManagement
GO

USE WarehouseManagement
GO
CREATE TABLE Suppliers
(
    SupplierId INT PRIMARY KEY IDENTITY(1,1),
    SupplierName NVARCHAR(100) NOT NULL,
    Phone VARCHAR(15),
    Address NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE()
)
GO
CREATE TABLE Products
(
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(150) NOT NULL,
    Quantity INT DEFAULT 0,
    Price DECIMAL(18,2),
    MinStock INT DEFAULT 10,
    SupplierId INT,
    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Product_Supplier
    FOREIGN KEY (SupplierId)
    REFERENCES Suppliers(SupplierId)
)
GO
CREATE TABLE StockLogs
(
    LogId INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    Type NVARCHAR(20), -- IMPORT / EXPORT
    Note NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Stock_Product
    FOREIGN KEY(ProductId)
    REFERENCES Products(ProductId)
)
GO

--Thêm sản phẩm
CREATE PROCEDURE sp_AddProduct
(
    @ProductName NVARCHAR(150),
    @Quantity INT,
    @Price DECIMAL(18,2),
    @MinStock INT,
    @SupplierId INT
)
AS
BEGIN
    INSERT INTO Products
    (
        ProductName,
        Quantity,
        Price,
        MinStock,
        SupplierId
    )
    VALUES
    (
        @ProductName,
        @Quantity,
        @Price,
        @MinStock,
        @SupplierId
    )
END
GO
--Lấy danh sách sản phẩm
CREATE PROCEDURE sp_GetProducts
AS
BEGIN
    SELECT 
        p.ProductId,
        p.ProductName,
        p.Quantity,
        p.Price,
        p.MinStock,
        s.SupplierName
    FROM Products p
    LEFT JOIN Suppliers s
        ON p.SupplierId = s.SupplierId
END
GO
--Cập nhật sản phẩm
CREATE PROCEDURE sp_UpdateProduct
(
    @ProductId INT,
    @ProductName NVARCHAR(150),
    @Quantity INT,
    @Price DECIMAL(18,2),
    @MinStock INT,
    @SupplierId INT
)
AS
BEGIN
    UPDATE Products
    SET
        ProductName = @ProductName,
        Quantity = @Quantity,
        Price = @Price,
        MinStock = @MinStock,
        SupplierId = @SupplierId
    WHERE ProductId = @ProductId
END
GO
--Xóa sản phẩm
CREATE PROCEDURE sp_DeleteProduct
(
    @ProductId INT
)
AS
BEGIN
    DELETE FROM Products
    WHERE ProductId = @ProductId
END
GO

--Supplier Stored Procedure
--Thêm nhà cung cấp
CREATE PROCEDURE sp_AddSupplier
(
    @SupplierName NVARCHAR(100),
    @Phone VARCHAR(15),
    @Address NVARCHAR(255)
)
AS
BEGIN
    INSERT INTO Suppliers
    (
        SupplierName,
        Phone,
        Address
    )
    VALUES
    (
        @SupplierName,
        @Phone,
        @Address
    )
END
GO
--Danh sách nhà cung cấp
CREATE PROCEDURE sp_GetSuppliers
AS
BEGIN
    SELECT * FROM Suppliers
END
GO
--Nhap kho
CREATE PROCEDURE sp_ImportStock
(
    @ProductId INT,
    @Quantity INT,
    @Note NVARCHAR(255)
)
AS
BEGIN

    UPDATE Products
    SET Quantity = Quantity + @Quantity
    WHERE ProductId = @ProductId

    INSERT INTO StockLogs
    (
        ProductId,
        Quantity,
        Type,
        Note
    )
    VALUES
    (
        @ProductId,
        @Quantity,
        'IMPORT',
        @Note
    )

END
GO
-- xuat kho
CREATE PROCEDURE sp_ExportStock
(
    @ProductId INT,
    @Quantity INT,
    @Note NVARCHAR(255)
)
AS
BEGIN

    DECLARE @CurrentStock INT

    SELECT @CurrentStock = Quantity
    FROM Products
    WHERE ProductId = @ProductId

    IF(@CurrentStock < @Quantity)
    BEGIN
        RAISERROR(N'Không đủ hàng trong kho',16,1)
        RETURN
    END

    UPDATE Products
    SET Quantity = Quantity - @Quantity
    WHERE ProductId = @ProductId

    INSERT INTO StockLogs
    (
        ProductId,
        Quantity,
        Type,
        Note
    )
    VALUES
    (
        @ProductId,
        @Quantity,
        'EXPORT',
        @Note
    )

END
GO
--Báo cáo tồn kho thấp
CREATE PROCEDURE sp_LowStockReport
AS
BEGIN
    SELECT
        ProductId,
        ProductName,
        Quantity,
        MinStock
    FROM Products
    WHERE Quantity <= MinStock
END
GO
--Lịch sử thay đổi tồn kho
CREATE PROCEDURE sp_InventoryHistory
AS
BEGIN
    SELECT
        sl.LogId,
        p.ProductName,
        sl.Quantity,
        sl.Type,
        sl.Note,
        sl.CreatedAt
    FROM StockLogs sl
    INNER JOIN Products p
        ON sl.ProductId = p.ProductId
    ORDER BY sl.CreatedAt DESC
END
GO