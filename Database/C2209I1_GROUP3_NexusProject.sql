USE master
GO 
DROP DATABASE IF EXISTS NEXUSPROJECT
CREATE DATABASE NEXUSPROJECT
GO
USE NEXUSPROJECT
GO

-- Tạo bảng Employee (Nhân Viên)
-- Bảng chứa thông tin nhân viên
CREATE TABLE Employee (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY, -- Mã nhân viên, khóa chính
    Username NVARCHAR(50) NOT NULL UNIQUE, -- Tên đăng nhập, duy nhất
    Password NVARCHAR(255) NOT NULL, -- Mật khẩu
    FullName NVARCHAR(100) NOT NULL, -- Họ tên nhân viên
    Role NVARCHAR(50) NOT NULL, -- Vai trò của nhân viên (Quản trị viên, Kỹ thuật, Tài chính, Bán lẻ)
    PhoneNumber NVARCHAR(15), -- Số điện thoại liên lạc
    Email NVARCHAR(100), -- Email liên lạc
	Status BIT,
	SecurityCode NVARCHAR(100),
	Photo NVARCHAR(255)
);
GO


-- Tạo bảng Role (Vai trò)
-- Bảng chứa thông tin vai trò của nhân viên
CREATE TABLE Role (
    RoleID INT IDENTITY(1,1) PRIMARY KEY, -- Mã vai trò, khóa chính
    RoleName NVARCHAR(50) NULL -- Tên vai trò
);
GO

-- Tạo bảng EmployeeRole (Vai trò Nhân Viên)
-- Bảng liên kết giữa nhân viên và vai trò của họ
CREATE TABLE EmployeeRole (
    EmployeeID INT NOT NULL, -- Khóa ngoại tham chiếu đến Employee
    RoleID INT NOT NULL, -- Khóa ngoại tham chiếu đến Role
    PRIMARY KEY (EmployeeID, RoleID), -- Khóa chính kép
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID), -- Khóa ngoại tham chiếu đến Employee
    FOREIGN KEY (RoleID) REFERENCES Role(RoleID) -- Khóa ngoại tham chiếu đến Role
);
GO

-- Tạo bảng Customer (Khách Hàng)
-- Bảng chứa thông tin khách hàng
CREATE TABLE Customer (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY, -- Mã khách hàng, khóa chính
    AccountNumber CHAR(16) UNIQUE NOT NULL, -- Số tài khoản duy nhất gồm 16 chữ số
    FullName NVARCHAR(100) NOT NULL, -- Họ tên khách hàng
    Address NVARCHAR(255), -- Địa chỉ khách hàng
    CityCode CHAR(10), -- Mã thành phố
    PhoneNumber NVARCHAR(15), -- Số điện thoại liên lạc
    Email NVARCHAR(100), -- Email liên lạc
    RegistrationDate DATE, -- Ngày đăng ký
    IDCard NVARCHAR(255), -- Chứng minh nhân dân
	Password NVARCHAR(100),
	Status BIT,
	SecurityCode NVARCHAR(100),
	Photo NVARCHAR(255)
);
GO

CREATE TABLE CustomerServicePackage (
    CustomerID INT NOT NULL, -- Mã khách hàng, khóa ngoại
    ConnectionID INT NOT NULL, -- Mã kết nối, khóa ngoại
    ServicePackageID INT NOT NULL, -- Mã gói dịch vụ, khóa ngoại
    PurchaseDate DATE NOT NULL, -- Ngày mua gói dịch vụ
    PRIMARY KEY (CustomerID, ConnectionID, ServicePackageID), -- Khóa chính kép
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID), -- Khóa ngoại tham chiếu đến bảng Customer
    FOREIGN KEY (ConnectionID) REFERENCES Connections(ConnectionID), -- Khóa ngoại tham chiếu đến bảng Connections
    FOREIGN KEY (ServicePackageID) REFERENCES ServicePackage(ServicePackageID) -- Khóa ngoại tham chiếu đến bảng ServicePackage
);
GO




