using CBSP.Customs.ImportDeclare;
using CBSP.Customs.ImportOrder;
using CBSP.Customs.ImportOthers;
using CBSP.Customs.ImportPayment;
using CBSP.Customs.ImportWayBill;
using CBSP.Import.ImportDelivery;
using CBSP.Import.ImportOthers;
using System;

namespace CBSP.Import
{
    public partial class frmMain : Library.ModuleProvider.frmModuleBase
    {
        public frmMain()
        {
            InitializeComponent();
            this.AddFunction(btn_CuOrder, typeof(frmCuOrderHead), "进口订单");
            this.AddFunction(btn_CuWaybill, typeof(frmCuWayBillHead), "进口运单");
            this.AddFunction(btn_Declare, typeof(frmCuDeclareHead), "进口清单");
            this.AddFunction(btn_CuPayment, typeof(frmCuPaymentHead), "进口支付单");
            this.AddFunction(btn_Devlivery, typeof(frmImDeliveryHead), "进口入库单");
            this.AddFunction(btn_CuOthers, typeof(frmCuStatus), "进口删单");
            this.AddFunction(btn_CuSummary, typeof(frmCuSummary), "商品汇总");
            this.AddFunction(btn_Repeat, typeof(frmCuRepeat), "面单补打");

        }


        private void btn_Bill_Click(object sender, EventArgs e)
        {
            this.ShowChildForm(typeof(frmCuPaymentHead));
        }

        private void btn_CuOrder_Click(object sender, EventArgs e)
        {
            this.ShowChildForm(typeof(frmCuOrderHead));
        }

        private void btn_CuWaybill_Click(object sender, EventArgs e)
        {
            this.ShowChildForm(typeof(frmCuWayBillHead));
        }

        private void btn_Declare_Click(object sender, EventArgs e)
        {
            this.ShowChildForm(typeof(frmCuDeclareHead));
        }

        private void btn_Devlivery_Click(object sender, EventArgs e)
        {
            this.ShowChildForm(typeof(frmImDeliveryHead));
        }

        private void btn_CuOthers_Click(object sender, EventArgs e)
        {
            this.ShowChildForm(typeof(frmCuStatus));
        }

        private void btn_CuSummary_Click(object sender, EventArgs e)
        {
            this.ShowChildForm(typeof(frmCuSummary));
        }

        private void btn_Repeat_Click(object sender, EventArgs e)
        {
            this.ShowChildForm(typeof(frmCuRepeat));
        }
    }
}
