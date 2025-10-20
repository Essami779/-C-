using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;

namespace Debt_system
{
    public partial class system : Form
    {
        private DatabaseManager dbManager;
        private DataTable currentSalesData;
        private int selectedSaleID = -1;

        public system()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // اختبار الاتصال بقاعدة البيانات
            if (!dbManager.TestConnection())
            {
                MessageBox.Show("لا يمكن الاتصال بقاعدة البيانات. تأكد من وجود SQL Server LocalDB.", 
                    "خطأ في الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // إعداد ListView
            SetupListView();
            
            // تحميل البيانات
            LoadAllSales();
            
            // تحديث الإجمالي
            UpdateTotalAmount();
            
            // إعداد التاريخ الحالي
            dateTime.Value = DateTime.Now;
        }

        private void SetupListView()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            
            // إضافة الأعمدة
            listView1.Columns.Clear();
            listView1.Columns.Add("ID", 50);
            listView1.Columns.Add("الاسم", 120);
            listView1.Columns.Add("الهاتف", 100);
            listView1.Columns.Add("اسم الصنف", 100);
            listView1.Columns.Add("السعر", 80);
            listView1.Columns.Add("الخصم", 80);
            listView1.Columns.Add("الإجمالي", 80);
            listView1.Columns.Add("المدفوع", 80);
            listView1.Columns.Add("المتبقي", 80);
            listView1.Columns.Add("التاريخ", 100);
        }

        private void LoadAllSales()
        {
            currentSalesData = dbManager.GetAllSales();
            PopulateListView(currentSalesData);
        }

