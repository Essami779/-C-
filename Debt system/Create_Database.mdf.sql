-- ===============================================
-- إنشاء قاعدة بيانات نظام الديون Database.mdf
-- ===============================================

-- إنشاء قاعدة البيانات
CREATE DATABASE [Database]
ON 
( NAME = 'Database',
  FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Database.mdf',
  SIZE = 10MB,
  MAXSIZE = 100MB,
  FILEGROWTH = 5MB )
LOG ON 
( NAME = 'Database_log',
  FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Database_log.ldf',
  SIZE = 5MB,
  MAXSIZE = 50MB,
  FILEGROWTH = 2MB );
GO

-- استخدام قاعدة البيانات
USE [Database];
GO

-- ===============================================
-- إنشاء الجداول
-- ===============================================

-- إنشاء جدول العملاء
CREATE TABLE [dbo].[Customers] (
    [CustomerID] INT IDENTITY(1,1) NOT NULL,
    [CustomerName] NVARCHAR(100) NOT NULL,
    [Phone] NVARCHAR(20) NOT NULL,
    [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([CustomerID] ASC)
);
GO

-- إنشاء جدول المنتجات
CREATE TABLE [dbo].[Products] (
    [ProductID] INT IDENTITY(1,1) NOT NULL,
    [ProductName] NVARCHAR(100) NOT NULL,
    [Price] DECIMAL(10,2) NOT NULL,
    [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([ProductID] ASC)
);
GO

-- إنشاء جدول المبيعات
CREATE TABLE [dbo].[Sales] (
    [SaleID] INT IDENTITY(1,1) NOT NULL,
    [CustomerID] INT NOT NULL,
    [ProductID] INT NOT NULL,
    [Quantity] INT NOT NULL DEFAULT 1,
    [UnitPrice] DECIMAL(10,2) NOT NULL,
    [Discount] DECIMAL(10,2) NOT NULL DEFAULT 0,
    [TotalAmount] DECIMAL(10,2) NOT NULL,
    [PaidAmount] DECIMAL(10,2) NOT NULL DEFAULT 0,
    [SaleDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [Notes] NVARCHAR(500) NULL,
    [IsCompleted] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED ([SaleID] ASC),
    CONSTRAINT [FK_Sales_Customers] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID]),
    CONSTRAINT [FK_Sales_Products] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ProductID])
);
GO

