namespace Debt_system
{
    partial class PaymentAmount_Test
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTotalDebts = new System.Windows.Forms.Label();
            this.lblAverageAmount = new System.Windows.Forms.Label();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblTotalPayments = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnTestPaymentDetails = new System.Windows.Forms.Button();
            this.btnTestRemaining = new System.Windows.Forms.Button();
            this.btnTestPayment = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(760, 300);
            this.dataGridView1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTotalDebts);
            this.groupBox1.Controls.Add(this.lblAverageAmount);
            this.groupBox1.Controls.Add(this.lblTotalAmount);
            this.groupBox1.Controls.Add(this.lblTotalPayments);
            this.groupBox1.Location = new System.Drawing.Point(12, 318);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 120);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "إحصائيات المدفوعات";
            // 
            // lblTotalDebts
            // 
            this.lblTotalDebts.AutoSize = true;
            this.lblTotalDebts.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalDebts.ForeColor = System.Drawing.Color.Red;
            this.lblTotalDebts.Location = new System.Drawing.Point(15, 90);
            this.lblTotalDebts.Name = "lblTotalDebts";
            this.lblTotalDebts.Size = new System.Drawing.Size(120, 14);
            this.lblTotalDebts.TabIndex = 3;
            this.lblTotalDebts.Text = "إجمالي الديون: 0.00 ريال";
            // 
            // lblAverageAmount
            // 
            this.lblAverageAmount.AutoSize = true;
            this.lblAverageAmount.Location = new System.Drawing.Point(15, 70);
            this.lblAverageAmount.Name = "lblAverageAmount";
            this.lblAverageAmount.Size = new System.Drawing.Size(120, 13);
            this.lblAverageAmount.TabIndex = 2;
            this.lblAverageAmount.Text = "متوسط المبلغ: 0.00 ريال";
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Location = new System.Drawing.Point(15, 50);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(120, 13);
            this.lblTotalAmount.TabIndex = 1;
            this.lblTotalAmount.Text = "إجمالي المبلغ: 0.00 ريال";
            // 
            // lblTotalPayments
            // 
            this.lblTotalPayments.AutoSize = true;
            this.lblTotalPayments.Location = new System.Drawing.Point(15, 30);
            this.lblTotalPayments.Name = "lblTotalPayments";
            this.lblTotalPayments.Size = new System.Drawing.Size(100, 13);
            this.lblTotalPayments.TabIndex = 0;
            this.lblTotalPayments.Text = "إجمالي المدفوعات: 0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnRefresh);
            this.groupBox2.Controls.Add(this.btnTestPaymentDetails);
            this.groupBox2.Controls.Add(this.btnTestRemaining);
            this.groupBox2.Controls.Add(this.btnTestPayment);
            this.groupBox2.Location = new System.Drawing.Point(418, 318);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(354, 120);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "اختبارات PaymentAmount";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(270, 80);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "إغلاق";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(270, 20);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnTestPaymentDetails
            // 
            this.btnTestPaymentDetails.Location = new System.Drawing.Point(15, 80);
            this.btnTestPaymentDetails.Name = "btnTestPaymentDetails";
            this.btnTestPaymentDetails.Size = new System.Drawing.Size(120, 30);
            this.btnTestPaymentDetails.TabIndex = 2;
            this.btnTestPaymentDetails.Text = "اختبار تفاصيل المدفوعات";
            this.btnTestPaymentDetails.UseVisualStyleBackColor = true;
            this.btnTestPaymentDetails.Click += new System.EventHandler(this.btnTestPaymentDetails_Click);
            // 
            // btnTestRemaining
            // 
            this.btnTestRemaining.Location = new System.Drawing.Point(15, 50);
            this.btnTestRemaining.Name = "btnTestRemaining";
            this.btnTestRemaining.Size = new System.Drawing.Size(120, 30);
            this.btnTestRemaining.TabIndex = 1;
            this.btnTestRemaining.Text = "اختبار المبلغ المتبقي";
            this.btnTestRemaining.UseVisualStyleBackColor = true;
            this.btnTestRemaining.Click += new System.EventHandler(this.btnTestRemaining_Click);
            // 
            // btnTestPayment
            // 
            this.btnTestPayment.Location = new System.Drawing.Point(15, 20);
            this.btnTestPayment.Name = "btnTestPayment";
            this.btnTestPayment.Size = new System.Drawing.Size(120, 30);
            this.btnTestPayment.TabIndex = 0;
            this.btnTestPayment.Text = "اختبار إدراج دفعة";
            this.btnTestPayment.UseVisualStyleBackColor = true;
            this.btnTestPayment.Click += new System.EventHandler(this.btnTestPayment_Click);
            // 
            // PaymentAmount_Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "PaymentAmount_Test";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "اختبار PaymentAmount";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTotalPayments;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label lblAverageAmount;
        private System.Windows.Forms.Label lblTotalDebts;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnTestPayment;
        private System.Windows.Forms.Button btnTestRemaining;
        private System.Windows.Forms.Button btnTestPaymentDetails;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
    }
}
