using System;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class fChangePassword : DevExpress.XtraEditors.XtraForm
    {
        public fChangePassword()
        {
            InitializeComponent();
        }
        public int idAcc;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbxNewPassword.Text == string.Empty || tbxConfirmNewPassword.Text == string.Empty)
            {
                MessageBox.Show("Hãy nhập mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (tbxNewPassword.Text != tbxConfirmNewPassword.Text)
            {
                MessageBox.Show("Nhập lại mật khẩu không khớp !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string salt = string.Empty, pass = string.Empty;
                if (DataProvider.ExecuteQuery("SELECT dbo.Accounts.Password, dbo.Accounts.Salt FROM dbo.Accounts WHERE id = " + idAcc) != null)
                {
                    foreach (DataRow d in DataProvider.ExecuteQuery("SELECT dbo.Accounts.Password, dbo.Accounts.Salt FROM dbo.Accounts WHERE id = " + idAcc).Rows)
                    {
                        salt = d["Salt"].ToString();
                        pass = d["Password"].ToString();
                    }
                }
                string encryptPassword = Encrypt("skagre" + tbxCurrentPassWord.Text + salt);
                if (encryptPassword != pass)
                {
                    MessageBox.Show("Mật khẩu hiện tại không đúng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbxCurrentPassWord.Text = string.Empty;
                    tbxNewPassword.Text = string.Empty;
                    tbxConfirmNewPassword.Text = string.Empty;
                    return;
                }
                DataProvider.ExecuteQuery("UPDATE dbo.Accounts SET Password = '" + Encrypt("skagre" + tbxNewPassword.Text + salt) + "' WHERE id = " + idAcc);
                MessageBox.Show("Cập nhật thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
        }

        private string Encrypt(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(str));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }

        private void tbxCurrentPassWord_Enter(object sender, EventArgs e)
        {
            tbxCurrentPassWord.isPassword = true;
        }

        private void tbxNewPassword_Enter(object sender, EventArgs e)
        {
            tbxNewPassword.isPassword = true;
        }

        private void tbxConfirmNewPassword_Enter(object sender, EventArgs e)
        {
            tbxConfirmNewPassword.isPassword = true;
        }

        private void fChangePassword_Load(object sender, EventArgs e)
        {
            this.ActiveControl = tbxCurrentPassWord;
        }
    }
}