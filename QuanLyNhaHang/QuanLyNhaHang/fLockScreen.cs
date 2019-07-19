using System;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class fLockScreen : DevExpress.XtraEditors.XtraForm
    {
        public fLockScreen()
        {
            InitializeComponent();
        }
        public int idAcc;
        private bool Check = false;

        public bool check { get { return Check; } }

        private void tbxInput_Enter(object sender, EventArgs e)
        {
            tbxInput.isPassword = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string salt = string.Empty, pass = string.Empty;
            if (DataProvider.ExecuteQuery("SELECT dbo.Accounts.Salt FROM dbo.Accounts WHERE id = " + idAcc) != null)
            {
                foreach (DataRow d in DataProvider.ExecuteQuery("SELECT dbo.Accounts.Password, dbo.Accounts.Salt FROM dbo.Accounts WHERE id = " + idAcc).Rows)
                {
                    salt = d["Salt"].ToString();
                    pass = d["Password"].ToString();
                }
            }
            string encryptPassword = Encrypt("skagre" + tbxInput.Text + salt);
            if (encryptPassword != pass)
            {
                MessageBox.Show("Mật khẩu không đúng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Check = true;
            this.Close();
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

        private void fLockScreen_Load(object sender, EventArgs e)
        {
            this.ActiveControl = tbxInput;
        }

    }
}