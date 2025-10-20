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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loginsys_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            // التحقق من اسم المستخدم وكلمة المرور
            if (username == "useradmin" && password == "admin123")
            {
                // إخفاء نافذة تسجيل الدخول
                this.Hide();
                
                // فتح النظام الرئيسي
                system mainSystem = new system();
                mainSystem.ShowDialog();
                
                // إغلاق نافذة تسجيل الدخول عند إغلاق النظام الرئيسي
                this.Close();
            }
            else
            {
                MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة!", "خطأ في تسجيل الدخول", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // مسح الحقول
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // جعل حقل كلمة المرور محمي
            textBox2.UseSystemPasswordChar = true;
            
            // التركيز على حقل اسم المستخدم عند فتح النافذة
            textBox1.Focus();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح بالدخول عند الضغط على Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox2.Focus();
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح بتسجيل الدخول عند الضغط على Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                loginsys_Click(sender, e);
                e.Handled = true;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
