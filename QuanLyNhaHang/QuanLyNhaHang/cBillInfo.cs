using System;
using System.Data;
using System.Linq;

namespace QuanLyNhaHang
{
    public class cBillInfo
    {
        private int id, billID, foodID, count;
        public cBillInfo(int id, int billID, int foodID, int count)
        {
            this.ID = id;
            this.BillID = billID;
            this.FoodID = foodID;
            this.Count = count;
        }

        public cBillInfo(DataRow row)
        {
            this.ID = (int)row["id"];
            this.BillID = (int)row["idbill"];
            this.FoodID = (int)row["idfood"];
            this.Count = (int)row["count"];
        }
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        public int FoodID
        {
            get { return foodID; }
            set { foodID = value; }
        }
        public int BillID
        {
            get { return billID; }
            set { billID = value; }
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public static void InsertBillInfo(int idBill, int idProduct, int Amount)
        {
            DataProvider.ExecuteNonQuery("USP_InsertBillInfo @idBill , @idProduct , @Amount", new object[] { idBill, idProduct, Amount });
        }
    }
}
