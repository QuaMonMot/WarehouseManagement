-- 1. Tạo Database
CREATE DATABASE QuanLyKho_DoAn;
GO
USE QuanLyKho_DoAn;
GO

-- ==========================================
-- PHẦN 1: TẠO CÁC BẢNG (TABLES) CÓ XÓA MỀM
-- ==========================================

-- Bảng Nhóm quyền
CREATE TABLE Roles (
    role_id INT IDENTITY(1,1) PRIMARY KEY,
    role_name NVARCHAR(50) NOT NULL,
    description NVARCHAR(255),
    is_deleted BIT DEFAULT 0 -- Xóa mềm
);

-- Bảng Người dùng
CREATE TABLE Users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    full_name NVARCHAR(100) NOT NULL,
    phone VARCHAR(15),
    role_id INT,
    is_deleted BIT DEFAULT 0, -- Xóa mềm (thay thế cho is_active)
    CONSTRAINT FK_User_Role FOREIGN KEY (role_id) REFERENCES Roles(role_id)
);

-- Bảng Danh mục
CREATE TABLE Categories (
    category_id INT IDENTITY(1,1) PRIMARY KEY,
    category_name NVARCHAR(100) NOT NULL,
    description NVARCHAR(255),
    is_deleted BIT DEFAULT 0 -- Xóa mềm
);

-- Bảng Nhà cung cấp
CREATE TABLE Suppliers (
    supplier_id INT IDENTITY(1,1) PRIMARY KEY,
    supplier_name NVARCHAR(150) NOT NULL,
    phone VARCHAR(15),
    address NVARCHAR(255),
    email VARCHAR(100),
    is_deleted BIT DEFAULT 0 -- Xóa mềm
);

-- Bảng Sản phẩm
CREATE TABLE Products (
    product_id INT IDENTITY(1,1) PRIMARY KEY,
    product_name NVARCHAR(150) NOT NULL,
    category_id INT,
    unit NVARCHAR(20) NOT NULL,
    price DECIMAL(18, 2) NOT NULL,
    is_deleted BIT DEFAULT 0, -- Xóa mềm
    CONSTRAINT FK_Product_Category FOREIGN KEY (category_id) REFERENCES Categories(category_id)
);

-- Bảng Tồn kho
CREATE TABLE Inventory (
    product_id INT PRIMARY KEY,
    quantity_in_stock INT DEFAULT 0,
    last_updated DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Inventory_Product FOREIGN KEY (product_id) REFERENCES Products(product_id)
);

-- Bảng Phiếu Nhập
CREATE TABLE Import_Orders (
    import_id INT IDENTITY(1,1) PRIMARY KEY,
    supplier_id INT,
    user_id INT,
    import_date DATETIME DEFAULT GETDATE(),
    status NVARCHAR(50) DEFAULT N'Đã hoàn thành', -- Trạng thái: Đã hoàn thành / Đã hủy
    total_amount DECIMAL(18, 2) DEFAULT 0,
    CONSTRAINT FK_Import_Supplier FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id),
    CONSTRAINT FK_Import_User FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- Chi tiết Phiếu Nhập
CREATE TABLE Import_Order_Details (
    import_id INT,
    product_id INT,
    quantity INT NOT NULL,
    unit_price DECIMAL(18, 2) NOT NULL,
    PRIMARY KEY (import_id, product_id),
    CONSTRAINT FK_Detail_Import FOREIGN KEY (import_id) REFERENCES Import_Orders(import_id),
    CONSTRAINT FK_Detail_Product_Import FOREIGN KEY (product_id) REFERENCES Products(product_id)
);

