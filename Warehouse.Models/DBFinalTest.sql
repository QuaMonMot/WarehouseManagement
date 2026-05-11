-- =============================================
-- DATABASE: WarehouseManagement
-- Hệ thống quản lý kho hàng
-- =============================================

USE master
GO

-- =============================================
-- TẠO DATABASE
-- =============================================
IF DB_ID('WarehouseManagement') IS NULL
BEGIN
    CREATE DATABASE WarehouseManagement
END
GO

USE WarehouseManagement
GO

-- =============================================
-- XÓA BẢNG CŨ (NẾU CÓ)
-- =============================================

IF OBJECT_ID('StockLogs', 'U') IS NOT NULL
    DROP TABLE StockLogs
GO

IF OBJECT_ID('Products', 'U') IS NOT NULL
    DROP TABLE Products
GO

IF OBJECT_ID('Suppliers', 'U') IS NOT NULL
    DROP TABLE Suppliers
GO

-- =============================================
-- BẢNG NHÀ CUNG CẤP
-- =============================================
CREATE TABLE Suppliers
(
    SupplierId INT PRIMARY KEY IDENTITY(1,1),

    SupplierCode NVARCHAR(50) UNIQUE,

    SupplierName NVARCHAR(100) NOT NULL,

    Phone VARCHAR(15),

    Address NVARCHAR(255),

    CreatedAt DATETIME DEFAULT GETDATE()
)
GO

-- =============================================
-- BẢNG SẢN PHẨM
-- =============================================
CREATE TABLE Products
(
    ProductId INT PRIMARY KEY IDENTITY(1,1),

    SKU NVARCHAR(50) UNIQUE,

    ProductName NVARCHAR(150) NOT NULL,

    Quantity INT DEFAULT 0,

    Price DECIMAL(18,2),

    MinStock INT DEFAULT 10,

    SupplierId INT,

    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Product_Supplier
    FOREIGN KEY (SupplierId)
    REFERENCES Suppliers(SupplierId),

    CONSTRAINT CK_Product_Quantity
    CHECK (Quantity >= 0),

    CONSTRAINT CK_Product_Price
    CHECK (Price >= 0)
)
GO

-- =============================================
-- BẢNG LỊCH SỬ NHẬP XUẤT KHO
-- =============================================
CREATE TABLE StockLogs
(
    LogId INT PRIMARY KEY IDENTITY(1,1),

    ProductId INT NOT NULL,

    Quantity INT NOT NULL,

    Type NVARCHAR(20),

    Note NVARCHAR(255),

    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Stock_Product
    FOREIGN KEY(ProductId)
    REFERENCES Products(ProductId)
)
GO
-- =============================================
-- BẢNG USer
-- =============================================
CREATE TABLE Users
(
    UserId INT PRIMARY KEY IDENTITY(1,1),

    Username NVARCHAR(100) UNIQUE NOT NULL,

    Password NVARCHAR(100) NOT NULL,

    Role NVARCHAR(50) NOT NULL
)
GO

INSERT INTO Users
(
    Username,
    Password,
    Role
)
VALUES
('admin', '123', 'Admin'),

('staff', '123', 'Staff')
GO
-- =============================================
-- THÊM NHÀ CUNG CẤP
-- =============================================
CREATE OR ALTER PROCEDURE sp_AddSupplier
(
    @SupplierCode NVARCHAR(50),
    @SupplierName NVARCHAR(100),
    @Phone VARCHAR(15),
    @Address NVARCHAR(255)
)
AS
BEGIN

    INSERT INTO Suppliers
    (
        SupplierCode,
        SupplierName,
        Phone,
        Address
    )
    VALUES
    (
        @SupplierCode,
        @SupplierName,
        @Phone,
        @Address
    )

END
GO

-- =============================================
-- DANH SÁCH NHÀ CUNG CẤP
-- =============================================
CREATE OR ALTER PROCEDURE sp_GetSuppliers
AS
BEGIN

    SELECT
        SupplierId,
        SupplierCode,
        SupplierName,
        Phone,
        Address,
        CreatedAt
    FROM Suppliers

END
GO

