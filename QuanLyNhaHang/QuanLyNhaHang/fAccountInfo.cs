using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class fAccountInfo : DevExpress.XtraEditors.XtraForm
    {
        public fAccountInfo()
        {
            InitializeComponent();
        }
        public int idAcc;

        private void fAccountInfo_Load(object sender, EventArgs e)
        {
            if (DataProvider.ExecuteQuery("SELECT dbo.Accounts.id, dbo.Accounts.Username, dbo.Accounts.FullName, dbo.Accounts.Sex, FORMAT(dbo.Accounts.DateCreated, 'dd-MM-yyyy   HH:mm:ss') AS DateCreated, dbo.Accounts.Permission, dbo.Accounts.AccountStatus FROM dbo.Accounts WHERE id = " + idAcc) != null)
            {
                foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT dbo.Accounts.id, dbo.Accounts.Username, dbo.Accounts.FullName, dbo.Accounts.Sex, FORMAT(dbo.Accounts.DateCreated, 'dd-MM-yyyy   HH:mm:ss') AS DateCreated, dbo.Accounts.Permission, dbo.Accounts.AccountStatus FROM dbo.Accounts WHERE id = " + idAcc).Rows)
                {
                    tbxID.Text = dr["id"].ToString();
                    tbxUsername.Text = dr["Username"].ToString();
                    tbxFullName.Text = dr["FullName"].ToString();
                    if (dr["Sex"].ToString() == "Nam")
                    {
                        rBtnMale.Checked = true;
                    }
                    else
                    {
                        rBtnFemale.Checked = true;
                    }
                    tbxDayCreated.Text = dr["DateCreated"].ToString();
                    tbxPermission.Text = dr["Permission"].ToString();
                    tbxStatus.Text = dr["AccountStatus"].ToString();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string sex = string.Empty;
            if (rBtnMale.Checked == true)
            {
                sex = "Nam";
            }
            else
            {
                sex = "Nữ";
            }
            DataProvider.ExecuteQuery("UPDATE dbo.Accounts SET FullName = N'" + tbxFullName.Text + "', Sex = N'" + sex + "' WHERE id = " + idAcc);
            MessageBox.Show("Cập nhật thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tbxFullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetter(e.KeyChar) && (Keys)e.KeyChar != Keys.Back && (Keys)e.KeyChar != Keys.Space)
            {
                e.Handled = true;
            }
        }
    }
}