using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Debt_system
{
    /// <summary>
    /// فورم اختبار PaymentAmount
    /// </summary>
    public partial class PaymentAmount_Test : Form
    {
        private DatabaseManager dbManager;

        public PaymentAmount_Test()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            InitializeTest();
        }

        private void InitializeTest()
        {
            // اختبار الاتصال بقاعدة البيانات
            if (!dbManager.TestConnection())
            {
                MessageBox.Show("لا يمكن الاتصال بقاعدة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // تحميل البيانات
            LoadTestData();
        }

        private void LoadTestData()
        {
            try
            {
                // تحميل المبيعات
                DataTable sales = dbManager.GetAllSales();
                dataGridView1.DataSource = sales;

                // تحميل إحصائيات المدفوعات
                DataTable stats = dbManager.GetPaymentStatistics();
                if (stats.Rows.Count > 0)
                {
                    DataRow row = stats.Rows[0];
                    lblTotalPayments.Text = $"إجمالي المدفوعات: {row["TotalPayments"]}";
                    lblTotalAmount.Text = $"إجمالي المبلغ: {Convert.ToDecimal(row["TotalPaymentAmount"]):F2} ريال";
                    lblAverageAmount.Text = $"متوسط المبلغ: {Convert.ToDecimal(row["AveragePaymentAmount"]):F2} ريال";
                }

                // تحميل إجمالي الديون
                decimal totalDebts = dbManager.GetTotalDebts();
                lblTotalDebts.Text = $"إجمالي الديون: {totalDebts:F2} ريال";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحميل البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTestPayment_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("يرجى اختيار مبيعة للاختبار", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int saleID = Convert.ToInt32(selectedRow.Cells["SaleID"].Value);
                decimal remainingAmount = Convert.ToDecimal(selectedRow.Cells["RemainingAmount"].Value);

                if (remainingAmount <= 0)
                {
                    MessageBox.Show("هذه المبيعة مدفوعة بالكامل", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // اختبار إدراج دفعة
                decimal testAmount = Math.Min(remainingAmount, 50); // اختبار بمبلغ 50 ريال أو المبلغ المتبقي
                
                if (dbManager.InsertPayment(saleID, testAmount, "اختبار", "دفعة اختبار"))
                {
                    MessageBox.Show($"تم إدراج دفعة اختبار بنجاح!\nالمبلغ: {testAmount:F2} ريال", 
                        "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // تحديث البيانات
                    LoadTestData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في اختبار الدفع: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTestRemaining_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("يرجى اختيار مبيعة للاختبار", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int saleID = Convert.ToInt32(selectedRow.Cells["SaleID"].Value);
                
                decimal remainingAmount = dbManager.GetRemainingAmount(saleID);
                MessageBox.Show($"المبلغ المتبقي للمبيعة {saleID}: {remainingAmount:F2} ريال", 
                    "نتيجة الاختبار", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في اختبار المبلغ المتبقي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTestPaymentDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("يرجى اختيار مبيعة للاختبار", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int saleID = Convert.ToInt32(selectedRow.Cells["SaleID"].Value);
                
                DataTable paymentDetails = dbManager.GetPaymentDetails(saleID);
                
                string details = $"تفاصيل المدفوعات للمبيعة {saleID}:\n\n";
                if (paymentDetails.Rows.Count > 0)
                {
                    foreach (DataRow row in paymentDetails.Rows)
                    {
                        details += $"مبلغ الدفعة: {Convert.ToDecimal(row["PaymentAmount"]):F2} ريال\n";
                        details += $"تاريخ الدفع: {Convert.ToDateTime(row["PaymentDate"]):yyyy-MM-dd}\n";
                        details += $"طريقة الدفع: {row["PaymentMethod"]}\n";
                        details += $"ملاحظات: {row["Notes"]}\n";
                        details += "---\n";
                    }
                }
                else
                {
                    details += "لا توجد مدفوعات مسجلة";
                }

                MessageBox.Show(details, "تفاصيل المدفوعات", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في اختبار تفاصيل المدفوعات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTestData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