-- Tạo bảng Order (Đơn Hàng)
-- Bảng chứa thông tin đơn hàng của khách hàng
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY, -- Mã đơn hàng, khóa chính
    OrderCode CHAR(11) UNIQUE NOT NULL, -- Mã đơn hàng duy nhất gồm 11 ký tự
    CustomerID INT NOT NULL, -- Mã khách hàng, khóa ngoại
    ConnectionType CHAR(1) NOT NULL, -- Loại kết nối (D: Dial-Up, B: Băng thông rộng, T: Điện thoại cố định)
    OrderDate DATE NOT NULL, -- Ngày đặt hàng
    Status NVARCHAR(50), -- Trạng thái đơn hàng
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) -- Khóa ngoại tham chiếu đến bảng Customer
);
GO

-- Tạo bảng Connection (Kết Nối)
-- Bảng chứa thông tin kết nối của khách hàng
CREATE TABLE Connections (
    ConnectionID INT IDENTITY(1,1) PRIMARY KEY, -- Mã kết nối, khóa chính
    AccountNumber CHAR(16) UNIQUE NOT NULL, -- Số tài khoản duy nhất
    CustomerID INT NOT NULL, -- Mã khách hàng, khóa ngoại
    ConnectionType CHAR(1) NOT NULL, -- Loại kết nối (D: Dial-Up, B: Băng thông rộng, T: Điện thoại cố định)
    Status NVARCHAR(50), -- Trạng thái kết nối
    ActivationDate DATE, -- Ngày kích hoạt kết nối
    TerminationDate DATE, -- Ngày ngừng kết nối (nếu có)
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) -- Khóa ngoại tham chiếu đến bảng Customer
);
GO

-- Tạo bảng Device (Thiết Bị)
-- Bảng chứa thông tin thiết bị
CREATE TABLE Device (
    DeviceID INT IDENTITY(1,1) PRIMARY KEY, -- Mã thiết bị, khóa chính
    DeviceName NVARCHAR(100) NOT NULL, -- Tên thiết bị
    DeviceType NVARCHAR(50), -- Loại thiết bị
    StockQuantity INT, -- Số lượng tồn kho
    SupplierID INT, -- Mã nhà cung cấp, khóa ngoại
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID) -- Khóa ngoại tham chiếu đến bảng Supplier
);
GO

-- Tạo bảng Supplier (Nhà Cung Cấp)
-- Bảng chứa thông tin nhà cung cấp thiết bị
CREATE TABLE Suppliers (
    SupplierID INT IDENTITY(1,1) PRIMARY KEY, -- Mã nhà cung cấp, khóa chính
    SupplierName NVARCHAR(100) NOT NULL, -- Tên nhà cung cấp
    ContactPerson NVARCHAR(100), -- Người liên hệ
    PhoneNumber NVARCHAR(15), -- Số điện thoại liên lạc
    Email NVARCHAR(100), -- Email liên lạc
    Address NVARCHAR(255) -- Địa chỉ nhà cung cấp
);
GO

-- Tạo bảng RetailStore (Cửa Hàng Bán Lẻ)
-- Bảng chứa thông tin cửa hàng bán lẻ
CREATE TABLE RetailStore (
    StoreID INT IDENTITY(1,1) PRIMARY KEY, -- Mã cửa hàng, khóa chính
    StoreName NVARCHAR(100), -- Tên cửa hàng
    Location NVARCHAR(255), -- Địa điểm cửa hàng
    PhoneNumber NVARCHAR(15) -- Số điện thoại liên lạc
);
GO

-- Tạo bảng Product (Sản Phẩm)
-- Bảng chứa thông tin sản phẩm
CREATE TABLE Product (
    ProductID INT IDENTITY(1,1) PRIMARY KEY, -- Mã sản phẩm, khóa chính
    ProductName NVARCHAR(100) NOT NULL, -- Tên sản phẩm
    Description NVARCHAR(MAX), -- Mô tả sản phẩm
    Price DECIMAL(10, 2), -- Giá sản phẩm
	Photo NVARCHAR(255)
);
GO

