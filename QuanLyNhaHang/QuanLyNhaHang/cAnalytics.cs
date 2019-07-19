using System;
using System.Data;
using System.Linq;

namespace QuanLyNhaHang
{
    public class cAnalytics
    {
        public static string BillLast28days()
        {
            int a = (int)DataProvider.ExecuteScalar("SELECT COUNT(id) FROM dbo.Bill WHERE FORMAT(PaymentDate, 'yyyy-MM-dd') > '" + DateTime.Now.AddDays(-28).ToString("yyyy-MM-dd") + "'");
            return a.ToString();
        }

        public static string RevenueLast28days()
        {
            string total = string.Empty;

            foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT SUM(Price * Amount) AS [Total] FROM dbo.Products, dbo.BillInfo, dbo.Bill WHERE dbo.Products.id = dbo.BillInfo.idProduct AND dbo.Bill.id = dbo.BillInfo.idBill AND FORMAT(PaymentDate, 'yyyy-MM-dd') > '" + DateTime.Now.AddDays(-28).ToString("yyyy-MM-dd") + "'").Rows)
            {
                total = dr["Total"].ToString();
            }

            if (total == string.Empty)
            {
                return "0";
            }
            return total;
        }

        public static string BillLifetime()
        {
            int a = (int)DataProvider.ExecuteScalar("SELECT COUNT(id) FROM dbo.Bill");
            return a.ToString();
        }

        public static string RevenueLifetime()
        {
            string total = string.Empty;

            foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT SUM(Price * Amount) AS [Total] FROM dbo.Products, dbo.BillInfo, dbo.Bill WHERE dbo.Products.id = dbo.BillInfo.idProduct AND dbo.Bill.id = dbo.BillInfo.idBill").Rows)
            {
                total = dr["Total"].ToString();
            }

            if (total == string.Empty)
            {
                return "0";
            }
            return total;
        }

        public static string BillToday()
        {
            int a = (int)DataProvider.ExecuteScalar("SELECT COUNT(id) FROM dbo.Bill WHERE FORMAT(PaymentDate, 'yyyy-MM-dd') = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
            return a.ToString();
        }

        public static string RevenueToday()
        {
            string total = string.Empty;

            foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT SUM(Price * Amount) AS [Total] FROM dbo.Products, dbo.BillInfo, dbo.Bill WHERE dbo.Products.id = dbo.BillInfo.idProduct AND dbo.Bill.id = dbo.BillInfo.idBill AND FORMAT(PaymentDate, 'yyyy-MM-dd') = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'").Rows)
            {
                total = dr["Total"].ToString();
            }

            if (total == string.Empty)
            {
                return "0";
            }
            return total;
        }

        public static string BillYesterday()
        {
            int a = (int)DataProvider.ExecuteScalar("SELECT COUNT(id) FROM dbo.Bill WHERE FORMAT(PaymentDate, 'yyyy-MM-dd') = '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "'");
            return a.ToString();
        }

        public static string RevenueYesterday()
        {
            string total = string.Empty;

            foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT SUM(Price * Amount) AS [Total] FROM dbo.Products, dbo.BillInfo, dbo.Bill WHERE dbo.Products.id = dbo.BillInfo.idProduct AND dbo.Bill.id = dbo.BillInfo.idBill AND FORMAT(PaymentDate, 'yyyy-MM-dd') = '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "'").Rows)
            {
                total = dr["Total"].ToString();
            }

            if (total == string.Empty)
            {
                return "0";
            }
            return total;
        }

        public static string BillThisMonth()
        {
            int a = (int)DataProvider.ExecuteScalar("SELECT COUNT(id) FROM dbo.Bill WHERE FORMAT(PaymentDate, 'yyyy-MM') = '" + DateTime.Now.ToString("yyyy-MM") + "'");
            return a.ToString();
        }

        public static string RevenueThisMonth()
        {
            string total = string.Empty;

            foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT SUM(Price * Amount) AS [Total] FROM dbo.Products, dbo.BillInfo, dbo.Bill WHERE dbo.Products.id = dbo.BillInfo.idProduct AND dbo.Bill.id = dbo.BillInfo.idBill AND FORMAT(PaymentDate, 'yyyy-MM') = '" + DateTime.Now.ToString("yyyy-MM") + "'").Rows)
            {
                total = dr["Total"].ToString();
            }

            if (total == string.Empty)
            {
                return "0";
            }
            return total;
        }

        public static string BillLastMonth()
        {
            int a = (int)DataProvider.ExecuteScalar("SELECT COUNT(id) FROM dbo.Bill WHERE FORMAT(PaymentDate, 'yyyy-MM') = '" + DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "'");
            return a.ToString();
        }

        public static string RevenueLastMonth()
        {
            string total = string.Empty;

            foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT SUM(Price * Amount) AS [Total] FROM dbo.Products, dbo.BillInfo, dbo.Bill WHERE dbo.Products.id = dbo.BillInfo.idProduct AND dbo.Bill.id = dbo.BillInfo.idBill AND FORMAT(PaymentDate, 'yyyy-MM') = '" + DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "'").Rows)
            {
                total = dr["Total"].ToString();
            }

            if (total == string.Empty)
            {
                return "0";
            }
            return total;
        }

        public static string BillCustom(string date1, string date2)
        {
            int a = (int)DataProvider.ExecuteScalar("SELECT COUNT(id) FROM dbo.Bill WHERE FORMAT(PaymentDate, 'yyyy-MM') >= '" + date1 + "' AND FORMAT(PaymentDate, 'yyyy-MM') <= '" + date2 + "'");
            return a.ToString();
        }

        public static string RevenueCustom(string date1, string date2)
        {
            string total = string.Empty;

            foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT SUM(Price * Amount) AS [Total] FROM dbo.Products, dbo.BillInfo, dbo.Bill WHERE dbo.Products.id = dbo.BillInfo.idProduct AND dbo.Bill.id = dbo.BillInfo.idBill AND FORMAT(PaymentDate, 'yyyy-MM') >= '" + date1 + "' AND FORMAT(PaymentDate, 'yyyy-MM') <= '" + date2 + "'").Rows)
            {
                total = dr["Total"].ToString();
            }

            if (total == string.Empty)
            {
                return "0";
            }
            return total;
        }
    }
}
