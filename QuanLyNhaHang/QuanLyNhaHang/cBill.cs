using System;
using System.Data;
using System.Linq;

namespace QuanLyNhaHang
{
    public class cBill
    {
        private int id;
        private string billstatus;
        private DateTime paymentDate;
        public cBill(int id, DateTime paymentDate, string billstatus)
        {
            this.ID = id;
            this.PaymentDate = paymentDate;
            this.BillStatus = billstatus;
        }

        public cBill(DataRow row)
        {
            this.ID = (int)row["id"];
            this.paymentDate = (DateTime)row["PaymentDate"];
            this.BillStatus = row["BillStatus"].ToString();
        }
        public string BillStatus
        {
            get { return billstatus; }
            set { billstatus = value; }
        }
        public DateTime PaymentDate
        {
            get { return paymentDate; }
            set { paymentDate = value; }
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public static int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.ExecuteQuery("SELECT * FROM dbo.Bill WHERE idDiningTable = " + id + " AND BillStatus = N'Chưa thanh toán'");

            if (data.Rows.Count > 0)
            {
                cBill bill = new cBill(data.Rows[0]);
                return bill.ID;
            }

            return -1;
        }

        public static void InsertBill(int id)
        {
            DataProvider.ExecuteQuery("INSERT INTO dbo.Bill (idDiningTable, BillStatus) VALUES (" + id + ", N'Chưa thanh toán')");
        }

        public static int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.ExecuteScalar("SELECT MAX(id) FROM dbo.Bill");
            }
            catch
            {
                return 1;
            }
        }
    }
}