-- Tạo bảng Invoice (Hóa Đơn)
-- Bảng chứa thông tin hóa đơn của khách hàng
CREATE TABLE Invoice (
    InvoiceID INT IDENTITY(1,1) PRIMARY KEY, -- Mã hóa đơn, khóa chính
    InvoiceNumber NVARCHAR(50) UNIQUE NOT NULL, -- Số hóa đơn duy nhất
    CustomerID INT NOT NULL, -- Mã khách hàng, khóa ngoại
    Amount DECIMAL(10, 2) NOT NULL, -- Số tiền hóa đơn
    IssueDate DATE NOT NULL, -- Ngày xuất hóa đơn
    PaymentDate DATE, -- Ngày thanh toán hóa đơn (nếu có)
    Status NVARCHAR(50), -- Trạng thái hóa đơn
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID), -- Khóa ngoại tham chiếu đến bảng Customer
);



-- Tạo bảng Payment (Thanh Toán)
-- Bảng chứa thông tin thanh toán hóa đơn
CREATE TABLE Payment (
    PaymentID INT IDENTITY(1,1) PRIMARY KEY, -- Mã thanh toán, khóa chính
    InvoiceID INT NOT NULL, -- Mã hóa đơn, khóa ngoại
    PaymentDate DATE NOT NULL, -- Ngày thanh toán
    Amount DECIMAL(10, 2) NOT NULL, -- Số tiền thanh toán
    PaymentMethod NVARCHAR(50), -- Phương thức thanh toán
    FOREIGN KEY (InvoiceID) REFERENCES Invoice(InvoiceID) -- Khóa ngoại tham chiếu đến bảng Invoice
);
GO

-- Tạo bảng ServicePackage (Gói Dịch Vụ)
-- Bảng chứa thông tin gói dịch vụ
CREATE TABLE ServicePackage (
    ServicePackageID INT IDENTITY(1,1) PRIMARY KEY, -- Mã gói dịch vụ, khóa chính
    ServicePackageName NVARCHAR(100) NOT NULL, -- Tên gói dịch vụ
    ConnectionType CHAR(1), -- Loại kết nối (D: Dial-Up, B: Băng thông rộng, T: Điện thoại cố định)
    Price DECIMAL(10, 2) NOT NULL, -- Giá gói dịch vụ
    Duration NVARCHAR(50), -- Thời hạn gói dịch vụ
    Details NVARCHAR(MAX) -- Chi tiết gói dịch vụ
);
GO

-- Tạo bảng DeviceOrder (Đơn Đặt Thiết Bị)
-- Bảng chứa thông tin đơn đặt thiết bị
CREATE TABLE DeviceOrder (
    DeviceOrderID INT IDENTITY(1,1) PRIMARY KEY, -- Mã đơn đặt thiết bị, khóa chính
    OrderID INT NOT NULL, -- Mã đơn hàng, khóa ngoại
    DeviceID INT NOT NULL, -- Mã thiết bị, khóa ngoại
    Quantity INT, -- Số lượng thiết bị
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID), -- Khóa ngoại tham chiếu đến bảng Order
    FOREIGN KEY (DeviceID) REFERENCES Device(DeviceID) -- Khóa ngoại tham chiếu đến bảng Device
);
GO

-- Tạo bảng Warehouse (Kho)
-- Bảng chứa thông tin kho và số lượng thiết bị trong kho
CREATE TABLE Warehouse (
    WarehouseID INT IDENTITY(1,1) PRIMARY KEY, -- Mã kho, khóa chính
    DeviceID INT NOT NULL, -- Mã thiết bị, khóa ngoại
    StoreID INT NOT NULL, -- Mã cửa hàng, khóa ngoại
    Quantity INT, -- Số lượng thiết bị trong kho
    FOREIGN KEY (DeviceID) REFERENCES Device(DeviceID), -- Khóa ngoại tham chiếu đến bảng Device
    FOREIGN KEY (StoreID) REFERENCES RetailStore(StoreID) -- Khóa ngoại tham chiếu đến bảng RetailStore
);
GO

-- Tạo bảng Discount (Giảm Giá)
-- Bảng chứa thông tin giảm giá
CREATE TABLE Discount (
    DiscountID INT IDENTITY(1,1) PRIMARY KEY, -- Mã giảm giá, khóa chính
    Description NVARCHAR(255), -- Mô tả giảm giá
    DiscountRate DECIMAL(5, 2), -- Tỷ lệ giảm giá
    Conditions NVARCHAR(255) -- Điều kiện áp dụng giảm giá
);
GO

