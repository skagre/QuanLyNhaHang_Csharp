using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QuanLyNhaHang
{
    public class cProducts
    {
        private string productname, category;
        private int amount;
        private ulong price, totalPrice;
        public cProducts(string category, string productname, int amount, ulong price, ulong totalPrice = 0)
        {
            this.ProductName = productname;
            this.Amount = amount;
            this.Price = price;
            this.TotalPrice = totalPrice;
            this.Category = category;
        }

        public cProducts(DataRow row)
        {
            try
            {
                this.ProductName = row["ProductName"].ToString();
                this.Price = (ulong)Convert.ToDouble(row["Price"].ToString());
                this.Amount = (int)row["Amount"];
                this.TotalPrice = (ulong)Convert.ToDouble(row["totalPrice"].ToString());
            }
            catch
            {
                try
                {
                    this.ProductName = row["ProductName"].ToString();
                    this.Price = (ulong)Convert.ToDouble(row["Price"].ToString());
                }
                catch
                {
                    try
                    {
                        this.Amount = (int)row["Amount"];
                        this.TotalPrice = (ulong)Convert.ToDouble(row["totalPrice"].ToString());
                    }
                    catch
                    {
                        this.Category = row["Category"].ToString();
                    }
                }
            }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public ulong TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; }
        }

        public ulong Price
        {
            get { return price; }
            set { price = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public string ProductName
        {
            get { return productname; }
            set { productname = value; }
        }

        public static List<cProducts> GetListProductsByTable(int id)
        {
            List<cProducts> listProducts = new List<cProducts>();

            DataTable data = DataProvider.ExecuteQuery("SELECT dbo.Products.ProductName, dbo.Products.Price, dbo.BillInfo.Amount, dbo.Products.Price * dbo.BillInfo.Amount AS totalPrice FROM dbo.Products, dbo.BillInfo, dbo.Bill WHERE dbo.Products.id = dbo.BillInfo.idProduct AND dbo.BillInfo.idBill = dbo.Bill.id AND dbo.Bill.BillStatus = N'Chưa thanh toán' AND dbo.Bill.idDiningTable = " + id);

            foreach (DataRow item in data.Rows)
            {
                cProducts Products = new cProducts(item);
                listProducts.Add(Products);
            }

            return listProducts;
        }

        public static List<cProducts> GetListProducts()
        {
            List<cProducts> listProducts = new List<cProducts>();

            DataTable data = DataProvider.ExecuteQuery("SELECT dbo.Products.ProductName, dbo.Products.Price FROM dbo.Products");

            foreach (DataRow item in data.Rows)
            {
                cProducts Products = new cProducts(item);
                listProducts.Add(Products);
            }

            return listProducts;
        }

        public static List<cProducts> GetListCategory(bool b)
        {
            List<cProducts> listProducts = new List<cProducts>();

            DataTable data = DataProvider.ExecuteQuery("SELECT DISTINCT dbo.Products.Category FROM dbo.Products");

            if (b)
            {
                DataRow dr = data.NewRow();
                dr["Category"] = "Tất cả";
                data.Rows.InsertAt(dr, 0);
            }

            foreach (DataRow item in data.Rows)
            {
                cProducts Products = new cProducts(item);
                listProducts.Add(Products);
            }

            return listProducts;
        }

        public static List<cProducts> GetListProductsByCategory(string name)
        {
            List<cProducts> a = new List<cProducts>();

            DataTable data = DataProvider.ExecuteQuery("SELECT dbo.Products.ProductName, dbo.Products.Price FROM dbo.Products WHERE dbo.Products.Category = N'" + name + "'");

            foreach (DataRow item in data.Rows)
            {
                cProducts Products = new cProducts(item);
                a.Add(Products);
            }

            return a;
        }

        public static void DeleteProduct(int id)
        {
            DataProvider.ExecuteNonQuery("EXEC USP_DeleteProduct @id", new object[] { id });
        }
    }
}
