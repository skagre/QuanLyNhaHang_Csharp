using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QuanLyNhaHang
{
    public class cDining_Table
    {
        private int id;
        private string diningtablename, tablestatus;
        public cDining_Table(int id, string name, string status)
        {
            this.ID = id;
            this.DiningTableName = name;
            this.TableStatus = status;
        }

        public cDining_Table(DataRow row)
        {
            this.ID = (int)row["id"];
            this.DiningTableName = row["DiningTableName"].ToString();
            this.TableStatus = row["TableStatus"].ToString();
        }

        public string TableStatus
        {
            get { return tablestatus; }
            set { tablestatus = value; }
        }

        public string DiningTableName
        {
            get { return diningtablename; }
            set { diningtablename = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public static List<cDining_Table> LoadAllTableList()
        {
            List<cDining_Table> tableList = new List<cDining_Table>();

            DataTable data = DataProvider.ExecuteQuery("SELECT * FROM dbo.Dining_Table");

            foreach (DataRow item in data.Rows)
            {
                cDining_Table table = new cDining_Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

        public static List<cDining_Table> LoadNonEmptyTableList()
        {
            List<cDining_Table> tableList = new List<cDining_Table>();

            DataTable data = DataProvider.ExecuteQuery("SELECT * FROM dbo.Dining_Table WHERE TableStatus = N'Có khách'");

            foreach (DataRow item in data.Rows)
            {
                cDining_Table table = new cDining_Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

        public static List<cDining_Table> LoadEmptyTableList()
        {
            List<cDining_Table> tableList = new List<cDining_Table>();

            DataTable data = DataProvider.ExecuteQuery("SELECT * FROM dbo.Dining_Table WHERE TableStatus = N'Trống'");

            foreach (DataRow item in data.Rows)
            {
                cDining_Table table = new cDining_Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

        public static void DeleteDiningTable(int id)
        {
            DataProvider.ExecuteNonQuery("EXEC USP_DeleteDiningTable @id", new object[] { id });
        }
    }
}