-- =============================================
-- CẬP NHẬT NHÀ CUNG CẤP
-- =============================================
CREATE OR ALTER PROCEDURE sp_UpdateSupplier
(
    @SupplierId INT,
    @SupplierCode NVARCHAR(50),
    @SupplierName NVARCHAR(100),
    @Phone VARCHAR(15),
    @Address NVARCHAR(255)
)
AS
BEGIN

    UPDATE Suppliers
    SET
        SupplierCode = @SupplierCode,
        SupplierName = @SupplierName,
        Phone = @Phone,
        Address = @Address
    WHERE SupplierId = @SupplierId

END
GO

-- =============================================
-- XÓA NHÀ CUNG CẤP
-- =============================================
CREATE OR ALTER PROCEDURE sp_DeleteSupplier
(
    @SupplierId INT
)
AS
BEGIN

    DELETE FROM Suppliers
    WHERE SupplierId = @SupplierId

END
GO

-- =============================================
-- THÊM SẢN PHẨM
-- =============================================
CREATE OR ALTER PROCEDURE sp_AddProduct
(
    @SKU NVARCHAR(50),
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
        SKU,
        ProductName,
        Quantity,
        Price,
        MinStock,
        SupplierId
    )
    VALUES
    (
        @SKU,
        @ProductName,
        @Quantity,
        @Price,
        @MinStock,
        @SupplierId
    )

END
GO

-- =============================================
-- DANH SÁCH SẢN PHẨM
-- =============================================
CREATE OR ALTER PROCEDURE sp_GetProducts
AS
BEGIN

    SELECT
        p.ProductId,
        p.SKU,
        p.ProductName,
        p.Quantity,
        p.Price,
        p.MinStock,
        p.SupplierId,
        s.SupplierName,
        p.CreatedAt
    FROM Products p
    LEFT JOIN Suppliers s
        ON p.SupplierId = s.SupplierId

END
GO

