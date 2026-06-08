USE master;
GO

-- 如果資料庫已存在則先刪除，確保腳本可重複執行
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'Northwind')
BEGIN
    ALTER DATABASE Northwind SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Northwind;
END
GO

CREATE DATABASE Northwind;
GO

USE Northwind;
GO

-- 1. 建立供應商資料表
CREATE TABLE Suppliers (
    SupplierID INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName NVARCHAR(40) NOT NULL,
    ContactName NVARCHAR(30) NULL
);

-- 2. 建立類別資料表
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(15) NOT NULL,
    [Description] NTEXT NULL
);

-- 3. 建立商品資料表
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(40) NOT NULL,
    SupplierID INT NULL FOREIGN KEY REFERENCES Suppliers(SupplierID),
    CategoryID INT NULL FOREIGN KEY REFERENCES Categories(CategoryID),
    QuantityPerUnit NVARCHAR(20) NULL,
    UnitPrice MONEY DEFAULT 0,
    UnitsInStock SMALLINT DEFAULT 0,
    Discontinued BIT NOT NULL DEFAULT 0
);

-- 4. 建立員工資料表 (簡潔版，適合系統登入)
CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    LastName NVARCHAR(20) NOT NULL,
    FirstName NVARCHAR(10) NOT NULL,
    Title NVARCHAR(30) NULL,
    Username NVARCHAR(50) NOT NULL UNIQUE, 
    [Password] NVARCHAR(100) NOT NULL,    
    [Role] NVARCHAR(20) NOT NULL DEFAULT 'Employee' 
);
GO

-- 5. 灌入資料
-- 供應商
INSERT INTO Suppliers (CompanyName, ContactName) VALUES 
(N'Exotic Liquids', N'Charlotte Cooper'),
(N'New Orleans Cajun Delights', N'Shelley Burke'),
(N'Grandma Kelly''s Homestead', N'Regina Murphy');

-- 類別
INSERT INTO Categories (CategoryName, [Description]) VALUES 
(N'Beverages', N'Soft drinks, coffees, teas, beers, and ales'),
(N'Condiments', N'Sweet and savory sauces, relishes, spreads, and seasonings'),
(N'Confections', N'Desserts, candies, and sweet breads');

-- 商品
INSERT INTO Products (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, Discontinued) VALUES
(N'Chai', 1, 1, N'10 boxes x 20 bags', 18.00, 39, 0),
(N'Chang', 1, 1, N'24 - 12 oz bottles', 19.00, 17, 0),
(N'Aniseed Syrup', 1, 2, N'12 - 550 ml bottles', 10.00, 13, 0),
(N'Chef Anton''s Cajun Seasoning', 2, 2, N'48 - 6 oz jars', 22.00, 53, 0),
(N'Chef Anton''s Gumbo Mix', 2, 2, N'36 boxes', 21.35, 0, 1),
(N'Grandma''s Boysenberry Spread', 3, 2, N'12 - 8 oz jars', 25.00, 120, 0),
(N'Uncle Bob''s Organic Dried Pears', 3, 3, N'12 - 1 lb pkgs', 30.00, 15, 0),
(N'蘋果汁 (Apple Juice)', 1, 1, N'24 - 12 oz cans', 15.50, 100, 0);

-- 員工
INSERT INTO Employees (LastName, FirstName, Title, Username, [Password], [Role]) VALUES 
(N'Davolio', N'Nancy', N'Sales Representative', N'nancy', N'123456', N'Employee'),
(N'Waston', N'John', N'Detective', N'john', N'123456', N'Employee'),
(N'Hikaru', N'Nakamura', N'Chess GrandMaster', N'hikaru', N'123456', N'Employee'),
(N'Fuller', N'Andrew', N'Vice President, Sales', N'andrew', N'123456', N'Admin'),
(N'Tu', N'BoYu', N'Technical Lead', N'admin', N'admin123', N'Admin');
GO