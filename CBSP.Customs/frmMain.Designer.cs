namespace CBSP.Import
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_CuOrder = new DevExpress.XtraEditors.SimpleButton();
            this.btn_CuWaybill = new DevExpress.XtraEditors.SimpleButton();
            this.btn_CuPayment = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Devlivery = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Declare = new DevExpress.XtraEditors.SimpleButton();
            this.btn_CuOthers = new DevExpress.XtraEditors.SimpleButton();
            this.btn_CuSummary = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Repeat = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.flowLayoutPanel1);
            this.panelControl1.Size = new System.Drawing.Size(692, 388);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btn_CuOrder);
            this.flowLayoutPanel1.Controls.Add(this.btn_CuWaybill);
            this.flowLayoutPanel1.Controls.Add(this.btn_CuPayment);
            this.flowLayoutPanel1.Controls.Add(this.btn_Devlivery);
            this.flowLayoutPanel1.Controls.Add(this.btn_Declare);
            this.flowLayoutPanel1.Controls.Add(this.btn_CuOthers);
            this.flowLayoutPanel1.Controls.Add(this.btn_CuSummary);
            this.flowLayoutPanel1.Controls.Add(this.btn_Repeat);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(675, 372);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btn_CuOrder
            // 
            this.btn_CuOrder.Location = new System.Drawing.Point(3, 3);
            this.btn_CuOrder.Name = "btn_CuOrder";
            this.btn_CuOrder.Size = new System.Drawing.Size(75, 35);
            this.btn_CuOrder.TabIndex = 3;
            this.btn_CuOrder.Text = "进口订单";
            this.btn_CuOrder.Click += new System.EventHandler(this.btn_CuOrder_Click);
            // 
            // btn_CuWaybill
            // 
            this.btn_CuWaybill.Location = new System.Drawing.Point(84, 3);
            this.btn_CuWaybill.Name = "btn_CuWaybill";
            this.btn_CuWaybill.Size = new System.Drawing.Size(75, 35);
            this.btn_CuWaybill.TabIndex = 4;
            this.btn_CuWaybill.Text = "进口运单";
            this.btn_CuWaybill.Click += new System.EventHandler(this.btn_CuWaybill_Click);
            // 
            // btn_CuPayment
            // 
            this.btn_CuPayment.Location = new System.Drawing.Point(165, 3);
            this.btn_CuPayment.Name = "btn_CuPayment";
            this.btn_CuPayment.Size = new System.Drawing.Size(75, 35);
            this.btn_CuPayment.TabIndex = 1;
            this.btn_CuPayment.Text = "进口支付单";
            this.btn_CuPayment.Click += new System.EventHandler(this.btn_Bill_Click);
            // 
            // btn_Devlivery
            // 
            this.btn_Devlivery.Location = new System.Drawing.Point(246, 3);
            this.btn_Devlivery.Name = "btn_Devlivery";
            this.btn_Devlivery.Size = new System.Drawing.Size(75, 35);
            this.btn_Devlivery.TabIndex = 6;
            this.btn_Devlivery.Text = "进口入库单";
            this.btn_Devlivery.Click += new System.EventHandler(this.btn_Devlivery_Click);
            // 
            // btn_Declare
            // 
            this.btn_Declare.Location = new System.Drawing.Point(327, 3);
            this.btn_Declare.Name = "btn_Declare";
            this.btn_Declare.Size = new System.Drawing.Size(75, 35);
            this.btn_Declare.TabIndex = 5;
            this.btn_Declare.Text = "进口清单";
            this.btn_Declare.Click += new System.EventHandler(this.btn_Declare_Click);
            // 
            // btn_CuOthers
            // 
            this.btn_CuOthers.Location = new System.Drawing.Point(408, 3);
            this.btn_CuOthers.Name = "btn_CuOthers";
            this.btn_CuOthers.Size = new System.Drawing.Size(75, 35);
            this.btn_CuOthers.TabIndex = 7;
            this.btn_CuOthers.Text = "进口删单";
            this.btn_CuOthers.Click += new System.EventHandler(this.btn_CuOthers_Click);
            // 
            // btn_CuSummary
            // 
            this.btn_CuSummary.Location = new System.Drawing.Point(489, 3);
            this.btn_CuSummary.Name = "btn_CuSummary";
            this.btn_CuSummary.Size = new System.Drawing.Size(75, 35);
            this.btn_CuSummary.TabIndex = 8;
            this.btn_CuSummary.Text = "商品汇总";
            this.btn_CuSummary.Click += new System.EventHandler(this.btn_CuSummary_Click);
            // 
            // btn_Repeat
            // 
            this.btn_Repeat.Location = new System.Drawing.Point(570, 3);
            this.btn_Repeat.Name = "btn_Repeat";
            this.btn_Repeat.Size = new System.Drawing.Size(75, 35);
            this.btn_Repeat.TabIndex = 9;
            this.btn_Repeat.Text = "面单补打";
            this.btn_Repeat.Click += new System.EventHandler(this.btn_Repeat_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 388);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "frmMain";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btn_CuPayment;
        private DevExpress.XtraEditors.SimpleButton btn_CuOrder;
        private DevExpress.XtraEditors.SimpleButton btn_CuWaybill;
        private DevExpress.XtraEditors.SimpleButton btn_Declare;
        private DevExpress.XtraEditors.SimpleButton btn_Devlivery;
        private DevExpress.XtraEditors.SimpleButton btn_CuOthers;
        private DevExpress.XtraEditors.SimpleButton btn_CuSummary;
        private DevExpress.XtraEditors.SimpleButton btn_Repeat;
    }
}