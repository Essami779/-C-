using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Debt_system
{
    public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DebtSystemConnection"].ConnectionString;
        }

        // فتح الاتصال بقاعدة البيانات
        private SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // تنفيذ استعلام SELECT وإرجاع DataTable
        public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في قاعدة البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dataTable;
        }

        // تنفيذ استعلام INSERT, UPDATE, DELETE وإرجاع عدد الصفوف المتأثرة
        public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في قاعدة البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return rowsAffected;
        }

        // إدراج عميل جديد
        public int InsertCustomer(string customerName, string phone)
        {
            string query = "INSERT INTO Customers (CustomerName, Phone) VALUES (@CustomerName, @Phone); SELECT SCOPE_IDENTITY();";
            SqlParameter[] parameters = {
                new SqlParameter("@CustomerName", customerName),
                new SqlParameter("@Phone", phone)
            };
            
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في إدراج العميل: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        // إدراج منتج جديد
        public int InsertProduct(string productName, decimal price)
        {
            string query = "INSERT INTO Products (ProductName, Price) VALUES (@ProductName, @Price); SELECT SCOPE_IDENTITY();";
            SqlParameter[] parameters = {
                new SqlParameter("@ProductName", productName),
                new SqlParameter("@Price", price)
            };
            
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في إدراج المنتج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        // إدراج مبيعة جديدة
        public int InsertSale(int customerID, int productID, decimal unitPrice, decimal discount, decimal totalAmount, string notes = "")
        {
            string query = @"INSERT INTO Sales (CustomerID, ProductID, UnitPrice, Discount, TotalAmount, Notes) 
                           VALUES (@CustomerID, @ProductID, @UnitPrice, @Discount, @TotalAmount, @Notes); 
                           SELECT SCOPE_IDENTITY();";
            SqlParameter[] parameters = {
                new SqlParameter("@CustomerID", customerID),
                new SqlParameter("@ProductID", productID),
                new SqlParameter("@UnitPrice", unitPrice),
                new SqlParameter("@Discount", discount),
                new SqlParameter("@TotalAmount", totalAmount),
                new SqlParameter("@Notes", notes)
            };
            
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في إدراج المبيعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        // تحديث مبيعة
        public bool UpdateSale(int saleID, int customerID, int productID, decimal unitPrice, decimal discount, decimal totalAmount, string notes = "")
        {
            string query = @"UPDATE Sales SET CustomerID = @CustomerID, ProductID = @ProductID, 
                           UnitPrice = @UnitPrice, Discount = @Discount, TotalAmount = @TotalAmount, 
                           Notes = @Notes WHERE SaleID = @SaleID";
            SqlParameter[] parameters = {
                new SqlParameter("@SaleID", saleID),
                new SqlParameter("@CustomerID", customerID),
                new SqlParameter("@ProductID", productID),
                new SqlParameter("@UnitPrice", unitPrice),
                new SqlParameter("@Discount", discount),
                new SqlParameter("@TotalAmount", totalAmount),
                new SqlParameter("@Notes", notes)
            };
            
            return ExecuteNonQuery(query, parameters) > 0;
        }

        // حذف مبيعة
        public bool DeleteSale(int saleID)
        {
            string query = "DELETE FROM Sales WHERE SaleID = @SaleID";
            SqlParameter[] parameters = {
                new SqlParameter("@SaleID", saleID)
            };
            
            return ExecuteNonQuery(query, parameters) > 0;
        }

        // إدراج دفعة مع التحقق من صحة المبلغ
        public bool InsertPayment(int saleID, decimal paymentAmount, string paymentMethod = "نقدي", string notes = "")
        {
            try
            {
                // التحقق من أن المبلغ أكبر من صفر
                if (paymentAmount <= 0)
                {
                    MessageBox.Show("مبلغ الدفعة يجب أن يكون أكبر من صفر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // التحقق من وجود المبيعة
                if (!SaleExists(saleID))
                {
                    MessageBox.Show("المبيعة المحددة غير موجودة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // التحقق من أن المبلغ لا يتجاوز المبلغ المتبقي
                decimal remainingAmount = GetRemainingAmount(saleID);
                if (paymentAmount > remainingAmount)
                {
                    MessageBox.Show($"مبلغ الدفعة ({paymentAmount:F2}) أكبر من المبلغ المتبقي ({remainingAmount:F2})", 
                        "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string query = @"INSERT INTO Payments (SaleID, PaymentAmount, PaymentMethod, Notes) 
                               VALUES (@SaleID, @PaymentAmount, @PaymentMethod, @Notes)";
                SqlParameter[] parameters = {
                    new SqlParameter("@SaleID", saleID),
                    new SqlParameter("@PaymentAmount", paymentAmount),
                    new SqlParameter("@PaymentMethod", paymentMethod ?? "نقدي"),
                    new SqlParameter("@Notes", notes ?? "")
                };
                
                bool result = ExecuteNonQuery(query, parameters) > 0;
                
                if (result)
                {
                    // تحديث المبلغ المدفوع في جدول المبيعات
                    UpdatePaidAmount(saleID);
                    
                    // التحقق من اكتمال الدفع
                    CheckPaymentCompletion(saleID);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في إدراج الدفعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // تحديث المبلغ المدفوع
        private void UpdatePaidAmount(int saleID)
        {
            string query = @"UPDATE Sales SET PaidAmount = (
                               SELECT ISNULL(SUM(PaymentAmount), 0) 
                               FROM Payments 
                               WHERE SaleID = @SaleID
                           ) WHERE SaleID = @SaleID";
            SqlParameter[] parameters = {
                new SqlParameter("@SaleID", saleID)
            };
            
            ExecuteNonQuery(query, parameters);
        }

        // الحصول على المبلغ المتبقي لمبيعة معينة
        public decimal GetRemainingAmount(int saleID)
        {
            try
            {
                string query = @"SELECT (TotalAmount - ISNULL(PaidAmount, 0)) AS RemainingAmount 
                               FROM Sales WHERE SaleID = @SaleID";
                SqlParameter[] parameters = {
                    new SqlParameter("@SaleID", saleID)
                };
                
                DataTable result = ExecuteQuery(query, parameters);
                if (result.Rows.Count > 0 && result.Rows[0]["RemainingAmount"] != DBNull.Value)
                {
                    return Convert.ToDecimal(result.Rows[0]["RemainingAmount"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في حساب المبلغ المتبقي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        // التحقق من اكتمال الدفع
        private void CheckPaymentCompletion(int saleID)
        {
            decimal remainingAmount = GetRemainingAmount(saleID);
            if (remainingAmount <= 0)
            {
                // تحديث حالة المبيعة كمكتملة
                string query = "UPDATE Sales SET IsCompleted = 1 WHERE SaleID = @SaleID";
                SqlParameter[] parameters = {
                    new SqlParameter("@SaleID", saleID)
                };
                ExecuteNonQuery(query, parameters);
            }
        }

        // الحصول على تفاصيل المدفوعات لمبيعة معينة
        public DataTable GetPaymentDetails(int saleID)
        {
            string query = @"SELECT 
                           p.PaymentID,
                           p.PaymentAmount,
                           p.PaymentDate,
                           p.PaymentMethod,
                           p.Notes,
                           s.TotalAmount,
                           s.PaidAmount,
                           (s.TotalAmount - s.PaidAmount) AS RemainingAmount
                           FROM Payments p
                           INNER JOIN Sales s ON p.SaleID = s.SaleID
                           WHERE p.SaleID = @SaleID
                           ORDER BY p.PaymentDate DESC";
            SqlParameter[] parameters = {
                new SqlParameter("@SaleID", saleID)
            };
            
            return ExecuteQuery(query, parameters);
        }

        // الحصول على إجمالي المدفوعات لعميل معين
        public decimal GetCustomerTotalPayments(int customerID)
        {
            string query = @"SELECT ISNULL(SUM(p.PaymentAmount), 0) AS TotalPayments
                           FROM Payments p
                           INNER JOIN Sales s ON p.SaleID = s.SaleID
                           WHERE s.CustomerID = @CustomerID";
            SqlParameter[] parameters = {
                new SqlParameter("@CustomerID", customerID)
            };
            
            DataTable result = ExecuteQuery(query, parameters);
            if (result.Rows.Count > 0 && result.Rows[0]["TotalPayments"] != DBNull.Value)
            {
                return Convert.ToDecimal(result.Rows[0]["TotalPayments"]);
            }
            return 0;
        }

        // الحصول على إحصائيات المدفوعات
        public DataTable GetPaymentStatistics()
        {
            string query = @"SELECT 
                           COUNT(*) AS TotalPayments,
                           ISNULL(SUM(PaymentAmount), 0) AS TotalPaymentAmount,
                           ISNULL(AVG(PaymentAmount), 0) AS AveragePaymentAmount,
                           MIN(PaymentDate) AS FirstPaymentDate,
                           MAX(PaymentDate) AS LastPaymentDate
                           FROM Payments";
            
            return ExecuteQuery(query);
        }

        // الحصول على جميع المبيعات مع تفاصيل العملاء والمنتجات
        public DataTable GetAllSales()
        {
            string query = @"SELECT s.SaleID, c.CustomerName, c.Phone, p.ProductName, 
                           s.UnitPrice, s.Discount, s.TotalAmount, s.PaidAmount, 
                           s.RemainingAmount, s.SaleDate, s.Notes
                           FROM Sales s
                           INNER JOIN Customers c ON s.CustomerID = c.CustomerID
                           INNER JOIN Products p ON s.ProductID = p.ProductID
                           ORDER BY s.SaleDate DESC";
            
            return ExecuteQuery(query);
        }

        // البحث في المبيعات
        public DataTable SearchSales(string searchTerm)
        {
            string query = @"SELECT s.SaleID, c.CustomerName, c.Phone, p.ProductName, 
                           s.UnitPrice, s.Discount, s.TotalAmount, s.PaidAmount, 
                           s.RemainingAmount, s.SaleDate, s.Notes
                           FROM Sales s
                           INNER JOIN Customers c ON s.CustomerID = c.CustomerID
                           INNER JOIN Products p ON s.ProductID = p.ProductID
                           WHERE c.CustomerName LIKE @SearchTerm 
                           OR c.Phone LIKE @SearchTerm 
                           OR p.ProductName LIKE @SearchTerm
                           ORDER BY s.SaleDate DESC";
            
            SqlParameter[] parameters = {
                new SqlParameter("@SearchTerm", $"%{searchTerm}%")
            };
            
            return ExecuteQuery(query, parameters);
        }

        // الحصول على العملاء
        public DataTable GetCustomers()
        {
            string query = "SELECT CustomerID, CustomerName, Phone FROM Customers WHERE IsActive = 1 ORDER BY CustomerName";
            return ExecuteQuery(query);
        }

        // الحصول على المنتجات
        public DataTable GetProducts()
        {
            string query = "SELECT ProductID, ProductName, Price FROM Products WHERE IsActive = 1 ORDER BY ProductName";
            return ExecuteQuery(query);
        }

        // الحصول على إجمالي الديون
        public decimal GetTotalDebts()
        {
            string query = "SELECT ISNULL(SUM(RemainingAmount), 0) FROM Sales WHERE RemainingAmount > 0";
            DataTable result = ExecuteQuery(query);
            
            if (result.Rows.Count > 0 && result.Rows[0][0] != DBNull.Value)
            {
                return Convert.ToDecimal(result.Rows[0][0]);
            }
            return 0;
        }

        // التحقق من وجود مبيعة
        private bool SaleExists(int saleID)
        {
            string query = "SELECT COUNT(*) FROM Sales WHERE SaleID = @SaleID";
            SqlParameter[] parameters = {
                new SqlParameter("@SaleID", saleID)
            };
            
            DataTable result = ExecuteQuery(query, parameters);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0][0]) > 0;
        }

        // اختبار الاتصال بقاعدة البيانات
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
