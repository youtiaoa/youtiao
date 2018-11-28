namespace CBSP.Customs.ImportDelivery
{ 
    partial class frmImDeliveryList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImDeliveryList));
            this.LC_Edit = new DevExpress.XtraLayout.LayoutControl();
            this.txtGUID = new DevExpress.XtraEditors.TextEdit();
            this.txtG_NUM = new DevExpress.XtraEditors.TextEdit();
            this.txtLOGISTICS_NO = new DevExpress.XtraEditors.TextEdit();
            this.txtNOTE = new DevExpress.XtraEditors.TextEdit();
            this.LCGroup_Edit = new DevExpress.XtraLayout.LayoutControlGroup();
            this.LCItem_DECL_DECLAR_CHECK_ID = new DevExpress.XtraLayout.LayoutControlItem();
            this.LCItem_DECL_NO = new DevExpress.XtraLayout.LayoutControlItem();
            this.LCItem_CONTAINER_QTY = new DevExpress.XtraLayout.LayoutControlItem();
            this.LCItem_CONTAINER_MODEL_CODE = new DevExpress.XtraLayout.LayoutControlItem();
            this.btn_ok = new DevExpress.XtraEditors.SimpleButton();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.LC_Edit)).BeginInit();
            this.LC_Edit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGUID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtG_NUM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLOGISTICS_NO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNOTE.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LCGroup_Edit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LCItem_DECL_DECLAR_CHECK_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LCItem_DECL_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LCItem_CONTAINER_QTY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LCItem_CONTAINER_MODEL_CODE)).BeginInit();
            this.SuspendLayout();
            // 
            // LC_Edit
            // 
            this.LC_Edit.Controls.Add(this.txtGUID);
            this.LC_Edit.Controls.Add(this.txtG_NUM);
            this.LC_Edit.Controls.Add(this.txtLOGISTICS_NO);
            this.LC_Edit.Controls.Add(this.txtNOTE);
            this.LC_Edit.Location = new System.Drawing.Point(5, 5);
            this.LC_Edit.Name = "LC_Edit";
            this.LC_Edit.Root = this.LCGroup_Edit;
            this.LC_Edit.Size = new System.Drawing.Size(728, 78);
            this.LC_Edit.TabIndex = 1;
            this.LC_Edit.Text = "layoutControl1";
            // 
            // txtGUID
            // 
            this.txtGUID.Enabled = false;
            this.txtGUID.Location = new System.Drawing.Point(87, 12);
            this.txtGUID.Name = "txtGUID";
            this.txtGUID.Properties.MaxLength = 64;
            this.txtGUID.Properties.ValidateOnEnterKey = true;
            this.txtGUID.Size = new System.Drawing.Size(275, 20);
            this.txtGUID.StyleController = this.LC_Edit;
            this.txtGUID.TabIndex = 4;
            // 
            // txtG_NUM
            // 
            this.txtG_NUM.Enabled = false;
            this.txtG_NUM.Location = new System.Drawing.Point(441, 12);
            this.txtG_NUM.Name = "txtG_NUM";
            this.txtG_NUM.Properties.MaxLength = 100;
            this.txtG_NUM.Properties.ValidateOnEnterKey = true;
            this.txtG_NUM.Size = new System.Drawing.Size(275, 20);
            this.txtG_NUM.StyleController = this.LC_Edit;
            this.txtG_NUM.TabIndex = 5;
            // 
            // txtLOGISTICS_NO
            // 
            this.txtLOGISTICS_NO.Location = new System.Drawing.Point(87, 36);
            this.txtLOGISTICS_NO.Name = "txtLOGISTICS_NO";
            this.txtLOGISTICS_NO.Properties.MaxLength = 76;
            this.txtLOGISTICS_NO.Properties.NullValuePrompt = "必填";
            this.txtLOGISTICS_NO.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtLOGISTICS_NO.Properties.ValidateOnEnterKey = true;
            this.txtLOGISTICS_NO.Size = new System.Drawing.Size(275, 20);
            this.txtLOGISTICS_NO.StyleController = this.LC_Edit;
            this.txtLOGISTICS_NO.TabIndex = 7;
            // 
            // txtNOTE
            // 
            this.txtNOTE.Location = new System.Drawing.Point(441, 36);
            this.txtNOTE.Name = "txtNOTE";
            this.txtNOTE.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtNOTE.Properties.Appearance.Options.UseBackColor = true;
            this.txtNOTE.Properties.MaxLength = 8;
            this.txtNOTE.Properties.ValidateOnEnterKey = true;
            this.txtNOTE.Size = new System.Drawing.Size(275, 20);
            this.txtNOTE.StyleController = this.LC_Edit;
            this.txtNOTE.TabIndex = 6;
            // 
            // LCGroup_Edit
            // 
            this.LCGroup_Edit.AppearanceItemCaption.Options.UseTextOptions = true;
            this.LCGroup_Edit.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.LCGroup_Edit.CustomizationFormText = "LCGroup_Edit";
            this.LCGroup_Edit.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.LCGroup_Edit.GroupBordersVisible = false;
            this.LCGroup_Edit.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.LCItem_DECL_DECLAR_CHECK_ID,
            this.LCItem_DECL_NO,
            this.LCItem_CONTAINER_QTY,
            this.LCItem_CONTAINER_MODEL_CODE});
            this.LCGroup_Edit.Location = new System.Drawing.Point(0, 0);
            this.LCGroup_Edit.Name = "LCGroup_Edit";
            this.LCGroup_Edit.Size = new System.Drawing.Size(728, 78);
            this.LCGroup_Edit.TextVisible = false;
            // 
            // LCItem_DECL_DECLAR_CHECK_ID
            // 
            this.LCItem_DECL_DECLAR_CHECK_ID.Control = this.txtGUID;
            this.LCItem_DECL_DECLAR_CHECK_ID.CustomizationFormText = "DECL_DECLAR_CHECK_ID";
            this.LCItem_DECL_DECLAR_CHECK_ID.Location = new System.Drawing.Point(0, 0);
            this.LCItem_DECL_DECLAR_CHECK_ID.Name = "LCItem_DECL_DECLAR_CHECK_ID";
            this.LCItem_DECL_DECLAR_CHECK_ID.Size = new System.Drawing.Size(354, 24);
            this.LCItem_DECL_DECLAR_CHECK_ID.Text = "GUID";
            this.LCItem_DECL_DECLAR_CHECK_ID.TextSize = new System.Drawing.Size(72, 14);
            // 
            // LCItem_DECL_NO
            // 
            this.LCItem_DECL_NO.Control = this.txtG_NUM;
            this.LCItem_DECL_NO.CustomizationFormText = "清单编号";
            this.LCItem_DECL_NO.Location = new System.Drawing.Point(354, 0);
            this.LCItem_DECL_NO.Name = "LCItem_DECL_NO";
            this.LCItem_DECL_NO.Size = new System.Drawing.Size(354, 24);
            this.LCItem_DECL_NO.Text = "序号";
            this.LCItem_DECL_NO.TextSize = new System.Drawing.Size(72, 14);
            // 
            // LCItem_CONTAINER_QTY
            // 
            this.LCItem_CONTAINER_QTY.Control = this.txtLOGISTICS_NO;
            this.LCItem_CONTAINER_QTY.CustomizationFormText = "集装箱数量";
            this.LCItem_CONTAINER_QTY.Location = new System.Drawing.Point(0, 24);
            this.LCItem_CONTAINER_QTY.Name = "LCItem_CONTAINER_QTY";
            this.LCItem_CONTAINER_QTY.Size = new System.Drawing.Size(354, 34);
            this.LCItem_CONTAINER_QTY.Text = "物流运单编号";
            this.LCItem_CONTAINER_QTY.TextSize = new System.Drawing.Size(72, 14);
            // 
            // LCItem_CONTAINER_MODEL_CODE
            // 
            this.LCItem_CONTAINER_MODEL_CODE.Control = this.txtNOTE;
            this.LCItem_CONTAINER_MODEL_CODE.CustomizationFormText = "集装箱规格";
            this.LCItem_CONTAINER_MODEL_CODE.Location = new System.Drawing.Point(354, 24);
            this.LCItem_CONTAINER_MODEL_CODE.Name = "LCItem_CONTAINER_MODEL_CODE";
            this.LCItem_CONTAINER_MODEL_CODE.Size = new System.Drawing.Size(354, 34);
            this.LCItem_CONTAINER_MODEL_CODE.Text = "备注";
            this.LCItem_CONTAINER_MODEL_CODE.TextSize = new System.Drawing.Size(72, 14);
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(251, 105);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(101, 23);
            this.btn_ok.TabIndex = 2;
            this.btn_ok.Text = "提交";
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(402, 105);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(101, 23);
            this.btn_cancel.TabIndex = 3;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // frmImDeliveryList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 147);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.LC_Edit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmImDeliveryList";
            this.Text = "统一版进口入库单表体信息";
            ((System.ComponentModel.ISupportInitialize)(this.LC_Edit)).EndInit();
            this.LC_Edit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtGUID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtG_NUM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLOGISTICS_NO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNOTE.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LCGroup_Edit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LCItem_DECL_DECLAR_CHECK_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LCItem_DECL_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LCItem_CONTAINER_QTY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LCItem_CONTAINER_MODEL_CODE)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl LC_Edit;
        private DevExpress.XtraEditors.TextEdit txtGUID;
        private DevExpress.XtraEditors.TextEdit txtG_NUM;
        private DevExpress.XtraEditors.TextEdit txtLOGISTICS_NO;
        private DevExpress.XtraLayout.LayoutControlGroup LCGroup_Edit;
        private DevExpress.XtraLayout.LayoutControlItem LCItem_DECL_DECLAR_CHECK_ID;
        private DevExpress.XtraLayout.LayoutControlItem LCItem_DECL_NO;
        private DevExpress.XtraLayout.LayoutControlItem LCItem_CONTAINER_MODEL_CODE;
        private DevExpress.XtraLayout.LayoutControlItem LCItem_CONTAINER_QTY;
        private DevExpress.XtraEditors.SimpleButton btn_ok;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.TextEdit txtNOTE;
    }
}