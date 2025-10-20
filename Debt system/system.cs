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
using System.Drawing.Printing;
using System.IO;

namespace Debt_system
{
    public partial class system : Form
    {
        private DatabaseManager dbManager;
        private DataTable currentSalesData;
        private int selectedSaleID = -1;
		// الطباعة
		private PrintDocument printDocument;
		private List<string> printBuffer = new List<string>();
		private int printLineIndex = 0;
		private PrintPreviewDialog printPreviewDialog;

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
            
			// ربط حدث تغيير التاريخ لتصفية البيانات اليومية
			dateTime.ValueChanged += dateTime_ValueChanged;
			
            // تحميل البيانات
            LoadAllSales();
            
            // تحديث الإجمالي
            UpdateTotalAmount();
            
            // إعداد التاريخ الحالي
            dateTime.Value = DateTime.Now;
			
			// تهيئة مستند الطباعة ومعاينة الطباعة
			printDocument = new PrintDocument();
			printDocument.DocumentName = "تقReport-المبيعات";
			printDocument.PrintPage += printDocument_PrintPage;
			printPreviewDialog = new PrintPreviewDialog();
			printPreviewDialog.Document = printDocument;
        }

		private void dateTime_ValueChanged(object sender, EventArgs e)
		{
			LoadSalesByDate(dateTime.Value.Date);
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

		private void LoadSalesByDate(DateTime selectedDate)
		{
			currentSalesData = dbManager.GetSalesByDate(selectedDate);
			PopulateListView(currentSalesData);
			UpdateTotalAmount(currentSalesData);
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

		private void UpdateTotalAmount(DataTable data)
		{
			try
			{
				decimal sum = 0;
				foreach (DataRow row in data.Rows)
				{
					if (row["RemainingAmount"] != DBNull.Value)
					{
						sum += Convert.ToDecimal(row["RemainingAmount"]);
					}
				}
				totoalprice.Text = sum.ToString("F2");
			}
			catch
			{
				// في حال أي خطأ غير متوقع، نعود للسلوك الافتراضي
				UpdateTotalAmount();
			}
		}

        private void ClearInputFields()
        {
            name.Clear();
            phone.Clear();
            materialsave.Clear();
            price.Clear();
            dicount.Clear();

            selectedSaleID = -1;
			// إعادة زر الحفظ إلى وضع الإضافة
			Save.Text = "أظافة";
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

			// تغيير نص زر الحفظ إلى حفظ التعديل
			Save.Text = "حفظ التعديل";

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

			// تحضير البيانات للطباعة: المحدد فقط أو جميع الظاهرة
			PreparePrintData();

			try
			{
				printLineIndex = 0;
				// عرض المعاينة قبل الطباعة
				printPreviewDialog.Width = 1000;
				printPreviewDialog.Height = 700;
				printPreviewDialog.ShowIcon = true;
				printPreviewDialog.PrintPreviewControl.Zoom = 1.0;
				printPreviewDialog.ShowDialog();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"خطأ أثناء المعاينة/الطباعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
        }

		private void PreparePrintData()
		{
			printBuffer.Clear();
			// عنوان التقرير
			string title = "تقرير المبيعات";
			string subTitle = string.Empty;
			if (listView1.SelectedItems.Count > 0)
			{
				subTitle = "- عنصر محدد";
			}
			else
			{
				subTitle = "- جميع العناصر الظاهرة";
			}
			printBuffer.Add($"{title} {subTitle}");
			printBuffer.Add($"التاريخ: {DateTime.Now:yyyy-MM-dd HH:mm}");
			printBuffer.Add(new string('-', 60));
			printBuffer.Add("ID | الاسم | الهاتف | الصنف | السعر | الخصم | الإجمالي | المدفوع | المتبقي | التاريخ");
			printBuffer.Add(new string('-', 60));

			if (listView1.SelectedItems.Count > 0)
			{
				AppendItemToPrint(listView1.SelectedItems[0]);
			}
			else
			{
				foreach (ListViewItem item in listView1.Items)
				{
					AppendItemToPrint(item);
				}
			}
		}

		private void AppendItemToPrint(ListViewItem item)
		{
			try
			{
				// الأعمدة مرتبة كما تمت إضافتها في PopulateListView
				string line = string.Join(" | ", new string[]
				{
					item.SubItems[0].Text, // ID
					item.SubItems[1].Text, // الاسم
					item.SubItems[2].Text, // الهاتف
					item.SubItems[3].Text, // الصنف
					item.SubItems[4].Text, // السعر
					item.SubItems[5].Text, // الخصم
					item.SubItems[6].Text, // الإجمالي
					item.SubItems[7].Text, // المدفوع
					item.SubItems[8].Text, // المتبقي
					item.SubItems[9].Text  // التاريخ
				});
				printBuffer.Add(line);
			}
			catch
			{
				// تجاهل السطر إن حدث خطأ في القراءة
			}
		}

		private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
		{
			Font headerFont = new Font("Tahoma", 10, FontStyle.Bold);
			Font textFont = new Font("Tahoma", 9, FontStyle.Regular);
			int lineHeight = (int)textFont.GetHeight(e.Graphics) + 6;
			int x = e.MarginBounds.Left;
			int y = e.MarginBounds.Top;
			int right = e.MarginBounds.Right;

			// طباعة الأسطر مع ترقيم الصفحات
			for (int i = printLineIndex; i < printBuffer.Count; i++)
			{
				string line = printBuffer[i];
				Font fontToUse = (i <= 2) ? headerFont : textFont; // أول أسطر عناوين
				SizeF size = e.Graphics.MeasureString(line, fontToUse, right - x);
				if (y + size.Height > e.MarginBounds.Bottom)
				{
					// صفحة جديدة
					e.HasMorePages = true;
					printLineIndex = i;
					return;
				}
				e.Graphics.DrawString(line, fontToUse, Brushes.Black, new RectangleF(x, y, right - x, size.Height));
				y += lineHeight;
			}

			// انتهت البيانات
			e.HasMorePages = false;
			printLineIndex = 0;
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
			try
			{
				string dataDir = (AppDomain.CurrentDomain.GetData("DataDirectory") as string) ?? AppDomain.CurrentDomain.BaseDirectory;
				string mdfPath = Path.Combine(dataDir, "Database.mdf");
				string ldfPath = Path.Combine(dataDir, "Database_log.ldf");

				if (!File.Exists(mdfPath))
				{
					MessageBox.Show("لم يتم العثور على ملف قاعدة البيانات (Database.mdf)", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				// تحديد مجلد المشروع (مستويان أعلى من مجلد التشغيل: bin/Debug أو bin/Release)
				string baseDir = AppDomain.CurrentDomain.BaseDirectory;
				string projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"..", @".."));
				string backupDir = Path.Combine(projectRoot, "Backup");
				Directory.CreateDirectory(backupDir);

				string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
				string destMdf = Path.Combine(backupDir, $"Database_{timestamp}.mdf");
				string destLdf = Path.Combine(backupDir, $"Database_log_{timestamp}.ldf");

				File.Copy(mdfPath, destMdf, true);
				if (File.Exists(ldfPath))
				{
					File.Copy(ldfPath, destLdf, true);
				}

				MessageBox.Show($"تم حفظ النسخة الاحتياطية في مجلد المشروع:\n{destMdf}", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"خطأ أثناء التصدير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
        }

        private void resevdata_Click(object sender, EventArgs e)
        {
			try
			{
				using (var openDlg = new OpenFileDialog())
				{
					openDlg.Title = "اختر ملف قاعدة البيانات (MDF) للاسترداد";
					openDlg.Filter = "Database MDF (*.mdf)|*.mdf";
					var result = openDlg.ShowDialog();
					if (result != DialogResult.OK)
						return;

					string backupMdf = openDlg.FileName;
					// محاولة إيجاد ملف السجل بجوار الملف المحدد
					string backupLdf = Path.Combine(Path.GetDirectoryName(backupMdf), Path.GetFileNameWithoutExtension(backupMdf) + "_log.ldf");

					// عرض معاينة البيانات (عدد السجلات في الجداول الرئيسية)
					string preview = GetDatabasePreview(backupMdf);
					DialogResult confirm = MessageBox.Show($"سيتم استرداد قاعدة البيانات من الملف التالي:\n{backupMdf}\n\nمعاينة:\n{preview}\n\nهل تريد المتابعة؟", "تأكيد الاسترداد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RightAlign);
					if (confirm != DialogResult.Yes)
						return;

					// نسخ الملفات إلى مجلد بيانات التطبيق
					string dataDir = (AppDomain.CurrentDomain.GetData("DataDirectory") as string) ?? AppDomain.CurrentDomain.BaseDirectory;
					string targetMdf = Path.Combine(dataDir, "Database.mdf");
					string targetLdf = Path.Combine(dataDir, "Database_log.ldf");

					// محاولة إغلاق أي اتصالات عبر GC/Pooling (إجرائي)
					GC.Collect();
					GC.WaitForPendingFinalizers();

					File.Copy(backupMdf, targetMdf, true);
					if (File.Exists(backupLdf))
					{
						File.Copy(backupLdf, targetLdf, true);
					}

					MessageBox.Show("تم استرداد قاعدة البيانات بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
					// إعادة تحميل البيانات في الواجهة
					LoadAllSales();
					UpdateTotalAmount();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"خطأ أثناء الاسترداد: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
        }

		private string GetDatabasePreview(string mdfPath)
		{
			try
			{
				string tempConnString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={mdfPath};Integrated Security=True;Connect Timeout=30";
				using (var conn = new System.Data.SqlClient.SqlConnection(tempConnString))
				{
					conn.Open();
					int customers = ExecuteScalarCount(conn, "SELECT COUNT(*) FROM Customers");
					int products = ExecuteScalarCount(conn, "SELECT COUNT(*) FROM Products");
					int sales = ExecuteScalarCount(conn, "SELECT COUNT(*) FROM Sales");
					int payments = ExecuteScalarCount(conn, "SELECT COUNT(*) FROM Payments");
					return $"العملاء: {customers}\nالمنتجات: {products}\nالمبيعات: {sales}\nالمدفوعات: {payments}";
				}
			}
			catch (Exception ex)
			{
				return $"تعذر قراءة المعاينة: {ex.Message}";
			}
		}

		private int ExecuteScalarCount(System.Data.SqlClient.SqlConnection conn, string query)
		{
			using (var cmd = new System.Data.SqlClient.SqlCommand(query, conn))
			{
				object val = cmd.ExecuteScalar();
				return (val == null || val == DBNull.Value) ? 0 : Convert.ToInt32(val);
			}
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

        private void dateTime_ValueChanged_1(object sender, EventArgs e)
        {

        }
    }
}
