namespace Debt_system
{
    partial class system
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("ID");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("الاسم");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("الهاتف");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("اسم الصنف");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("السعر");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("الخصم");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("التاريخ");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("المدفوع");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("الاجمالي");
            this.paydit = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.print = new System.Windows.Forms.Button();
            this.pay = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.totoalprice = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.sesearch = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dateTime = new System.Windows.Forms.DateTimePicker();
            this.resevdata = new System.Windows.Forms.Button();
            this.senddata = new System.Windows.Forms.Button();
            this.intent = new System.Windows.Forms.Button();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.edit = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.detailsdata = new System.Windows.Forms.GroupBox();
            this.dicount = new System.Windows.Forms.TextBox();
            this.price = new System.Windows.Forms.TextBox();
            this.materialsave = new System.Windows.Forms.TextBox();
            this.phone = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4.SuspendLayout();
            this.detailsdata.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // paydit
            // 
            this.paydit.Location = new System.Drawing.Point(324, 19);
            this.paydit.Name = "paydit";
            this.paydit.Size = new System.Drawing.Size(101, 52);
            this.paydit.TabIndex = 5;
            this.paydit.Text = "تفاصيل الدفع";
            this.paydit.UseVisualStyleBackColor = true;
            this.paydit.Click += new System.EventHandler(this.paydit_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(6, 19);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(118, 52);
            this.button5.TabIndex = 4;
            this.button5.Text = "كل البيانات";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(184, 19);
            this.print.Name = "print";
            this.print.Size = new System.Drawing.Size(110, 52);
            this.print.TabIndex = 3;
            this.print.Text = "طباعة";
            this.print.UseVisualStyleBackColor = true;
            this.print.Click += new System.EventHandler(this.print_Click);
            // 
            // pay
            // 
            this.pay.Location = new System.Drawing.Point(471, 19);
            this.pay.Name = "pay";
            this.pay.Size = new System.Drawing.Size(96, 52);
            this.pay.TabIndex = 2;
            this.pay.Text = "تسديد";
            this.pay.UseVisualStyleBackColor = true;
            this.pay.Click += new System.EventHandler(this.pay_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.totoalprice);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.sesearch);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.dateTime);
            this.groupBox4.Location = new System.Drawing.Point(11, 316);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(877, 53);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            // 
            // totoalprice
            // 
            this.totoalprice.Location = new System.Drawing.Point(50, 13);
            this.totoalprice.Multiline = true;
            this.totoalprice.Name = "totoalprice";
            this.totoalprice.Size = new System.Drawing.Size(108, 24);
            this.totoalprice.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(192, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "الاجمالي";
            // 
            // sesearch
            // 
            this.sesearch.Location = new System.Drawing.Point(332, 13);
            this.sesearch.Multiline = true;
            this.sesearch.Name = "sesearch";
            this.sesearch.Size = new System.Drawing.Size(180, 24);
            this.sesearch.TabIndex = 12;
            this.sesearch.TextChanged += new System.EventHandler(this.sesearch_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(546, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "البحث";
            // 
            // dateTime
            // 
            this.dateTime.Location = new System.Drawing.Point(655, 17);
            this.dateTime.Name = "dateTime";
            this.dateTime.Size = new System.Drawing.Size(200, 20);
            this.dateTime.TabIndex = 0;
            this.dateTime.ValueChanged += new System.EventHandler(this.dateTime_ValueChanged_1);
            // 
            // resevdata
            // 
            this.resevdata.Location = new System.Drawing.Point(759, 88);
            this.resevdata.Name = "resevdata";
            this.resevdata.Size = new System.Drawing.Size(115, 59);
            this.resevdata.TabIndex = 18;
            this.resevdata.Text = "استرداد البيانات";
            this.resevdata.UseVisualStyleBackColor = true;
            this.resevdata.Click += new System.EventHandler(this.resevdata_Click);
            // 
            // senddata
            // 
            this.senddata.Location = new System.Drawing.Point(618, 84);
            this.senddata.Name = "senddata";
            this.senddata.Size = new System.Drawing.Size(115, 59);
            this.senddata.TabIndex = 17;
            this.senddata.Text = "ارسال البيانات";
            this.senddata.UseVisualStyleBackColor = true;
            this.senddata.Click += new System.EventHandler(this.senddata_Click);
            // 
            // intent
            // 
            this.intent.Location = new System.Drawing.Point(465, 84);
            this.intent.Name = "intent";
            this.intent.Size = new System.Drawing.Size(115, 59);
            this.intent.TabIndex = 13;
            this.intent.Text = "ربط على الانترنت";
            this.intent.UseVisualStyleBackColor = true;
            this.intent.Click += new System.EventHandler(this.intent_Click);
            // 
            // edit
            // 
            this.edit.Location = new System.Drawing.Point(598, 19);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(108, 52);
            this.edit.TabIndex = 1;
            this.edit.Text = "تعديل";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.edit_Click);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(744, 19);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(103, 52);
            this.Save.TabIndex = 0;
            this.Save.Text = "أظافة";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9});
            this.listView1.Location = new System.Drawing.Point(11, 375);
            this.listView1.Name = "listView1";
            this.listView1.RightToLeftLayout = true;
            this.listView1.Size = new System.Drawing.Size(877, 97);
            this.listView1.TabIndex = 20;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // detailsdata
            // 
            this.detailsdata.Controls.Add(this.paydit);
            this.detailsdata.Controls.Add(this.button5);
            this.detailsdata.Controls.Add(this.print);
            this.detailsdata.Controls.Add(this.pay);
            this.detailsdata.Controls.Add(this.edit);
            this.detailsdata.Controls.Add(this.Save);
            this.detailsdata.Location = new System.Drawing.Point(11, 232);
            this.detailsdata.Name = "detailsdata";
            this.detailsdata.Size = new System.Drawing.Size(869, 77);
            this.detailsdata.TabIndex = 15;
            this.detailsdata.TabStop = false;
            // 
            // dicount
            // 
            this.dicount.Location = new System.Drawing.Point(93, 167);
            this.dicount.Multiline = true;
            this.dicount.Name = "dicount";
            this.dicount.Size = new System.Drawing.Size(224, 21);
            this.dicount.TabIndex = 9;
            // 
            // price
            // 
            this.price.Location = new System.Drawing.Point(93, 130);
            this.price.Multiline = true;
            this.price.Name = "price";
            this.price.Size = new System.Drawing.Size(224, 21);
            this.price.TabIndex = 8;
            // 
            // materialsave
            // 
            this.materialsave.Location = new System.Drawing.Point(93, 96);
            this.materialsave.Multiline = true;
            this.materialsave.Name = "materialsave";
            this.materialsave.Size = new System.Drawing.Size(224, 21);
            this.materialsave.TabIndex = 7;
            // 
            // phone
            // 
            this.phone.Location = new System.Drawing.Point(93, 61);
            this.phone.Multiline = true;
            this.phone.Name = "phone";
            this.phone.Size = new System.Drawing.Size(224, 21);
            this.phone.TabIndex = 6;
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(93, 19);
            this.name.Multiline = true;
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(224, 21);
            this.name.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(359, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "الخصم";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(360, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "السعر";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(360, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "اسم الصنف";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(357, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "الهاتف";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(363, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "الاسم";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dicount);
            this.groupBox1.Controls.Add(this.price);
            this.groupBox1.Controls.Add(this.materialsave);
            this.groupBox1.Controls.Add(this.phone);
            this.groupBox1.Controls.Add(this.name);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(11, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(448, 213);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "أظافة البيانات";
            // 
            // system
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(892, 504);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.resevdata);
            this.Controls.Add(this.senddata);
            this.Controls.Add(this.intent);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.detailsdata);
            this.Controls.Add(this.groupBox1);
            this.Name = "system";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "system";
            this.Load += new System.EventHandler(this.system_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.detailsdata.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button paydit;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button print;
        private System.Windows.Forms.Button pay;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox totoalprice;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox sesearch;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dateTime;
        private System.Windows.Forms.Button resevdata;
        private System.Windows.Forms.Button senddata;
        private System.Windows.Forms.Button intent;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox detailsdata;
        private System.Windows.Forms.TextBox dicount;
        private System.Windows.Forms.TextBox price;
        private System.Windows.Forms.TextBox materialsave;
        private System.Windows.Forms.TextBox phone;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}