-- Bảng Phiếu Xuất
CREATE TABLE Export_Orders (
    export_id INT IDENTITY(1,1) PRIMARY KEY,
    customer_name NVARCHAR(150),
    user_id INT,
    export_date DATETIME DEFAULT GETDATE(),
    status NVARCHAR(50) DEFAULT N'Đã hoàn thành', -- Trạng thái: Đã hoàn thành / Đã hủy
    CONSTRAINT FK_Export_User FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- Chi tiết Phiếu Xuất
CREATE TABLE Export_Order_Details (
    export_id INT,
    product_id INT,
    quantity INT NOT NULL,
    unit_price DECIMAL(18, 2) NOT NULL,
    PRIMARY KEY (export_id, product_id),
    CONSTRAINT FK_Detail_Export FOREIGN KEY (export_id) REFERENCES Export_Orders(export_id),
    CONSTRAINT FK_Detail_Product_Export FOREIGN KEY (product_id) REFERENCES Products(product_id)
);
GO

-- ==========================================
-- PHẦN 2: CHÈN DỮ LIỆU MẪU (GẦN 170 DÒNG)
-- ==========================================

-- 1. Roles (3 rows)
INSERT INTO Roles (role_name, description) VALUES 
(N'Admin', N'Toàn quyền'), (N'Thủ kho', N'Quản lý kho'), (N'Bán hàng', N'Xuất kho');

-- 2. Users (3 rows - GIỮ NGUYÊN)
INSERT INTO Users (username, password_hash, full_name, phone, role_id) VALUES 
('admin', '123', N'Nguyễn Quản Trị', '090', 1),
('an_kho', '123', N'Trần Thị An', '091', 2),
('binh_sale', '123', N'Lê Thái Bình', '092', 3);

-- 3. Categories (10 rows - Bổ sung 3)
INSERT INTO Categories (category_name, is_deleted) VALUES 
(N'Laptop', 0), (N'Điện thoại', 0), (N'Phụ kiện', 0), (N'Màn hình', 0), 
(N'Bàn phím', 0), (N'Chuột', 0), (N'Loa', 0),
(N'Máy in & Scan', 0), (N'Thiết bị mạng', 0), (N'Linh kiện PC cũ', 1); -- Danh mục này đã bị xóa mềm

-- 4. Suppliers (10 rows - Bổ sung 3)
INSERT INTO Suppliers (supplier_name, address, is_deleted) VALUES 
(N'Apple VN', N'HCM', 0), (N'Samsung VN', N'Bắc Ninh', 0), (N'Dell Global', N'USA', 0), 
(N'Asus VN', N'Đài Loan', 0), (N'Logitech', N'China', 0), (N'Sony VN', N'Japan', 0), (N'Xiaomi VN', N'China', 0),
(N'HP Việt Nam', N'Hà Nội', 0), (N'Cisco Networking', N'USA', 0), (N'Nhà cung cấp dỏm', N'Đã phá sản', 1); -- Đã xóa mềm

-- 5. Products (40 rows - Bổ sung 15)
INSERT INTO Products (product_name, category_id, unit, price, is_deleted) VALUES 
(N'Macbook M2', 1, N'Cái', 30000, 0), (N'iPhone 15', 2, N'Cái', 25000, 0), (N'Sạc Anker', 3, N'Cái', 500, 0),
(N'Dell XPS', 1, N'Cái', 35000, 0), (N'Màn Dell 27', 4, N'Cái', 8000, 0), (N'Phím Cơ K8', 5, N'Cái', 2000, 0),
(N'Chuột G304', 6, N'Cái', 800, 0), (N'Loa Marshall', 7, N'Cái', 7000, 0), (N'iPad Pro', 2, N'Cái', 20000, 0),
(N'Samsung S23', 2, N'Cái', 22000, 0), (N'Màn LG 24', 4, N'Cái', 4000, 0), (N'Phím DareU', 5, N'Cái', 500, 0),
(N'Chuột M331', 6, N'Cái', 300, 0), (N'Tai nghe Sony', 3, N'Cái', 4000, 0), (N'Asus Zenbook', 1, N'Cái', 20000, 0),
(N'Sạc dự phòng', 3, N'Cái', 600, 0), (N'Bàn di chuột', 3, N'Cái', 100, 0), (N'Cáp Lightning', 3, N'Sợi', 200, 0),
(N'Macbook M1', 1, N'Cái', 20000, 0), (N'iPhone 14', 2, N'Cái', 18000, 0), (N'Màn Samsung', 4, N'Cái', 5000, 0),
(N'Phím Akko', 5, N'Cái', 1500, 0), (N'Chuột Razer', 6, N'Cái', 1200, 0), (N'Loa JBL', 7, N'Cái', 3000, 0), (N'Túi chống sốc', 3, N'Cái', 400, 0),
-- Bổ sung 15 dòng mới:
(N'Máy in HP LaserJet Pro', 8, N'Cái', 4500, 0), (N'Máy in màu Canon', 8, N'Cái', 3200, 0), (N'Mực in HP 05A', 8, N'Hộp', 800, 0),
(N'Router Wifi TP-Link', 9, N'Cái', 600, 0), (N'Switch Cisco 24 Port', 9, N'Cái', 15000, 0), (N'Cáp mạng Cat6 (Thùng)', 9, N'Thùng', 1200, 0),
(N'CPU Intel Core i9', 10, N'Cái', 12000, 0), (N'RAM Corsair 16GB', 10, N'Thanh', 1500, 0), (N'Ổ cứng SSD Samsung 1TB', 10, N'Cái', 2500, 0),
(N'Card VGA RTX 3060', 10, N'Cái', 8500, 0), (N'Mainboard Asus ROG', 10, N'Cái', 4000, 0), (N'Nguồn máy tính 750W', 10, N'Cái', 1800, 0),
(N'Vỏ Case NZXT', 10, N'Cái', 2200, 0), 
(N'Máy quét mã vạch cũ', 8, N'Cái', 500, 1), -- Sản phẩm đã ngừng kinh doanh (Xóa mềm = 1)
(N'Điện thoại Nokia phím', 2, N'Cái', 300, 1); -- Sản phẩm đã ngừng kinh doanh (Xóa mềm = 1)

-- 6. Import Orders (15 rows - Bổ sung 5, có 1 phiếu HỦY)
INSERT INTO Import_Orders (supplier_id, user_id, status) VALUES 
(1, 2, N'Đã hoàn thành'), (2, 2, N'Đã hoàn thành'), (3, 2, N'Đã hoàn thành'), (4, 2, N'Đã hoàn thành'), (5, 2, N'Đã hoàn thành'),
(6, 2, N'Đã hoàn thành'), (7, 2, N'Đã hoàn thành'), (1, 2, N'Đã hoàn thành'), (2, 2, N'Đã hoàn thành'), (5, 2, N'Đã hoàn thành'),
-- Bổ sung 5 dòng:
(8, 2, N'Đã hoàn thành'), (9, 2, N'Đã hoàn thành'), (4, 2, N'Đã hoàn thành'), 
(3, 2, N'Đã hoàn thành'),
(10, 2, N'Đã hủy'); -- PHIẾU NHẬP NÀY BỊ HỦY DO HÀNG LỖI

-- 7. Import Details (37 rows - Bổ sung 12)
INSERT INTO Import_Order_Details (import_id, product_id, quantity, unit_price) VALUES 
(1, 1, 10, 28000), (1, 2, 20, 24000), (2, 10, 15, 21000), (3, 4, 10, 33000), (3, 5, 10, 7500),
(4, 15, 10, 18000), (5, 7, 50, 700), (5, 13, 50, 250), (6, 14, 20, 3800), (6, 8, 10, 6500),
(7, 3, 100, 450), (7, 16, 50, 550), (8, 19, 10, 19000), (8, 20, 10, 17000), (9, 10, 5, 21000),
(10, 6, 20, 1800), (1, 3, 20, 450), (2, 2, 5, 24000), (3, 11, 10, 3800), (4, 12, 30, 450),
(5, 22, 10, 1400), (6, 24, 15, 2800), (7, 25, 40, 350), (8, 9, 5, 19000), (9, 21, 10, 4800),
-- Bổ sung 12 dòng mới:
(11, 26, 20, 4000), (11, 27, 10, 3000), (11, 28, 50, 600), 
(12, 29, 30, 500), (12, 30, 5, 14000), (12, 31, 10, 1000),
(13, 32, 20, 11500), (13, 33, 50, 1200), (13, 34, 40, 2200),
(14, 35, 15, 8000), (14, 36, 20, 3500),
(15, 39, 100, 200); -- NHẬP MÁY QUÉT MÃ VẠCH (PHIẾU NÀY BỊ HỦY NÊN TÍNH TỒN KHO SẼ KHÔNG CỘNG VÀO)

-- 8. Export Orders (15 rows - Bổ sung 5, có 1 phiếu HỦY)
INSERT INTO Export_Orders (customer_name, user_id, status) VALUES 
(N'Nguyễn Văn A', 3, N'Đã hoàn thành'), (N'Trần Văn B', 3, N'Đã hoàn thành'), (N'Công ty K', 3, N'Đã hoàn thành'), 
(N'Đại lý X', 3, N'Đã hoàn thành'), (N'Lẻ chị Lan', 3, N'Đã hoàn thành'), (N'Anh Hùng IT', 3, N'Đã hoàn thành'), 
(N'Cửa hàng Z', 3, N'Đã hoàn thành'), (N'Khách vãng lai', 3, N'Đã hoàn thành'), (N'Công ty M', 3, N'Đã hoàn thành'), 
(N'Chị Mai', 3, N'Đã hoàn thành'),
-- Bổ sung 5 dòng mới:
(N'Dự án cty Y', 3, N'Đã hoàn thành'), (N'Trường ĐH BK', 3, N'Đã hoàn thành'), (N'Phòng Net VIP', 3, N'Đã hoàn thành'), 
(N'Khách sỉ tỉnh', 3, N'Đã hoàn thành'),
(N'Khách ảo bom hàng', 3, N'Đã hủy'); -- PHIẾU XUẤT NÀY BỊ HỦY DO KHÁCH KHÔNG NHẬN HÀNG

-- 9. Export Details (37 rows - Bổ sung 12)
INSERT INTO Export_Order_Details (export_id, product_id, quantity, unit_price) VALUES 
(1, 1, 1, 30000), (1, 7, 2, 800), (2, 2, 2, 25000), (3, 4, 5, 35000), (3, 5, 2, 8000),
(4, 10, 10, 22000), (4, 13, 10, 300), (5, 15, 1, 20000), (6, 8, 1, 7000), (6, 14, 2, 4000),
(7, 3, 20, 500), (7, 16, 10, 600), (8, 19, 1, 20000), (8, 20, 1, 18000), (9, 6, 5, 2000),
(10, 12, 10, 500), (1, 2, 1, 25000), (2, 10, 2, 22000), (3, 11, 2, 4000), (4, 22, 2, 1500),
(5, 24, 1, 3000), (6, 25, 2, 400), (7, 9, 1, 20000), (8, 21, 2, 5000), (9, 7, 5, 800),
-- Bổ sung 12 dòng mới:
(11, 26, 5, 4500), (11, 28, 10, 800), (11, 29, 10, 600),
(12, 30, 2, 15000), (12, 31, 5, 1200),
(13, 32, 10, 12000), (13, 33, 20, 1500), (13, 34, 10, 2500), (13, 35, 10, 8500),
(14, 36, 5, 4000), (14, 27, 2, 3200),
(15, 1, 5, 30000); -- XUẤT 5 MACBOOK NHƯNG BỊ HỦY NÊN TỒN KHO VẪN SẼ GIỮ NGUYÊN

-- 10. Inventory (Logic nâng cao tính Tồn Kho chuẩn xác)
-- BÍ QUYẾT: Chỉ lấy SUM của những Phiếu Nhập/Xuất có status = N'Đã hoàn thành'
INSERT INTO Inventory (product_id, quantity_in_stock)
SELECT p.product_id, 
       ISNULL((
           SELECT SUM(iod.quantity) 
           FROM Import_Order_Details iod 
           JOIN Import_Orders io ON iod.import_id = io.import_id 
           WHERE iod.product_id = p.product_id AND io.status = N'Đã hoàn thành'
       ), 0) 
       - 
       ISNULL((
           SELECT SUM(eod.quantity) 
           FROM Export_Order_Details eod 
           JOIN Export_Orders eo ON eod.export_id = eo.export_id 
           WHERE eod.product_id = p.product_id AND eo.status = N'Đã hoàn thành'
       ), 0)
FROM Products p;
GO
CREATE PROCEDURE sp_ImportGoods
    @SupplierId INT,
    @UserId INT,
    @ProductId INT,
    @Quantity INT,
    @UnitPrice DECIMAL(18,2)
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Tạo phiếu nhập mới và lấy ID vừa tạo
        INSERT INTO Import_Orders (supplier_id, user_id, total_amount)
        VALUES (@SupplierId, @UserId, @Quantity * @UnitPrice);
        
        DECLARE @ImportId INT = SCOPE_IDENTITY();

        -- 2. Thêm vào chi tiết phiếu nhập
        INSERT INTO Import_Order_Details (import_id, product_id, quantity, unit_price)
        VALUES (@ImportId, @ProductId, @Quantity, @UnitPrice);

        -- 3. Cập nhật tồn kho (Nếu chưa có thì Insert, có rồi thì Update)
        IF EXISTS (SELECT 1 FROM Inventory WHERE product_id = @ProductId)
            UPDATE Inventory SET quantity_in_stock = quantity_in_stock + @Quantity, last_updated = GETDATE()
            WHERE product_id = @ProductId;
        ELSE
            INSERT INTO Inventory (product_id, quantity_in_stock) VALUES (@ProductId, @Quantity);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
CREATE PROCEDURE sp_GetLowStockAlert
    @Threshold INT = 10 -- Ngưỡng cảnh báo (mặc định dưới 10 cái)
AS
BEGIN
    SELECT p.product_name, i.quantity_in_stock, p.unit
    FROM Inventory i
    JOIN Products p ON i.product_id = p.product_id
    WHERE i.quantity_in_stock <= @Threshold AND p.is_deleted = 0;
END
GO
CREATE PROCEDURE sp_ReportDailyStock
    @Date DATE
AS
BEGIN
    SELECT 'NHAP' as Type, p.product_name, d.quantity, o.import_date as ActionDate
    FROM Import_Order_Details d
    JOIN Import_Orders o ON d.import_id = o.import_id
    JOIN Products p ON d.product_id = p.product_id
    WHERE CAST(o.import_date AS DATE) = @Date
    UNION ALL
    SELECT 'XUAT' as Type, p.product_name, d.quantity, o.export_date
    FROM Export_Order_Details d
    JOIN Export_Orders o ON d.export_id = o.export_id
    JOIN Products p ON d.product_id = p.product_id
    WHERE CAST(o.export_date AS DATE) = @Date;
END
GO
-- 1. Lấy danh sách nhà cung cấp (chỉ lấy những người chưa bị xóa mềm)
CREATE PROCEDURE sp_GetAllSuppliers
AS
BEGIN
    SELECT supplier_id, supplier_name, phone, address, email 
    FROM Suppliers 
    WHERE is_deleted = 0
    ORDER BY supplier_id DESC;
END
GO

-- 2. Thêm mới nhà cung cấp
CREATE PROCEDURE sp_InsertSupplier
    @Name NVARCHAR(150),
    @Phone VARCHAR(15),
    @Address NVARCHAR(255),
    @Email VARCHAR(100)
AS
BEGIN
    INSERT INTO Suppliers (supplier_name, phone, address, email, is_deleted)
    VALUES (@Name, @Phone, @Address, @Email, 0);
END
GO

-- 3. Cập nhật thông tin nhà cung cấp
CREATE PROCEDURE sp_UpdateSupplier
    @Id INT,
    @Name NVARCHAR(150),
    @Phone VARCHAR(15),
    @Address NVARCHAR(255),
    @Email VARCHAR(100)
AS
BEGIN
    UPDATE Suppliers 
    SET supplier_name = @Name, 
        phone = @Phone, 
        address = @Address, 
        email = @Email
    WHERE supplier_id = @Id;
END
GO

-- 4. Xóa mềm nhà cung cấp
CREATE PROCEDURE sp_SoftDeleteSupplier
    @Id INT
AS
BEGIN
    UPDATE Suppliers 
    SET is_deleted = 1 
    WHERE supplier_id = @Id;
END
GO
CREATE PROCEDURE sp_GetInventoryReport
    @MinStock INT = 0 -- Nếu truyền vào 10, nó sẽ lọc các món còn dưới 10 cái
AS
BEGIN
    SELECT 
        p.product_id, 
        p.product_name, 
        c.category_name,
        i.quantity_in_stock, 
        p.unit,
        i.last_updated
    FROM Inventory i
    JOIN Products p ON i.product_id = p.product_id
    JOIN Categories c ON p.category_id = c.category_id
    WHERE i.quantity_in_stock <= CASE WHEN @MinStock = 0 THEN i.quantity_in_stock ELSE @MinStock END
    AND p.is_deleted = 0;
END
GO
CREATE PROCEDURE sp_GetStockHistory
    @ProductId INT
AS
BEGIN
    -- Lấy lịch sử từ phiếu nhập
    SELECT 
        o.import_date AS ActionDate, 
        N'Nhập kho' AS ActionType, 
        d.quantity AS Amount, 
        u.full_name AS PerformedBy
    FROM Import_Order_Details d
    JOIN Import_Orders o ON d.import_id = o.import_id
    JOIN Users u ON o.user_id = u.user_id
    WHERE d.product_id = @ProductId

    UNION ALL

    -- Lấy lịch sử từ phiếu xuất
    SELECT 
        e.export_date, 
        N'Xuất kho', 
        -d.quantity, -- Số âm để thể hiện là xuất đi
        u.full_name
    FROM Export_Order_Details d
    JOIN Export_Orders e ON d.export_id = e.export_id
    JOIN Users u ON e.user_id = u.user_id
    WHERE d.product_id = @ProductId
    ORDER BY ActionDate DESC;
END
GO
CREATE PROCEDURE sp_GetGeneralStatistics
AS
BEGIN
    SELECT 
        (SELECT COUNT(*) FROM Products WHERE is_deleted = 0) AS TotalProducts,
        (SELECT COUNT(*) FROM Suppliers WHERE is_deleted = 0) AS TotalSuppliers,
        (SELECT ISNULL(SUM(quantity_in_stock), 0) FROM Inventory) AS TotalStock,
        (SELECT ISNULL(SUM(total_amount), 0) FROM Import_Orders WHERE status = N'Đã hoàn thành') AS TotalImportValue
END
GO
CREATE PROCEDURE sp_ReportInventoryFlow
    @FromDate DATE,
    @ToDate DATE
AS
BEGIN
    SELECT 
        p.product_id,
        p.product_name,
        p.unit,
        ISNULL(ImportData.TotalImport, 0) AS QtyImport,
        ISNULL(ExportData.TotalExport, 0) AS QtyExport,
        i.quantity_in_stock AS CurrentInventory
    FROM Products p
    LEFT JOIN (
        SELECT product_id, SUM(quantity) AS TotalImport 
        FROM Import_Order_Details d JOIN Import_Orders o ON d.import_id = o.import_id
        WHERE o.import_date BETWEEN @FromDate AND @ToDate AND o.status = N'Đã hoàn thành'
        GROUP BY product_id
    ) ImportData ON p.product_id = ImportData.product_id
    LEFT JOIN (
        SELECT product_id, SUM(quantity) AS TotalExport 
        FROM Export_Order_Details d JOIN Export_Orders o ON d.export_id = o.export_id
        WHERE o.export_date BETWEEN @FromDate AND @ToDate AND o.status = N'Đã hoàn thành'
        GROUP BY product_id
    ) ExportData ON p.product_id = ExportData.product_id
    JOIN Inventory i ON p.product_id = i.product_id
    WHERE p.is_deleted = 0;
END
GO
-- 1. Thủ tục Đăng nhập (Kiểm tra username và password)
CREATE PROCEDURE sp_Login
    @Username VARCHAR(50),
    @PasswordHash VARCHAR(255)
AS
BEGIN
    SELECT u.user_id, u.username, u.full_name, r.role_name
    FROM Users u
    JOIN Roles r ON u.role_id = r.role_id
    WHERE u.username = @Username 
      AND u.password_hash = @PasswordHash 
      AND u.is_deleted = 0;
END
GO

-- 2. Lấy danh sách người dùng
CREATE PROCEDURE sp_GetAllUsers
AS
BEGIN
    SELECT u.user_id, u.username, u.full_name, u.phone, r.role_name, u.is_deleted
    FROM Users u
    JOIN Roles r ON u.role_id = r.role_id
    WHERE u.is_deleted = 0;
END
GO
-- 1. Lấy danh sách tất cả vai trò (để hiển thị lên Combobox khi tạo User)
CREATE PROCEDURE sp_GetAllRoles
AS
BEGIN
    SELECT role_id, role_name, description 
    FROM Roles 
    WHERE is_deleted = 0;
END
GO

-- 2. Thêm vai trò mới (nếu cần)
CREATE PROCEDURE sp_InsertRole
    @RoleName NVARCHAR(50),
    @Description NVARCHAR(255)
AS
BEGIN
    INSERT INTO Roles (role_name, description, is_deleted)
    VALUES (@RoleName, @Description, 0);
END
GO
-- 1. Lấy danh sách sản phẩm kèm tên loại (Dùng để hiển thị lên bảng)
CREATE PROCEDURE sp_GetAllProducts
AS
BEGIN
    SELECT p.product_id, p.product_name, p.unit, p.price, c.category_name, p.category_id
    FROM Products p
    JOIN Categories c ON p.category_id = c.category_id
    WHERE p.is_deleted = 0;
END
GO

-- 2. Thêm sản phẩm mới (Kèm khởi tạo tồn kho)
CREATE PROCEDURE sp_InsertProduct
    @Name NVARCHAR(150),
    @CategoryId INT,
    @Unit NVARCHAR(20),
    @Price DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Thêm vào bảng Product
        INSERT INTO Products (product_name, category_id, unit, price, is_deleted)
        VALUES (@Name, @CategoryId, @Unit, @Price, 0);
        
        DECLARE @NewId INT = SCOPE_IDENTITY();

        -- Khởi tạo ngay tồn kho bằng 0 cho sản phẩm này
        INSERT INTO Inventory (product_id, quantity_in_stock, last_updated)
        VALUES (@NewId, 0, GETDATE());

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
ALTER PROCEDURE sp_InsertProduct
    @Name NVARCHAR(150),
    @CategoryId INT,
    @Unit NVARCHAR(20),
    @Price DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Thêm sản phẩm
        INSERT INTO Products (product_name, category_id, unit, price, is_deleted)
        VALUES (@Name, @CategoryId, @Unit, @Price, 0);
        
        DECLARE @NewId INT = SCOPE_IDENTITY();

        -- 2. Thêm tồn kho
        INSERT INTO Inventory (product_id, quantity_in_stock, last_updated)
        VALUES (@NewId, 0, GETDATE());

        COMMIT TRANSACTION;
        SELECT 1 AS Result; -- Trả về 1 nếu thành công
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        -- In lỗi chi tiết ra tab Messages
        PRINT 'Loi SQL: ' + ERROR_MESSAGE();
        SELECT 0 AS Result; -- Trả về 0 nếu thất bại
    END CATCH
END
GO
CREATE PROCEDURE sp_ExportProduct
    @UserId INT,
    @ProductId INT,
    @Qty INT,          -- Số lượng cần xuất
    @CustomerName NVARCHAR(150) = NULL, -- Tên khách hàng (không bắt buộc)
    @UnitPrice DECIMAL(18,2) = 0        -- Giá xuất (nếu có)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentStock INT;

    -- 1. Kiểm tra hàng trong kho còn đủ không
    SELECT @CurrentStock = quantity_in_stock 
    FROM Inventory 
    WHERE product_id = @ProductId;

    -- Nếu không tìm thấy sản phẩm hoặc kho không đủ
    IF @CurrentStock IS NULL OR @CurrentStock < @Qty
    BEGIN
        RAISERROR(N'Số lượng trong kho không đủ! (Hiện còn: %d)', 16, 1, @CurrentStock);
        RETURN;
    END

    BEGIN TRANSACTION;
    BEGIN TRY
        -- 2. Tạo phiếu xuất mới
        INSERT INTO Export_Orders (customer_name, user_id, export_date, status)
        VALUES (ISNULL(@CustomerName, N'Khách lẻ'), @UserId, GETDATE(), N'Đã hoàn thành');
        
        DECLARE @ExportId INT = SCOPE_IDENTITY();

        -- 3. Thêm vào chi tiết phiếu xuất
        INSERT INTO Export_Order_Details (export_id, product_id, quantity, unit_price)
        VALUES (@ExportId, @ProductId, @Qty, @UnitPrice);

        -- 4. CẬP NHẬT TỒN KHO: Trừ đi số lượng đã xuất
        UPDATE Inventory 
        SET quantity_in_stock = quantity_in_stock - @Qty,
            last_updated = GETDATE()
        WHERE product_id = @ProductId;

        COMMIT TRANSACTION;
        SELECT 1 AS Result; -- Trả về 1 nếu mọi thứ OK
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'Loi SQL: ' + ERROR_MESSAGE();
        SELECT 0 AS Result; -- Trả về 0 nếu thất bại
    END CATCH
END
GO