-- إنشاء جدول المدفوعات
CREATE TABLE [dbo].[Payments] (
    [PaymentID] INT IDENTITY(1,1) NOT NULL,
    [SaleID] INT NOT NULL,
    [PaymentAmount] DECIMAL(10,2) NOT NULL,
    [PaymentDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [PaymentMethod] NVARCHAR(50) NOT NULL DEFAULT 'نقدي',
    [Notes] NVARCHAR(500) NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED ([PaymentID] ASC),
    CONSTRAINT [FK_Payments_Sales] FOREIGN KEY ([SaleID]) REFERENCES [dbo].[Sales] ([SaleID])
);
GO

-- ===============================================
-- إنشاء الفهارس لتحسين الأداء
-- ===============================================

-- فهرس على جدول المبيعات
CREATE NONCLUSTERED INDEX [IX_Sales_CustomerID] ON [dbo].[Sales] ([CustomerID]);
CREATE NONCLUSTERED INDEX [IX_Sales_ProductID] ON [dbo].[Sales] ([ProductID]);
CREATE NONCLUSTERED INDEX [IX_Sales_SaleDate] ON [dbo].[Sales] ([SaleDate]);
CREATE NONCLUSTERED INDEX [IX_Sales_IsCompleted] ON [dbo].[Sales] ([IsCompleted]);

-- فهرس على جدول المدفوعات
CREATE NONCLUSTERED INDEX [IX_Payments_SaleID] ON [dbo].[Payments] ([SaleID]);
CREATE NONCLUSTERED INDEX [IX_Payments_PaymentDate] ON [dbo].[Payments] ([PaymentDate]);

-- فهرس على جدول العملاء
CREATE NONCLUSTERED INDEX [IX_Customers_Phone] ON [dbo].[Customers] ([Phone]);
CREATE NONCLUSTERED INDEX [IX_Customers_IsActive] ON [dbo].[Customers] ([IsActive]);

-- فهرس على جدول المنتجات
CREATE NONCLUSTERED INDEX [IX_Products_IsActive] ON [dbo].[Products] ([IsActive]);
GO

-- ===============================================
-- إنشاء العمليات المخزنة (Stored Procedures)
-- ===============================================

-- عملية حساب المبلغ المتبقي
CREATE PROCEDURE [dbo].[CalculateRemainingAmount]
    @SaleID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE [dbo].[Sales]
    SET [PaidAmount] = ISNULL((
        SELECT SUM([PaymentAmount])
        FROM [dbo].[Payments]
        WHERE [SaleID] = @SaleID
    ), 0)
    WHERE [SaleID] = @SaleID;
END;
GO

-- عملية إدراج دفعة جديدة
CREATE PROCEDURE [dbo].[InsertPayment]
    @SaleID INT,
    @PaymentAmount DECIMAL(10,2),
    @PaymentMethod NVARCHAR(50) = 'نقدي',
    @Notes NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- إدراج الدفعة
        INSERT INTO [dbo].[Payments] ([SaleID], [PaymentAmount], [PaymentMethod], [Notes])
        VALUES (@SaleID, @PaymentAmount, @PaymentMethod, @Notes);
        
        -- تحديث المبلغ المدفوع
        EXEC [dbo].[CalculateRemainingAmount] @SaleID;
        
        COMMIT TRANSACTION;
        SELECT 1 AS Success;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 0 AS Success, ERROR_MESSAGE() AS ErrorMessage;
    END CATCH;
END;
GO

-- عملية الحصول على إجمالي الديون
CREATE PROCEDURE [dbo].[GetTotalDebts]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        ISNULL(SUM([TotalAmount] - [PaidAmount]), 0) AS TotalDebts
    FROM [dbo].[Sales]
    WHERE ([TotalAmount] - [PaidAmount]) > 0;
END;
GO

-- ===============================================
-- إدراج بيانات تجريبية
-- ===============================================

-- إدراج عملاء تجريبيين
INSERT INTO [dbo].[Customers] ([CustomerName], [Phone]) VALUES 
('أحمد محمد علي', '01234567890'),
('فاطمة حسن', '01111111111'),
('محمد عبدالله', '02222222222'),
('سارة أحمد', '03333333333'),
('علي محمود', '04444444444');
GO

-- إدراج منتجات تجريبية
INSERT INTO [dbo].[Products] ([ProductName], [Price]) VALUES 
('قميص قطني', 50.00),
('بنطلون جينز', 80.00),
('حذاء رياضي', 120.00),
('جاكيت شتوي', 150.00),
('حقيبة يد', 60.00),
('ساعة ذكية', 200.00),
('نظارة شمسية', 30.00),
('حزام جلد', 25.00);
GO

-- إدراج مبيعات تجريبية
INSERT INTO [dbo].[Sales] ([CustomerID], [ProductID], [UnitPrice], [Discount], [TotalAmount], [Notes]) VALUES 
(1, 1, 50.00, 5.00, 45.00, 'مبيعة تجريبية'),
(2, 2, 80.00, 0.00, 80.00, 'بدون خصم'),
(3, 3, 120.00, 10.00, 110.00, 'خصم خاص'),
(1, 4, 150.00, 15.00, 135.00, 'عميل VIP'),
(4, 5, 60.00, 0.00, 60.00, 'دفع نقدي');
GO

-- إدراج مدفوعات تجريبية
INSERT INTO [dbo].[Payments] ([SaleID], [PaymentAmount], [PaymentMethod], [Notes]) VALUES 
(1, 25.00, 'نقدي', 'دفعة أولى'),
(1, 20.00, 'نقدي', 'دفعة ثانية'),
(2, 80.00, 'نقدي', 'دفع كامل'),
(3, 50.00, 'بطاقة ائتمان', 'دفعة أولى');
GO

-- تحديث المبالغ المدفوعة
EXEC [dbo].[CalculateRemainingAmount] 1;
EXEC [dbo].[CalculateRemainingAmount] 2;
EXEC [dbo].[CalculateRemainingAmount] 3;
EXEC [dbo].[CalculateRemainingAmount] 4;
EXEC [dbo].[CalculateRemainingAmount] 5;
GO

-- ===============================================
-- إنشاء Views للتقارير
-- ===============================================

-- عرض المبيعات مع تفاصيل العملاء والمنتجات
CREATE VIEW [dbo].[SalesDetails] AS
SELECT 
    s.[SaleID],
    c.[CustomerName],
    c.[Phone],
    p.[ProductName],
    s.[UnitPrice],
    s.[Discount],
    s.[TotalAmount],
    s.[PaidAmount],
    (s.[TotalAmount] - s.[PaidAmount]) AS [RemainingAmount],
    s.[SaleDate],
    s.[Notes],
    CASE 
        WHEN (s.[TotalAmount] - s.[PaidAmount]) = 0 THEN 'مدفوع بالكامل'
        WHEN s.[PaidAmount] > 0 THEN 'مدفوع جزئياً'
        ELSE 'غير مدفوع'
    END AS [PaymentStatus]
FROM [dbo].[Sales] s
INNER JOIN [dbo].[Customers] c ON s.[CustomerID] = c.[CustomerID]
INNER JOIN [dbo].[Products] p ON s.[ProductID] = p.[ProductID];
GO

-- عرض إحصائيات العملاء
CREATE VIEW [dbo].[CustomerStatistics] AS
SELECT 
    c.[CustomerID],
    c.[CustomerName],
    c.[Phone],
    COUNT(s.[SaleID]) AS [TotalSales],
    ISNULL(SUM(s.[TotalAmount]), 0) AS [TotalPurchases],
    ISNULL(SUM(s.[PaidAmount]), 0) AS [TotalPaid],
    ISNULL(SUM(s.[TotalAmount] - s.[PaidAmount]), 0) AS [TotalDebts]
FROM [dbo].[Customers] c
LEFT JOIN [dbo].[Sales] s ON c.[CustomerID] = s.[CustomerID]
WHERE c.[IsActive] = 1
GROUP BY c.[CustomerID], c.[CustomerName], c.[Phone];
GO

-- ===============================================
-- إنشاء Triggers
-- ===============================================

-- Trigger لتحديث المبلغ المدفوع عند إدراج دفعة جديدة
CREATE TRIGGER [dbo].[TR_Payments_Insert]
ON [dbo].[Payments]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE s
    SET s.[PaidAmount] = ISNULL((
        SELECT SUM([PaymentAmount])
        FROM [dbo].[Payments]
        WHERE [SaleID] = s.[SaleID]
    ), 0)
    FROM [dbo].[Sales] s
    INNER JOIN inserted i ON s.[SaleID] = i.[SaleID];
END;
GO

-- Trigger لتحديث المبلغ المدفوع عند حذف دفعة
CREATE TRIGGER [dbo].[TR_Payments_Delete]
ON [dbo].[Payments]
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE s
    SET s.[PaidAmount] = ISNULL((
        SELECT SUM([PaymentAmount])
        FROM [dbo].[Payments]
        WHERE [SaleID] = s.[SaleID]
    ), 0)
    FROM [dbo].[Sales] s
    INNER JOIN deleted d ON s.[SaleID] = d.[SaleID];
END;
GO

-- ===============================================
-- إنهاء الإعداد
-- ===============================================

PRINT '===============================================';
PRINT 'تم إنشاء قاعدة البيانات Database.mdf بنجاح!';
PRINT '===============================================';
PRINT 'تم إنشاء الجداول التالية:';
PRINT '- Customers (العملاء)';
PRINT '- Products (المنتجات)';
PRINT '- Sales (المبيعات)';
PRINT '- Payments (المدفوعات)';
PRINT '===============================================';
PRINT 'تم إنشاء العمليات المخزنة:';
PRINT '- CalculateRemainingAmount';
PRINT '- InsertPayment';
PRINT '- GetTotalDebts';
PRINT '===============================================';
PRINT 'تم إنشاء Views:';
PRINT '- SalesDetails';
PRINT '- CustomerStatistics';
PRINT '===============================================';
PRINT 'تم إدراج بيانات تجريبية للاختبار';
PRINT '===============================================';
