using System;

namespace QuanLyNhaHang
{
    public partial class rBill : DevExpress.XtraReports.UI.XtraReport
    {
        public rBill(int id)
        {
            InitializeComponent();
            this.dataTableBillTableAdapter.Fill(this.dataSet.DataTableBill, id);
            xrTotalPrice.Text = this.queriesTableAdapter.ScalarQuery(id).ToString();
            xrPaymentDate.Text = this.queriesTableAdapter.ScalarQuery2(id).ToString();
        }

    }
}
