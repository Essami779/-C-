-- إنشاء قاعدة بيانات نظام الديون
USE master;
GO

-- إنشاء قاعدة البيانات إذا لم تكن موجودة
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'DebtSystemDB')
BEGIN
    CREATE DATABASE DebtSystemDB;
END
GO

USE DebtSystemDB;
GO

-- إنشاء جدول العملاء
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Customers' AND xtype='U')
BEGIN
    CREATE TABLE Customers (
        CustomerID INT IDENTITY(1,1) PRIMARY KEY,
        CustomerName NVARCHAR(100) NOT NULL,
        Phone NVARCHAR(20) NOT NULL,
        CreatedDate DATETIME DEFAULT GETDATE(),
        IsActive BIT DEFAULT 1
    );
END
GO

-- إنشاء جدول المنتجات/المواد
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
BEGIN
    CREATE TABLE Products (
        ProductID INT IDENTITY(1,1) PRIMARY KEY,
        ProductName NVARCHAR(100) NOT NULL,
        Price DECIMAL(10,2) NOT NULL,
        CreatedDate DATETIME DEFAULT GETDATE(),
        IsActive BIT DEFAULT 1
    );
END
GO

-- إنشاء جدول المبيعات/الديون
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Sales' AND xtype='U')
BEGIN
    CREATE TABLE Sales (
        SaleID INT IDENTITY(1,1) PRIMARY KEY,
        CustomerID INT NOT NULL,
        ProductID INT NOT NULL,
        Quantity INT DEFAULT 1,
        UnitPrice DECIMAL(10,2) NOT NULL,
        Discount DECIMAL(10,2) DEFAULT 0,
        TotalAmount DECIMAL(10,2) NOT NULL,
        PaidAmount DECIMAL(10,2) DEFAULT 0,
        RemainingAmount AS (TotalAmount - PaidAmount),
        SaleDate DATETIME DEFAULT GETDATE(),
        Notes NVARCHAR(500),
        IsCompleted BIT DEFAULT 0,
        FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
        FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
    );
END
GO

-- إنشاء جدول المدفوعات
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Payments' AND xtype='U')
BEGIN
    CREATE TABLE Payments (
        PaymentID INT IDENTITY(1,1) PRIMARY KEY,
        SaleID INT NOT NULL,
        PaymentAmount DECIMAL(10,2) NOT NULL,
        PaymentDate DATETIME DEFAULT GETDATE(),
        PaymentMethod NVARCHAR(50) DEFAULT 'نقدي',
        Notes NVARCHAR(500),
        FOREIGN KEY (SaleID) REFERENCES Sales(SaleID)
    );
END
GO

-- إدراج بيانات تجريبية
-- إدراج عملاء تجريبيين
IF NOT EXISTS (SELECT * FROM Customers WHERE CustomerName = 'عميل تجريبي')
BEGIN
    INSERT INTO Customers (CustomerName, Phone) VALUES 
    ('عميل تجريبي', '01234567890'),
    ('أحمد محمد', '01111111111'),
    ('فاطمة علي', '02222222222');
END
GO

-- إدراج منتجات تجريبية
IF NOT EXISTS (SELECT * FROM Products WHERE ProductName = 'منتج تجريبي')
BEGIN
    INSERT INTO Products (ProductName, Price) VALUES 
    ('منتج تجريبي', 100.00),
    ('قميص', 50.00),
    ('بنطلون', 80.00),
    ('حذاء', 120.00);
END
GO

-- إنشاء فهارس لتحسين الأداء
CREATE NONCLUSTERED INDEX IX_Sales_CustomerID ON Sales(CustomerID);
CREATE NONCLUSTERED INDEX IX_Sales_SaleDate ON Sales(SaleDate);
CREATE NONCLUSTERED INDEX IX_Payments_SaleID ON Payments(SaleID);
GO

PRINT 'تم إنشاء قاعدة البيانات والجداول بنجاح!';