-- =============================================
-- CẬP NHẬT SẢN PHẨM
-- =============================================
CREATE OR ALTER PROCEDURE sp_UpdateProduct
(
    @ProductId INT,
    @SKU NVARCHAR(50),
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
        SKU = @SKU,
        ProductName = @ProductName,
        Quantity = @Quantity,
        Price = @Price,
        MinStock = @MinStock,
        SupplierId = @SupplierId
    WHERE ProductId = @ProductId

END
GO

-- =============================================
-- XÓA SẢN PHẨM
-- =============================================
CREATE OR ALTER PROCEDURE sp_DeleteProduct
(
    @ProductId INT
)
AS
BEGIN

    DELETE FROM Products
    WHERE ProductId = @ProductId

END
GO

-- =============================================
-- NHẬP KHO
-- =============================================
CREATE OR ALTER PROCEDURE sp_ImportStock
(
    @ProductId INT,
    @Quantity INT,
    @Note NVARCHAR(255)
)
AS
BEGIN

    BEGIN TRY

        BEGIN TRAN

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

        COMMIT TRAN

    END TRY

    BEGIN CATCH

        ROLLBACK TRAN

        THROW

    END CATCH

END
GO

-- =============================================
-- XUẤT KHO
-- =============================================
CREATE OR ALTER PROCEDURE sp_ExportStock
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

    BEGIN TRY

        BEGIN TRAN

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

        COMMIT TRAN

    END TRY

    BEGIN CATCH

        ROLLBACK TRAN

        THROW

    END CATCH

END
GO

-- =============================================
-- BÁO CÁO TỒN KHO THẤP
-- =============================================
CREATE OR ALTER PROCEDURE sp_LowStockReport
AS
BEGIN

    SELECT
        ProductId,
        SKU,
        ProductName,
        Quantity,
        MinStock
    FROM Products
    WHERE Quantity <= MinStock

END
GO

-- =============================================
-- LỊCH SỬ NHẬP XUẤT KHO
-- =============================================
CREATE OR ALTER PROCEDURE sp_InventoryHistory
AS
BEGIN

    SELECT
        sl.LogId,
        p.SKU,
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

-- =============================================
-- DỮ LIỆU MẪU NHÀ CUNG CẤP
-- =============================================
INSERT INTO Suppliers
(
    SupplierCode,
    SupplierName,
    Phone,
    Address
)
VALUES
('SUP-001', N'Dell Supplier', '0900000001', N'Hồ Chí Minh'),
('SUP-002', N'Asus Supplier', '0900000002', N'Hà Nội'),
('SUP-003', N'HP Supplier', '0900000003', N'Đà Nẵng')
GO

-- =============================================
-- DỮ LIỆU MẪU SẢN PHẨM
-- =============================================
INSERT INTO Products
(
    SKU,
    ProductName,
    Quantity,
    Price,
    MinStock,
    SupplierId
)
VALUES
('DEL-001', N'Dell Inspiron 15', 20, 1500, 5, 1),

('ASUS-001', N'Asus ROG Strix', 15, 2200, 3, 2),

('HP-001', N'HP Pavilion', 10, 1300, 2, 3)
GO
-- =============================================
-- KHO HÀNG 2
-- =============================================
--hIỂN THỊ TỔNG QUAN KHO HÀNG
CREATE OR ALTER PROCEDURE sp_StockDashboard
AS
BEGIN

    SELECT
        COUNT(*) AS TotalProducts,

        SUM(Quantity) AS TotalStock,

        (
            SELECT COUNT(*)
            FROM Products
            WHERE Quantity <= MinStock
        ) AS LowStockProducts,

        (
            SELECT ISNULL(SUM(Quantity),0)
            FROM StockLogs
            WHERE Type = 'IMPORT'
        ) AS TotalImport,

        (
            SELECT ISNULL(SUM(Quantity),0)
            FROM StockLogs
            WHERE Type = 'EXPORT'
        ) AS TotalExport

    FROM Products

END
GO
--tÌM KIẾM SẢN PHẨM THEO TÊN HOẶC SKU
CREATE OR ALTER PROCEDURE sp_SearchProducts
(
    @Keyword NVARCHAR(100)
)
AS
BEGIN

    SELECT
        ProductId,
        SKU,
        ProductName,
        Quantity,
        Price,
        MinStock,
        SupplierId
    FROM Products
    WHERE
        ProductName LIKE '%' + @Keyword + '%'
        OR SKU LIKE '%' + @Keyword + '%'

END
GO
--tÌM KIẾM  
CREATE OR ALTER PROCEDURE sp_SearchProducts
(
    @Keyword NVARCHAR(100)
)
AS
BEGIN

    SELECT
        ProductId,
        SKU,
        ProductName,
        Quantity,
        Price,
        MinStock,
        SupplierId
    FROM Products
    WHERE
        ProductName LIKE '%' + @Keyword + '%'
        OR SKU LIKE '%' + @Keyword + '%'

END
GO
--pHÂN TRANG SẢN PHẨM
CREATE OR ALTER PROCEDURE sp_GetProductsPaging
(
    @Page INT,
    @PageSize INT
)
AS
BEGIN

    SELECT
        ProductId,
        SKU,
        ProductName,
        Quantity,
        Price,
        MinStock,
        SupplierId
    FROM Products

    ORDER BY ProductId

    OFFSET (@Page - 1) * @PageSize ROWS

    FETCH NEXT @PageSize ROWS ONLY

END
GO
--bÁO CÁO
CREATE OR ALTER PROCEDURE sp_StockReport
AS
BEGIN

    SELECT
        p.ProductName,

        SUM(
            CASE
                WHEN sl.Type = 'IMPORT'
                THEN sl.Quantity
                ELSE 0
            END
        ) AS TotalImport,

        SUM(
            CASE
                WHEN sl.Type = 'EXPORT'
                THEN sl.Quantity
                ELSE 0
            END
        ) AS TotalExport

    FROM StockLogs sl

    INNER JOIN Products p
        ON sl.ProductId = p.ProductId

    GROUP BY p.ProductName

END
GO
-- =============================================
-- TEST
-- =============================================

EXEC sp_GetSuppliers
GO

EXEC sp_GetProducts
GO

EXEC sp_LowStockReport
GO

EXEC sp_InventoryHistory
GO
