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
    public partial class PaymentForm : Form
    {
        public decimal PaymentAmount { get; private set; }
        public string PaymentMethod { get; private set; }
        public string Notes { get; private set; }

        private int saleID;
        private decimal remainingAmount;
        private string customerName;

        public PaymentForm(int saleID, decimal remainingAmount, string customerName)
        {
            InitializeComponent();
            this.saleID = saleID;
            this.remainingAmount = remainingAmount;
            this.customerName = customerName;
            InitializeForm();
        }

        private void InitializeForm()
        {
            // إعداد النموذج
            this.Text = $"تسديد الدفع - {customerName}";
            
            // إعداد القيم الافتراضية
            txtPaymentAmount.Text = remainingAmount.ToString("F2");
            cmbPaymentMethod.SelectedIndex = 0;
            
            // إعداد الحد الأقصى للمبلغ
            txtPaymentAmount.MaxLength = 10;
            
            // تحديث عرض المبلغ المتبقي
            label1.Text = $"المبلغ المتبقي: {remainingAmount:F2} ريال";
            lblRemainingAfter.Text = $"المبلغ المتبقي بعد الدفع: {remainingAmount:F2} ريال";
            
            // التركيز على حقل المبلغ
            txtPaymentAmount.Focus();
            txtPaymentAmount.SelectAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                PaymentAmount = Convert.ToDecimal(txtPaymentAmount.Text);
                PaymentMethod = cmbPaymentMethod.SelectedItem.ToString();
                Notes = txtNotes.Text.Trim();
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateInput()
        {
            // التحقق من مبلغ الدفع
            if (string.IsNullOrWhiteSpace(txtPaymentAmount.Text))
            {
                MessageBox.Show("يرجى إدخال مبلغ الدفع", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPaymentAmount.Focus();
                return false;
            }



            

            // التحقق من طريقة الدفع
            if (cmbPaymentMethod.SelectedIndex == -1)
            {
                MessageBox.Show("يرجى اختيار طريقة الدفع", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPaymentMethod.Focus();
                return false;
            }

            return true;
        }

        private void txtPaymentAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح بالأرقام والنقطة فقط
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // السماح بنقطة واحدة فقط
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }

            // السماح بالدخول عند الضغط على Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnOK_Click(sender, e);
            }
        }

        private void txtPaymentAmount_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void btnFullPayment_Click(object sender, EventArgs e)
        {
            txtPaymentAmount.Text = remainingAmount.ToString("F2");
            txtPaymentAmount.Focus();
        }

        private void btnHalfPayment_Click(object sender, EventArgs e)
        {
            decimal halfAmount = remainingAmount / 2;
            txtPaymentAmount.Text = halfAmount.ToString("F2");
            txtPaymentAmount.Focus();
        }
    }
}