-- Tạo bảng ConnectionPackage (Gói Kết Nối)
-- Bảng liên kết giữa kết nối và gói dịch vụ
CREATE TABLE ConnectionPackage (
    ConnectionID INT NOT NULL, -- Mã kết nối, khóa ngoại
    ServicePackageID INT NOT NULL, -- Mã gói dịch vụ, khóa ngoại
    StartDate DATE NOT NULL, -- Ngày bắt đầu áp dụng gói dịch vụ
    EndDate DATE, -- Ngày kết thúc áp dụng gói dịch vụ (nếu có)
    PRIMARY KEY (ConnectionID, ServicePackageID), -- Khóa chính kép
    FOREIGN KEY (ConnectionID) REFERENCES Connections(ConnectionID), -- Khóa ngoại tham chiếu đến bảng Connection
    FOREIGN KEY (ServicePackageID) REFERENCES ServicePackage(ServicePackageID) -- Khóa ngoại tham chiếu đến bảng ServicePackage
);
GO

-- Tạo bảng ConnectionRequest (Yêu Cầu Kết Nối)
-- Bảng chứa thông tin yêu cầu kết nối từ khách hàng
CREATE TABLE ConnectionRequest (
    RequestID INT IDENTITY(1,1) PRIMARY KEY, -- Mã yêu cầu, khóa chính
    CustomerID INT NOT NULL, -- Mã khách hàng, khóa ngoại
    ConnectionType CHAR(1), -- Loại kết nối (D: Dial-Up, B: Băng thông rộng, T: Điện thoại cố định)
    RequestDate DATE NOT NULL, -- Ngày yêu cầu kết nối
    Status NVARCHAR(50), -- Trạng thái yêu cầu kết nối
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) -- Khóa ngoại tham chiếu đến bảng Customer
);
GO

-- Tạo bảng TransactionLog (Nhật Ký Giao Dịch)
-- Bảng chứa thông tin nhật ký giao dịch của khách hàng
CREATE TABLE TransactionLog (
    TransactionID INT IDENTITY(1,1) PRIMARY KEY, -- Mã giao dịch, khóa chính
    CustomerID INT NOT NULL, -- Mã khách hàng, khóa ngoại
    TransactionType NVARCHAR(50), -- Loại giao dịch
    TransactionDate DATETIME DEFAULT GETDATE(), -- Ngày giờ giao dịch
    Details NVARCHAR(MAX), -- Chi tiết giao dịch
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) -- Khóa ngoại tham chiếu đến bảng Customer
);
GO

-- Tạo bảng CustomerFeedback (Phản Hồi Khách Hàng)
-- Bảng chứa thông tin phản hồi từ khách hàng
CREATE TABLE CustomerFeedback (
    FeedbackID INT IDENTITY(1,1) PRIMARY KEY, -- Mã phản hồi, khóa chính
    CustomerID INT NOT NULL, -- Mã khách hàng, khóa ngoại
    FeedbackDate DATE NOT NULL, -- Ngày phản hồi
    Feedback NVARCHAR(MAX), -- Nội dung phản hồi
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) -- Khóa ngoại tham chiếu đến bảng Customer
);
GO

-- Tạo bảng Finance (Tài Chính)
-- Bảng chứa thông tin tài chính của khách hàng
CREATE TABLE Finance (
    FinanceID INT IDENTITY(1,1) PRIMARY KEY, -- Mã tài chính, khóa chính
    CustomerID INT NOT NULL, -- Mã khách hàng, khóa ngoại
    ConnectionID INT NOT NULL, -- Mã kết nối, khóa ngoại
    TotalAmount DECIMAL(10, 2), -- Tổng số tiền
    AmountPaid DECIMAL(10, 2), -- Số tiền đã thanh toán
    RemainingAmount DECIMAL(10, 2), -- Số tiền còn lại
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID), -- Khóa ngoại tham chiếu đến bảng Customer
    FOREIGN KEY (ConnectionID) REFERENCES Connections(ConnectionID) -- Khóa ngoại tham chiếu đến bảng Connection
);
GO

CREATE TABLE ContactMessages (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    
);