        private void PopulateListView(DataTable data)
        {
            try
            {
                listView1.Items.Clear();
                
                foreach (DataRow row in data.Rows)
                {
                    ListViewItem item = new ListViewItem(row["SaleID"].ToString());
                    item.SubItems.Add(row["CustomerName"].ToString());
                    item.SubItems.Add(row["Phone"].ToString());
                    item.SubItems.Add(row["ProductName"].ToString());
                    item.SubItems.Add(Convert.ToDecimal(row["UnitPrice"]).ToString("F2"));
                    item.SubItems.Add(Convert.ToDecimal(row["Discount"]).ToString("F2"));
                    item.SubItems.Add(Convert.ToDecimal(row["TotalAmount"]).ToString("F2"));
                    item.SubItems.Add(Convert.ToDecimal(row["PaidAmount"]).ToString("F2"));
                    item.SubItems.Add(Convert.ToDecimal(row["RemainingAmount"]).ToString("F2"));
                    item.SubItems.Add(Convert.ToDateTime(row["SaleDate"]).ToString("yyyy-MM-dd"));
                    
                    // تلوين الصفوف حسب حالة الدفع
                    decimal remainingAmount = Convert.ToDecimal(row["RemainingAmount"]);
                    decimal paidAmount = Convert.ToDecimal(row["PaidAmount"]);
                    
                    if (remainingAmount <= 0)
                    {
                        item.BackColor = Color.LightGreen; // مدفوع بالكامل
                        item.ToolTipText = "مدفوع بالكامل";
                    }
                    else if (paidAmount > 0)
                    {
                        item.BackColor = Color.LightYellow; // مدفوع جزئياً
                        item.ToolTipText = $"مدفوع جزئياً - متبقي: {remainingAmount:F2} ريال";
                    }
                    else
                    {
                        item.BackColor = Color.LightPink; // غير مدفوع
                        item.ToolTipText = $"غير مدفوع - المبلغ: {Convert.ToDecimal(row["TotalAmount"]):F2} ريال";
                    }
                    
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في عرض البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTotalAmount()
        {
            decimal totalDebts = dbManager.GetTotalDebts();
            totoalprice.Text = totalDebts.ToString("F2");
        }

        private void ClearInputFields()
        {
            name.Clear();
            phone.Clear();
            materialsave.Clear();
            price.Clear();
            dicount.Clear();

            selectedSaleID = -1;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(name.Text))
            {
                MessageBox.Show("يرجى إدخال اسم العميل", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                name.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(phone.Text))
            {
                MessageBox.Show("يرجى إدخال رقم الهاتف", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                phone.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(materialsave.Text))
            {
                MessageBox.Show("يرجى إدخال اسم الصنف", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                materialsave.Focus();
                return false;
            }

          /*
          if (!decimal.TryParse(price.Text, out decimal priceValue) || priceValue <= 0)
            {
                MessageBox.Show("يرجى إدخال سعر صحيح", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                price.Focus();
                return false;
            }

            if (!decimal.TryParse(dicount.Text, out decimal discountValue))
            {
                discountValue = 0;
            }
            */
            return true;
        }

        // أحداث الأزرار
        private void Save_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                // إدراج أو تحديث العميل
                int customerID = GetOrCreateCustomer(name.Text.Trim(), phone.Text.Trim());
                if (customerID == -1) return;

                // إدراج أو تحديث المنتج
                int productID = GetOrCreateProduct(materialsave.Text.Trim(), Convert.ToDecimal(price.Text));
                if (productID == -1) return;

                decimal unitPrice = Convert.ToDecimal(price.Text);
                decimal discount = string.IsNullOrWhiteSpace(dicount.Text) ? 0 : Convert.ToDecimal(dicount.Text);
                decimal totalAmount = unitPrice - discount;

                if (selectedSaleID == -1)
                {
                    // إدراج مبيعة جديدة
                    int saleID = dbManager.InsertSale(customerID, productID, unitPrice, discount, totalAmount);
                    if (saleID > 0)
                    {
                        MessageBox.Show("تم حفظ البيانات بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearInputFields();
                        LoadAllSales();
                        UpdateTotalAmount();
                    }
                }
                else
                {
                    // تحديث مبيعة موجودة
                    if (dbManager.UpdateSale(selectedSaleID, customerID, productID, unitPrice, discount, totalAmount))
                    {
                        MessageBox.Show("تم تحديث البيانات بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearInputFields();
                        LoadAllSales();
                        UpdateTotalAmount();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في حفظ البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetOrCreateCustomer(string customerName, string phone)
        {
            // البحث عن العميل
            DataTable customers = dbManager.GetCustomers();
            foreach (DataRow row in customers.Rows)
            {
                if (row["CustomerName"].ToString().Trim() == customerName && 
                    row["Phone"].ToString().Trim() == phone)
                {
                    return Convert.ToInt32(row["CustomerID"]);
                }
            }

            // إنشاء عميل جديد
            return dbManager.InsertCustomer(customerName, phone);
        }

        private int GetOrCreateProduct(string productName, decimal price)
        {
            // البحث عن المنتج
            DataTable products = dbManager.GetProducts();
            foreach (DataRow row in products.Rows)
            {
                if (row["ProductName"].ToString().Trim() == productName)
                {
                    return Convert.ToInt32(row["ProductID"]);
                }
            }

            // إنشاء منتج جديد
            return dbManager.InsertProduct(productName, price);
        }

        private void edit_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("يرجى اختيار عنصر للتعديل", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ListViewItem selectedItem = listView1.SelectedItems[0];
            selectedSaleID = Convert.ToInt32(selectedItem.Text);

            // ملء الحقول بالبيانات المختارة
            name.Text = selectedItem.SubItems[1].Text;
            phone.Text = selectedItem.SubItems[2].Text;
            materialsave.Text = selectedItem.SubItems[3].Text;
            price.Text = selectedItem.SubItems[4].Text;
            dicount.Text = selectedItem.SubItems[5].Text;

            MessageBox.Show("تم تحميل البيانات للتعديل. قم بالتعديل واضغط حفظ", "معلومات", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadAllSales();
            UpdateTotalAmount();
            MessageBox.Show("تم تحديث جميع البيانات", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void print_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات للطباعة", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // هنا يمكن إضافة كود الطباعة
            MessageBox.Show("ميزة الطباعة ستكون متاحة قريباً", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pay_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show("يرجى اختيار عنصر لتسديد الدفع", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ListViewItem selectedItem = listView1.SelectedItems[0];
                int saleID = Convert.ToInt32(selectedItem.Text);
                decimal remainingAmount = Convert.ToDecimal(selectedItem.SubItems[8].Text);

                if (remainingAmount <= 0)
                {
                    MessageBox.Show("هذا العنصر مدفوع بالكامل", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // إنشاء نافذة إدخال مخصصة للمدفوعات
                PaymentForm paymentForm = new PaymentForm(saleID, remainingAmount, selectedItem.SubItems[1].Text);
                if (paymentForm.ShowDialog() == DialogResult.OK)
                {
                    decimal paymentAmount = paymentForm.PaymentAmount;
                    string paymentMethod = paymentForm.PaymentMethod;
                    string notes = paymentForm.Notes;

                    if (dbManager.InsertPayment(saleID, paymentAmount, paymentMethod, notes))
                    {
                        string message = $"تم تسجيل الدفعة بنجاح!\n\n";
                        message += $"مبلغ الدفعة: {paymentAmount:F2} ريال\n";
                        message += $"طريقة الدفع: {paymentMethod}\n";
                        message += $"المبلغ المتبقي: {(remainingAmount - paymentAmount):F2} ريال";
                        
                        MessageBox.Show(message, "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllSales();
                        UpdateTotalAmount();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تسديد الدفع: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void paydit_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("يرجى اختيار عنصر لعرض تفاصيل الدفع", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ListViewItem selectedItem = listView1.SelectedItems[0];
            int saleID = Convert.ToInt32(selectedItem.Text);
            
            // الحصول على تفاصيل المدفوعات من قاعدة البيانات
            DataTable paymentDetails = dbManager.GetPaymentDetails(saleID);
            
            // عرض تفاصيل الدفع الأساسية
            string details = $"تفاصيل الدفع للعنصر رقم: {saleID}\n";
            details += "==========================================\n\n";
            details += $"اسم العميل: {selectedItem.SubItems[1].Text}\n";
            details += $"اسم الصنف: {selectedItem.SubItems[3].Text}\n";
            details += $"السعر الأصلي: {selectedItem.SubItems[4].Text} ريال\n";
            details += $"الخصم: {selectedItem.SubItems[5].Text} ريال\n";
            details += $"الإجمالي: {selectedItem.SubItems[6].Text} ريال\n";
            details += $"المدفوع: {selectedItem.SubItems[7].Text} ريال\n";
            details += $"المتبقي: {selectedItem.SubItems[8].Text} ريال\n";
            details += $"تاريخ البيع: {selectedItem.SubItems[9].Text}\n\n";
            
            // عرض تفاصيل المدفوعات
            if (paymentDetails.Rows.Count > 0)
            {
                details += "تفاصيل المدفوعات:\n";
                details += "==================\n";
                
                decimal totalPaid = 0;
                for (int i = 0; i < paymentDetails.Rows.Count; i++)
                {
                    DataRow row = paymentDetails.Rows[i];
                    decimal paymentAmount = Convert.ToDecimal(row["PaymentAmount"]);
                    DateTime paymentDate = Convert.ToDateTime(row["PaymentDate"]);
                    string paymentMethod = row["PaymentMethod"].ToString();
                    string notes = row["Notes"].ToString();
                    
                    details += $"{i + 1}. مبلغ الدفعة: {paymentAmount:F2} ريال\n";
                    details += $"   تاريخ الدفع: {paymentDate:yyyy-MM-dd}\n";
                    details += $"   طريقة الدفع: {paymentMethod}\n";
                    if (!string.IsNullOrEmpty(notes))
                    {
                        details += $"   ملاحظات: {notes}\n";
                    }
                    details += "\n";
                    
                    totalPaid += paymentAmount;
                }
                
                details += $"إجمالي المدفوع: {totalPaid:F2} ريال\n";
            }
            else
            {
                details += "لم يتم تسجيل أي مدفوعات بعد.\n";
            }

            MessageBox.Show(details, "تفاصيل الدفع", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void sesearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(sesearch.Text))
            {
                LoadAllSales();
            }
            else
            {
                DataTable searchResults = dbManager.SearchSales(sesearch.Text);
                PopulateListView(searchResults);
            }
        }

        private void intent_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ميزة الربط بالإنترنت ستكون متاحة قريباً", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void senddata_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ميزة إرسال البيانات ستكون متاحة قريباً", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void resevdata_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ميزة استرداد البيانات ستكون متاحة قريباً", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // يمكن إضافة منطق إضافي عند اختيار عنصر
        }

        private void system_Load(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
