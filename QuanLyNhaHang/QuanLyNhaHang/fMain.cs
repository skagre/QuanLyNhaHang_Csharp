using Bunifu.Framework.UI;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class fMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public fMain()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public int idAcc;
        public string Perminssion;
        private int tableID;
        private bool CheckEdit, CheckEdit2;
        private void fMain_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSetEmployees.Employees' table. You can move, or remove it, as needed.
            this.employeesTableAdapter.Fill(this.dataSetEmployees.Employees);
            // TODO: This line of code loads data into the 'dataSetProducts.Products' table. You can move, or remove it, as needed.
            this.productsTableAdapter.Fill(this.dataSetProducts.Products);

            timer.Start();


            panel.Visible = true;
            pnl0.Visible = false;  
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            barStaticItemTime.Caption = "Giờ hệ thống:" + DateTime.Now.ToString("  HH':'mm':'ss     dd-MM-yyy");
        }

        #region Region Author
        private void barButtonItemBAC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/skagre");
        }

        private void barButtonItemCHAU_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/lengocchau99");
        }
        #endregion

        #region FlowLayoutPanel
        private void loadFpnl(List<cDining_Table> t)
        {
            fpnl.Controls.Clear();
            foreach (cDining_Table item in t)
            {
                BunifuTileButton btn = new BunifuTileButton();
                btn.LabelText = item.DiningTableName;
                btn.Click += btn_Click;
                btn.Tag = item;

                btn.ContextMenuStrip = cmsFpnl;

                btn.Size = new Size(90, 90);
                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;
                btn.color = Color.White;
                btn.colorActive = Color.White;
                btn.ImagePosition = 10;
                btn.LabelPosition = 30;

                switch (item.TableStatus)
                {
                    case "Trống":
                        btn.Image = Properties.Resources.table2;
                        break;
                    default:
                        btn.Image = Properties.Resources.table1;
                        break;
                }
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate ()
                    {
                        fpnl.Controls.Add(btn);
                    });
                }
                else
                {
                    fpnl.Controls.Add(btn);
                }
            }
        }
        private void btn_Click(object sender, EventArgs e)
        {
            tableID = ((sender as BunifuTileButton).Tag as cDining_Table).ID;
            lblTable.Text = ((sender as BunifuTileButton).Tag as cDining_Table).DiningTableName.ToUpper();
            lsvBill.Tag = (sender as BunifuTileButton).Tag;
            string tableStatus = ((sender as BunifuTileButton).Tag as cDining_Table).TableStatus;

            if (tableStatus == "Trống")
            {
                btnPayment.Enabled = false;
            }
            else
            {
                btnPayment.Enabled = true;
            }
            ShowBill(tableID);
        }

        private void fpnl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cmsFpnl.Show(Cursor.Position);
            }
        }

        private void hiểnThịBànCóNgườiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFpnl(cDining_Table.LoadNonEmptyTableList());
        }

        private void hiểnThịBànTrốngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFpnl(cDining_Table.LoadEmptyTableList());
        }

        private void tấtCảCácBànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFpnl(cDining_Table.LoadAllTableList());
        }

        private void thêmBànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Một bàn mới sẽ được thêm vào ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int addTable = DataProvider.ExecuteQuery("SELECT dbo.Dining_Table.id FROM dbo.Dining_Table").Rows.Count + 1;
                DataProvider.ExecuteQuery("INSERT INTO dbo.Dining_Table (DiningTableName, TableStatus) VALUES (N'Bàn " + addTable.ToString() + "', N'Trống')");

                loadFpnl(cDining_Table.LoadAllTableList());
            }
        }

        private void xóaBànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Một bàn sẽ bị xóa ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string deleteTable = string.Empty;
                    foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT MAX(dbo.Dining_Table.id) AS MaxID FROM dbo.Dining_Table").Rows)
                    {
                        deleteTable = dr["MaxID"].ToString();
                    }
                    if (DataProvider.ExecuteQuery("SELECT * FROM dbo.Dining_Table WHERE DiningTableName = N'Bàn " + deleteTable.ToString() + "' AND TableStatus = N'Có khách'").Rows.Count > 0)
                    {
                        MessageBox.Show("Không thể xóa bàn đang có người !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    cDining_Table.DeleteDiningTable(Convert.ToInt32(deleteTable));
                    loadFpnl(cDining_Table.LoadAllTableList());
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Lỗi " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                loadFpnl(cDining_Table.LoadAllTableList());
            }
        }
        #endregion

        #region ListView Bill
        private void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<cProducts> listBillInfo = cProducts.GetListProductsByTable(id);
            ulong totalPrice = 0;
            foreach (cProducts item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.ProductName.ToString());
                lsvItem.SubItems.Add(item.Amount.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());

                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }

            tbxTotalPrice.Text = totalPrice.ToString();
        }

        private void lsvBill_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lsvBill.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    cmsBill.Show(Cursor.Position);
                }
            }
        }

        private void tsmBillDelete_Click(object sender, EventArgs e)
        {
            string a = string.Empty;
            string b = string.Empty;
            string c = string.Empty;

            if (DataProvider.ExecuteQuery("SELECT dbo.Products.id AS idProduct, Bill.id AS idBill, dbo.BillInfo.Amount AS Amount FROM dbo.Products, dbo.Bill, dbo.BillInfo WHERE dbo.Products.ProductName = N'" + lsvBill.SelectedItems[0].Text + "' AND dbo.Bill.id = dbo.BillInfo.idBill AND dbo.BillInfo.idProduct = dbo.Products.id AND dbo.Bill.BillStatus = N'Chưa thanh toán' AND dbo.Bill.idDiningTable = " + tableID) != null)
            {
                foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT dbo.Products.id AS idProduct, Bill.id AS idBill, dbo.BillInfo.Amount AS Amount FROM dbo.Products, dbo.Bill, dbo.BillInfo WHERE dbo.Products.ProductName = N'" + lsvBill.SelectedItems[0].Text + "' AND dbo.Bill.id = dbo.BillInfo.idBill AND dbo.BillInfo.idProduct = dbo.Products.id AND dbo.Bill.BillStatus = N'Chưa thanh toán' AND dbo.Bill.idDiningTable = " + tableID).Rows)
                {
                    a = dr["idProduct"].ToString();
                    b = dr["idBill"].ToString();
                    c = dr["Amount"].ToString();
                }
            }

            int idProduct = int.Parse(a);
            int idBill = int.Parse(b);
            int Amount = int.Parse(c);

            fInput f = new fInput();
            f.ShowDialog();

            if (f.amount > Amount)
            {
                MessageBox.Show("Số lượng không phù hợp !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (f.amount == Amount)
            {
                DataProvider.ExecuteQuery("DELETE FROM dbo.BillInfo WHERE idFood = " + idProduct + " AND idBill = " + idBill + " AND Amount = " + Amount);
            }
            else
            {
                DataProvider.ExecuteQuery("UPDATE dbo.BillInfo SET Amount = " + (Amount - f.amount) + " WHERE idProduct = " + idProduct + " AND idBill = " + idBill);
            }


            lsvBill.Items.RemoveAt(lsvBill.FocusedItem.Index);

            ShowBill(tableID);
        }
        #endregion    

        #region Menu
        private void LoadMenu()
        {
            lsvMenu.Items.Clear();
            List<cProducts> listMenuInfo = cProducts.GetListProducts();

            foreach (cProducts item in listMenuInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.ProductName.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());

                lsvMenu.Items.Add(lsvItem);
                lsvMenu.Tag = item;
            }
        }

        private void lsvMenu_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lsvMenu.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    cmsMenu.Show(Cursor.Position);
                }
            }
        }

        private void tsmMenuAdd_Click(object sender, EventArgs e)
        {
            cDining_Table table = lsvBill.Tag as cDining_Table;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            fInput f = new fInput();
            f.ShowDialog();

            if (f.amount == 0)
            {
                return;
            }

            string id = string.Empty;
            if (DataProvider.ExecuteQuery("SELECT dbo.Products.id FROM dbo.Products WHERE dbo.Products.ProductName = N'" + lsvMenu.SelectedItems[0].Text + "'") != null)
            {
                foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT dbo.Products.id FROM dbo.Products WHERE dbo.Products.ProductName = N'" + lsvMenu.SelectedItems[0].Text + "'").Rows)
                {
                    id = dr["id"].ToString();
                }
            }

            int idBill = cBill.GetUncheckBillIDByTableID(table.ID);

            int productID = int.Parse(id);

            int amount = f.amount;

            if (idBill == -1)
            {
                cBill.InsertBill(table.ID);
                cBillInfo.InsertBillInfo(cBill.GetMaxIDBill(), productID, amount);
            }
            else
            {
                cBillInfo.InsertBillInfo(idBill, productID, amount);
            }

            ShowBill(table.ID);
            loadFpnl(cDining_Table.LoadAllTableList());
            btnPayment.Enabled = true;
        }

        private void LoadComboBoxCategory(bool b, ComboBox cbx)
        {
            List<cProducts> listMenu = cProducts.GetListCategory(b);
            cbx.DataSource = listMenu;
            cbx.DisplayMember = "Category";
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            lsvMenu.Items.Clear();
            if (cbxCategory.SelectedItem == null)
                return;
            if (cbxCategory.Text == "Tất cả")
            {
                LoadMenu();
                //Thread thread = new Thread(new ThreadStart());
                //thread.Start();
                return;
            }

            List<cProducts> listMenu = cProducts.GetListProductsByCategory(cbxCategory.Text);
            foreach (cProducts item in listMenu)
            {
                ListViewItem lsvItem = new ListViewItem(item.ProductName.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());

                lsvMenu.Items.Add(lsvItem);
                lsvMenu.Tag = item;
            }
        }

        private void tbxSearch_Enter(object sender, EventArgs e)
        {
            if (tbxSearch.text == "Tìm kiếm ...")
            {
                tbxSearch.text = string.Empty;
            }
        }

        private void tbxSearch_Leave(object sender, EventArgs e)
        {
            if (tbxSearch.text == string.Empty)
            {
                tbxSearch.text = "Tìm kiếm ...";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lsvMenu.Items.Clear();

            List<cProducts> listMenu = new List<cProducts>();

            DataTable data = DataProvider.ExecuteQuery("SELECT dbo.Products.ProductName, dbo.Products.Price FROM dbo.Products WHERE dbo.Products.ProductName LIKE N'%" + tbxSearch.text + "%'");

            foreach (DataRow item in data.Rows)
            {
                cProducts menu = new cProducts(item);
                listMenu.Add(menu);
            }

            foreach (cProducts item in listMenu)
            {
                ListViewItem lsvItem = new ListViewItem(item.ProductName.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());

                lsvMenu.Items.Add(lsvItem);
            }

            cbxCategory.SelectedIndex = 0;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadComboBoxCategory(true, cbxCategory);
            cbxCategory.SelectedIndex = 0;
        }
        #endregion

        private void btnPayment_Click(object sender, EventArgs e)
        {
            cDining_Table table = lsvBill.Tag as cDining_Table;

            int idBill = cBill.GetUncheckBillIDByTableID(table.ID);

            if (idBill != -1)
            {
                if (MessageBox.Show("Bạn có chắc thanh toán hóa đơn cho " + table.DiningTableName, "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    DataProvider.ExecuteQuery("UPDATE dbo.Bill SET BillStatus = N'Đã thanh toán', PaymentDate = CURRENT_TIMESTAMP WHERE id = " + idBill);

                    rBill report = new rBill(idBill);
                    ReportPrintTool print = new ReportPrintTool(report);
                    report.ShowPreview();

                    ShowBill(table.ID);

                    loadFpnl(cDining_Table.LoadAllTableList());
                    btnPayment.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            fSwitch f = new fSwitch();
            f.ShowDialog();
            loadFpnl(cDining_Table.LoadAllTableList());
        }

        #region Bar Account
        private void barBtnAccountInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fAccountInfo f = new fAccountInfo();
            f.idAcc = this.idAcc;
            f.ShowDialog();
        }

        private void barBtnChangePassword_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fChangePassword f = new fChangePassword();
            f.idAcc = this.idAcc;
            f.ShowDialog();
        }

        private void barBtnLogout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn đăng xuất ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Close();
            }
        }
        #endregion


        private void barBtnLockScreen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fLockScreen f = new fLockScreen();
            f.idAcc = this.idAcc;

            while (f.check == false)
            {
                f.ShowDialog();
            }
        }

        private void barBtnExchange_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new Thread(() => {
                pnl1.Visible = false;
                pnl2.Visible = false;
                pnl3.Visible = false;
                pnl0.Visible = true;
                pnl0.BringToFront();

                loadFpnl(cDining_Table.LoadAllTableList());
                LoadComboBoxCategory(true, cbxCategory);
                btnPayment.Enabled = false;
            }).Start();
        }

        private void LoadProducts()
        {
            XuLyButton(true);
            if (DataProvider.ExecuteQuery("SELECT * FROM dbo.Products").Rows.Count == 0)
            {
                MessageBox.Show("Hiện tại không có sản phẩm nào trong hệ thống, hãy thêm !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnProductEdit.Enabled = btnProductDelete.Enabled = false;
            }
            else
            {
                btnProductEdit.Enabled = btnProductDelete.Enabled = true;
            }
            this.productsTableAdapter.Fill(this.dataSetProducts.Products);

            LoadComboBoxCategory(false, cbxCategory2);


        }

        private void barBtnProduct_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            pnl0.Visible = true;
            pnl2.Visible = false;
            pnl3.Visible = false;
            pnl1.Visible = true;
            pnl1.BringToFront();

            categoryTextEdit.Location = new Point(10000, 10000);
            btnProduct2.Location = new Point(10000, 10000);
            LoadProducts();

        }

        private void barBtnEmployees_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
            if (Perminssion != "Admin")
            {
                MessageBox.Show("Bạn không có quyền truy cập !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            sexTextEdit.Location = new Point(10000, 10000);
            this.employeesTableAdapter.Fill(this.dataSetEmployees.Employees);
            pnl0.Visible = true;
            pnl1.Visible = true;
            pnl3.Visible = false;
            pnl2.Visible = true;
            pnl2.BringToFront();

            XuLyButton2(true);     
        }

        private void XuLyButton(bool b)
        {
            btnProductAdd.Enabled = btnProductDelete.Enabled = btnProductEdit.Enabled = b;
            btnProductSave.Enabled = btnProductCancel.Enabled = groupboxProductEdit.Enabled = !b;

            categoryTextEdit.Visible = false;
            btnProduct2.Visible = false;
            cbxCategory2.Visible = true;
            btnProduct1.Visible = true;
        }

        private void XuLyButton2(bool b)
        {
            btnEmployeesAdd.Enabled = btnEmployeesDelete.Enabled = btnEmployeesEdit.Enabled = b;
            btnEmployeesSave.Enabled = btnEmployeesCancel.Enabled = groupboxEmployeesEdit.Enabled = !b;
        }

        private void btnProductPrevious_Click(object sender, EventArgs e)
        {
            this.productsBindingSource.MovePrevious();
        }

        private void btnProductNext_Click(object sender, EventArgs e)
        {
            this.productsBindingSource.MoveNext();
        }

        private void btnProductAdd_Click(object sender, EventArgs e)
        {
            XuLyButton(false);
            productsGridControl.Enabled = false;
            productNameTextEdit.Text = string.Empty;
            productNameTextEdit.Focus();
            cbxCategory2.SelectedIndex = 0;
            priceSpinEdit.Value = 0;
            categoryTextEdit.Text = cbxCategory2.Text;
        }
        private void btnProductEdit_Click(object sender, EventArgs e)
        {
            XuLyButton(false);
            productsGridControl.Enabled = false;
            productNameTextEdit.Focus();
            CheckEdit = true;
        }
        private void btnProductSave_Click(object sender, EventArgs e)
        {
            if (priceSpinEdit.Value < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (productNameTextEdit.Text == string.Empty || categoryTextEdit.Text == string.Empty)
            {
                MessageBox.Show("Thông tin chỉnh sửa không được trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (CheckEdit)
            {
                try
                {
                    productsTableAdapter.UpdateQuery(categoryTextEdit.Text, (double)priceSpinEdit.Value, productNameTextEdit.Text, Convert.ToInt32(textEdit1.Text));
                }
                catch(Exception ex)
                {
                    if (ex.Message.Contains("UNIQUE KEY"))
                    {
                        MessageBox.Show("Tên sản phẩm đã tồn tại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                
                CheckEdit = false;

                productsTableAdapter.Fill(this.dataSetProducts.Products);
                productsGridControl.Enabled = true;
                XuLyButton(true);
                return;
            }

            try
            {
                productsTableAdapter.InsertQuery(productNameTextEdit.Text, categoryTextEdit.Text, (double)priceSpinEdit.Value);

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    MessageBox.Show("Tên sản phẩm đã tồn tại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }

            }
            XuLyButton(true);
            productsTableAdapter.Fill(this.dataSetProducts.Products);
            productsGridControl.Enabled = true;
        }

        private void btnProductCancel_Click(object sender, EventArgs e)
        {
            this.productsBindingSource.CancelEdit();
            CheckEdit = false;
            productsGridControl.Enabled = true;
            XuLyButton(true);
        }

        private void btnProductDelete_Click(object sender, EventArgs e)
        {
            productsGridControl.Enabled = false;
            if (MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này ?\nLưu ý: Sản phẩm sẽ bị xóa trong hóa đơn", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    cProducts.DeleteProduct(Convert.ToInt32(textEdit1.Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            productsTableAdapter.Fill(this.dataSetProducts.Products);
            productsGridControl.Enabled = true;
            LoadProducts();
        }

        private void btnEmployeesBlocked_Click(object sender, EventArgs e)
        {
            accountsGridControl.Enabled = false;
            if (MessageBox.Show("Bạn có chắc muốn khóa tài khoản này ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (idTextEdit.Text == idAcc.ToString())
                {
                    MessageBox.Show("Không thể khóa tài khoản chính bạn !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (textEdit2.Text == "Admin")
                {
                    MessageBox.Show("Không thể khóa tài khoản Admin !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DataProvider.ExecuteQuery("UPDATE dbo.Accounts SET dbo.Accounts.AccountStatus = N'Đã bị khóa' WHERE dbo.Accounts.id = N'" + idTextEdit.Text + "'");
                    this.employeesTableAdapter.Fill(this.dataSetEmployees.Employees);
                    MessageBox.Show("Khóa tài khoản thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            accountsGridControl.Enabled = true;
        }

        private void btnEmployeesEdit_Click(object sender, EventArgs e)
        {
            CheckEdit2 = true;
            XuLyButton2(false);
            accountsGridControl.Enabled = false;
            fullNameTextEdit.Focus();
        }

        private void btnEmployeesUnblocked_Click(object sender, EventArgs e)
        {
            accountsGridControl.Enabled = false;
            if (MessageBox.Show("Bạn có chắc muốn mở khóa tài khoản này ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                DataProvider.ExecuteQuery("UPDATE dbo.Accounts SET dbo.Accounts.AccountStatus = N'Hoạt động' WHERE dbo.Accounts.id = N'" + idTextEdit.Text + "'");
                this.employeesTableAdapter.Fill(this.dataSetEmployees.Employees);
                MessageBox.Show("Mở khóa tài khoản thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            accountsGridControl.Enabled = true;
        }

        private void btnEmployeesSave_Click(object sender, EventArgs e)
        {
            if (fullNameTextEdit.Text == string.Empty || sexTextEdit.Text == string.Empty)
            {
                MessageBox.Show("Thông tin chỉnh sửa không được trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (CheckEdit2)
            {
                try
                {
                    string sex = string.Empty;
                    if (radioButton1.Checked)
                    {
                        sex = "Nam";
                    }
                    else
                    {
                        sex = "Nữ";
                    }
                    DataProvider.ExecuteQuery("UPDATE dbo.Accounts SET dbo.Accounts.FullName = N'" + fullNameTextEdit.Text + "', dbo.Accounts.Sex = N'" + sex + "' WHERE dbo.Accounts.id = '" + idTextEdit.Text + "'");
                    this.employeesTableAdapter.Fill(this.dataSetEmployees.Employees);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            CheckEdit2 = false;
            XuLyButton2(true);
            accountsGridControl.Enabled = true;
        }

        private void btnEmployeesCancel_Click(object sender, EventArgs e)
        {
            this.employeesBindingSource.CancelEdit();
            CheckEdit2 = false;
            accountsGridControl.Enabled = true;
            XuLyButton2(true);
        }

        private void btnEmployeesPrevious_Click(object sender, EventArgs e)
        {
            this.employeesBindingSource.MovePrevious();
        }

        private void btnEmployeesNext_Click(object sender, EventArgs e)
        {
            this.employeesBindingSource.MoveNext();
        }

        private void sexTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (sexTextEdit.Text == "Nam")
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
        }

        private void ribbonControl1_SelectedPageChanged(object sender, EventArgs e)
        {
            pnl0.Visible = false;
        }

        private void barBtnBackup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Perminssion != "Admin")
            {
                MessageBox.Show("Bạn không có quyền truy cập !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("Bạn có muốn sao lưu database ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    if (DataProvider.ExecuteQuery("BACKUP DATABASE " + DataProvider.dbName() + " TO  DISK = '" + folderDialog.SelectedPath + "\\backupDB" + "-" + DateTime.Now.ToString("dd-MM-yyyy--HH-mm-ss") + ".bak'") == null)
                    {
                        return;
                    }
                    MessageBox.Show("Backup Database thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void barBtnRestore_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Perminssion != "Admin")
            {
                MessageBox.Show("Bạn không có quyền truy cập !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("Bạn có muốn khôi phục database ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "SQL SERVER database backup files|*.bak";
                openDialog.Title = "Database restore";
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    DataProvider.ExecuteQuery("ALTER DATABASE [" + DataProvider.dbName() + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                    DataProvider.ExecuteQuery("USE MASTER RESTORE DATABASE [" + DataProvider.dbName() + "] FROM DISK='" + openDialog.FileName + "'WITH REPLACE;");
                    DataProvider.ExecuteQuery("ALTER DATABASE [" + DataProvider.dbName() + "] SET MULTI_USER");


                    MessageBox.Show("Restore Database thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("Coming Soon !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("Coming Soon !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("Coming Soon !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("Coming Soon !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barBtnInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("ĐỀ TÀI: QUẢN LÝ NHÀ HÀNG\n\nBáo cáo cuối kỳ môn học Lập trình .Net\n\nLast Update: 12/11/2018", "Xin Chào");
        }

        private void barBtnLicenseKey_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("Miễn phí và sẽ luôn như vậy :)", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void barBtnReportBugs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/skagre");
        }

        private void cbxCategory2_SelectedIndexChanged(object sender, EventArgs e)
        {
            categoryTextEdit.Text = cbxCategory2.Text;
        }

        private void btnProduct1_Click(object sender, EventArgs e)
        {
            categoryTextEdit.Text = string.Empty;
            categoryTextEdit.Location = new Point(121, 108);
            btnProduct2.Location = new Point(288, 108);
            categoryTextEdit.Visible = true;
            btnProduct2.Visible = true;
            cbxCategory2.Visible = false;
            btnProduct1.Visible = false;
        }

        private void btnProduct2_Click(object sender, EventArgs e)
        {
            categoryTextEdit.Text = cbxCategory2.Text;
            categoryTextEdit.Visible = false;
            btnProduct2.Visible = false;
            cbxCategory2.Visible = true;
            btnProduct1.Visible = true;
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            lblBillCustom.Text = cAnalytics.BillCustom(dateEdit1.Text, dateEdit2.Text);
            lblRevenueCustom.Text = cAnalytics.RevenueCustom(dateEdit1.Text, dateEdit2.Text);
            groupBox15.Text = dateEdit1.Text + " - " + dateEdit2.Text;
        }

        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            lblBillCustom.Text = cAnalytics.BillCustom(dateEdit1.Text, dateEdit2.Text);
            lblRevenueCustom.Text = cAnalytics.RevenueCustom(dateEdit1.Text, dateEdit2.Text);
            groupBox15.Text = dateEdit1.Text + " - " + dateEdit2.Text;
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void barBtnAnalytics_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Perminssion != "Admin")
            {
                MessageBox.Show("Bạn không có quyền truy cập !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            pnl0.Visible = true;
            pnl1.Visible = true;
            pnl2.Visible = true;
            pnl3.Visible = true;
            pnl3.BringToFront();


            dateEdit1.Text = DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy");
            dateEdit2.Text = DateTime.Now.ToString("dd-MM-yyyy");
            groupBox15.Text = dateEdit1.Text + " - " + dateEdit2.Text;

            lblBillCustom.Text = cAnalytics.BillCustom(dateEdit1.Text, dateEdit2.Text);
            lblRevenueCustom.Text = cAnalytics.RevenueCustom(dateEdit1.Text, dateEdit2.Text);

            lblBillLast28days.Text = cAnalytics.BillLast28days();
            lblRevenueLast28days.Text = cAnalytics.RevenueLast28days();

            lblBillLifetime.Text = cAnalytics.BillLifetime();
            lblRevenueLifetime.Text = cAnalytics.RevenueLifetime();

            lblBillToday.Text = cAnalytics.BillToday();
            lblRevenueToday.Text = cAnalytics.RevenueToday();

            lblBillYesterday.Text = cAnalytics.BillYesterday();
            lblRevenueYesterday.Text = cAnalytics.RevenueYesterday();

            lblBillThisMonth.Text = cAnalytics.BillThisMonth();
            lblRevenueThisMonth.Text = cAnalytics.RevenueThisMonth();

            lblBillLastMonth.Text = cAnalytics.BillLastMonth();
            lblRevenueLastMonth.Text = cAnalytics.RevenueLastMonth();

            if (Convert.ToDouble(cAnalytics.BillToday()) >= Convert.ToDouble(cAnalytics.BillYesterday()))
            {
                lblPercentBillToday.ForeColor = Color.Green;
                if (Convert.ToDouble(cAnalytics.BillYesterday()) == 0)
                {
                    lblPercentBillToday.Text = "+" + Convert.ToString(Convert.ToDouble(cAnalytics.BillToday()) * 100) + "%";
                }
                else
                {
                    lblPercentBillToday.Text = "+" + Convert.ToString(Math.Round((Convert.ToDouble(cAnalytics.BillToday()) - Convert.ToDouble(cAnalytics.BillYesterday())) / Convert.ToDouble(cAnalytics.BillYesterday()) * 100, 1)) + "%";
                }
            }
            else
            {
                lblPercentBillToday.ForeColor = Color.Red;
                lblPercentBillToday.Text = "-" + Convert.ToString(Math.Round((Convert.ToDouble(cAnalytics.BillYesterday()) - Convert.ToDouble(cAnalytics.BillToday())) / Convert.ToDouble(cAnalytics.BillToday()) * 100, 1)) + "%";
            }

            if (Convert.ToDouble(cAnalytics.RevenueToday()) >= Convert.ToDouble(cAnalytics.RevenueYesterday()))
            {
                lblPercentRevenueToday.ForeColor = Color.Green;
                if (Convert.ToDouble(cAnalytics.RevenueYesterday()) == 0)
                {
                    lblPercentRevenueToday.Text = "+" + Convert.ToString(Convert.ToDouble(cAnalytics.RevenueToday()) * 100) + "%";
                }
                else
                {
                    lblPercentRevenueToday.Text = "+" + Convert.ToString(Math.Round((Convert.ToDouble(cAnalytics.RevenueToday()) - Convert.ToDouble(cAnalytics.RevenueYesterday())) / Convert.ToDouble(cAnalytics.RevenueYesterday()) * 100, 1)) + "%";
                }
            }
            else
            {
                lblPercentRevenueToday.ForeColor = Color.Red;
                lblPercentRevenueToday.Text = "-" + Convert.ToString(Math.Round((Convert.ToDouble(cAnalytics.RevenueYesterday()) - Convert.ToDouble(cAnalytics.RevenueToday())) / Convert.ToDouble(cAnalytics.RevenueToday()) * 100, 1)) + "%";
            }

            //

            if (Convert.ToDouble(cAnalytics.BillThisMonth()) >= Convert.ToDouble(cAnalytics.BillLastMonth()))
            {
                lblPercentBillThisMonth.ForeColor = Color.Green;
                if (Convert.ToDouble(cAnalytics.BillLastMonth()) == 0)
                {
                    lblPercentBillThisMonth.Text = "+" + Convert.ToString(Convert.ToDouble(cAnalytics.BillThisMonth()) * 100) + "%";
                }
                else
                {
                    lblPercentBillThisMonth.Text = "+" + Convert.ToString(Math.Round((Convert.ToDouble(cAnalytics.BillThisMonth()) - Convert.ToDouble(cAnalytics.BillLastMonth())) / Convert.ToDouble(cAnalytics.BillLastMonth()) * 100, 1)) + "%";
                }
            }
            else
            {
                lblPercentBillThisMonth.ForeColor = Color.Red;
                lblPercentBillThisMonth.Text = "-" + Convert.ToString(Math.Round((Convert.ToDouble(cAnalytics.BillLastMonth()) - Convert.ToDouble(cAnalytics.BillThisMonth())) / Convert.ToDouble(cAnalytics.BillThisMonth()) * 100, 1)) + "%";
            }

            if (Convert.ToDouble(cAnalytics.RevenueThisMonth()) >= Convert.ToDouble(cAnalytics.RevenueLastMonth()))
            {
                lblPercentRevenueThisMonth.ForeColor = Color.Green;
                if (Convert.ToDouble(cAnalytics.RevenueYesterday()) == 0)
                {
                    lblPercentRevenueThisMonth.Text = "+" + Convert.ToString(Convert.ToDouble(cAnalytics.RevenueThisMonth()) * 100) + "%";
                }
                else
                {
                    lblPercentRevenueThisMonth.Text = "+" + Convert.ToString(Math.Round((Convert.ToDouble(cAnalytics.RevenueThisMonth()) - Convert.ToDouble(cAnalytics.RevenueLastMonth())) / Convert.ToDouble(cAnalytics.RevenueLastMonth()) * 100, 1)) + "%";
                }
            }
            else
            {
                lblPercentRevenueThisMonth.ForeColor = Color.Red;
                lblPercentRevenueThisMonth.Text = "-" + Convert.ToString(Math.Round((Convert.ToDouble(cAnalytics.RevenueLastMonth()) - Convert.ToDouble(cAnalytics.RevenueThisMonth())) / Convert.ToDouble(cAnalytics.RevenueThisMonth()) * 100, 1)) + "%";
            }




            //Chart
            string total = string.Empty;
            for (int i = -28; i <= 0; i++)
            {
                foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT SUM(Price * Amount) AS [Total] FROM dbo.Products, dbo.BillInfo, dbo.Bill WHERE dbo.Products.id = dbo.BillInfo.idProduct AND dbo.Bill.id = dbo.BillInfo.idBill AND FORMAT(PaymentDate, 'yyyy-MM-dd') = '" + DateTime.Now.AddDays(i).ToString("yyyy-MM-dd") + "'").Rows)
                {
                    total = dr["Total"].ToString();
                }
                if (total == string.Empty)
                {
                    chartControl.Series["Tổng doanh thu"].Points.AddPoint(DateTime.Now.AddDays(i).ToString("dd-MM-yyyy"), 0);
                }
                else
                {
                    chartControl.Series["Tổng doanh thu"].Points.AddPoint(DateTime.Now.AddDays(i).ToString("dd-MM-yyyy"), Convert.ToDouble(total));
                }
            }

        }
    }
}
