using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class fLogin : DevExpress.XtraEditors.XtraForm
    {
        System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer();
        Label logreg = new Label();

        public fLogin()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.Size = new Size(640, 440);
        }

        private void fLogin_Load(object sender, EventArgs e)
        {
            timer1.Tick += new EventHandler(timer1_go);
            timer2.Tick += new EventHandler(timer2_go);
            timer1.Interval = 10;
            timer2.Interval = 5;
            timer1.Start();

            ptbHideShowPassword.Image = Properties.Resources.hide;
            ptbHideShowPassword.Enabled = false;

            ptbHideShowPassword2.Image = Properties.Resources.hide;
            ptbHideShowPassword2.Enabled = false;
            this.ActiveControl = btnLogin;
        }

        void timer1_go(object sender, EventArgs e)
        {
            Logoposition();
        }

        void timer2_go(object sender, EventArgs e)
        {
            if (logreg.Text == "Đăng ký")
            {
                reg();
            }
            else if (logreg.Text == "Đăng nhập")
            {
                log();
            }
        }

        void Logoposition()
        {
            if (pnlLogin.Left == 320)
            {
                ptbLogo.Top += 1;
                if (ptbLogo.Top > 50)
                {
                    timer1.Stop();
                }
            }
            if (pnlRegister.Left == 320)
            {
                ptbLogo.Top -= 1;
                if (ptbLogo.Top < 34)
                {
                    timer1.Stop();
                }
            }
        }

        void line()
        {
            if (pnlLogin.Left == 320)
            {
                bunifuSeparator2.LineThickness = 2;
                bunifuSeparator2.LineColor = Color.FromArgb(0, 173, 239);
                bunifuSeparator1.LineThickness = 1;
                bunifuSeparator1.LineColor = Color.Silver;
            }
            if (pnlRegister.Left == 320)
            {
                bunifuSeparator2.LineThickness = 1;
                bunifuSeparator2.LineColor = Color.Silver;
                bunifuSeparator1.LineThickness = 2;
                bunifuSeparator1.LineColor = Color.FromArgb(0, 173, 239);
            }
        }

        void reg()
        {
            if (pnlRegister.Left > 320)
            {
                timer1.Start();
                line();

                pnlLogin.Left -= 20;
                pnlRegister.Left -= 20;
                if (pnlRegister.Left == 320)
                {
                    timer2.Stop();
                }
            }
        }
        void log()
        {
            if (pnlLogin.Left < 320)
            {
                timer1.Start();
                line();

                pnlRegister.Left += 20;
                pnlLogin.Left += 20;
                if (pnlLogin.Left == 640)
                {
                    timer2.Stop();
                }
            }
        }

        private void LoginRegister(object sender, EventArgs e)
        {
            Label lr = (Label)sender;

            logreg = lr;
            timer2.Start();
        }

        private void tbxLoginUsername_Enter(object sender, EventArgs e)
        {
            if (tbxLoginUsername.Text == "Tài khoản")
            {
                tbxLoginUsername.Text = string.Empty;
            }
        }

        private void tbxLoginUsername_Leave(object sender, EventArgs e)
        {
            if (tbxLoginUsername.Text == string.Empty)
            {
                tbxLoginUsername.Text = "Tài khoản";
            }
        }

        private void tbxLoginPassword_Enter(object sender, EventArgs e)
        {
            if (tbxLoginPassword.Text == "Mật khẩu")
            {
                tbxLoginPassword.PasswordChar = '*';
                ptbHideShowPassword.Enabled = true;
                tbxLoginPassword.Text = string.Empty;
            }
        }

        private void tbxLoginPassword_Leave(object sender, EventArgs e)
        {
            if (tbxLoginPassword.Text == string.Empty)
            {
                tbxLoginPassword.PasswordChar = '\0';
                ptbHideShowPassword.Enabled = false;
                tbxLoginPassword.Text = "Mật khẩu";
            }
        }

        private void tbxRegisterFullName_Enter(object sender, EventArgs e)
        {
            if (tbxRegisterFullName.Text == "Họ tên")
            {
                tbxRegisterFullName.Text = string.Empty;
            }
        }

        private void tbxRegisterFullName_Leave(object sender, EventArgs e)
        {
            if (tbxRegisterFullName.Text == string.Empty)
            {
                tbxRegisterFullName.Text = "Họ tên";
            }
            else
            {
                tbxRegisterFullName.Text = ChuanHoaHoTen();
            }
        }

        private void tbxRegisterUsername_Enter(object sender, EventArgs e)
        {
            if (tbxRegisterUsername.Text == "Tài khoản")
            {
                tbxRegisterUsername.Text = string.Empty;
            }
        }

        private void tbxRegisterUsername_Leave(object sender, EventArgs e)
        {
            if (tbxRegisterUsername.Text == string.Empty)
            {
                tbxRegisterUsername.Text = "Tài khoản";
            }
        }

        private void tbxRegisterPassword_Enter(object sender, EventArgs e)
        {
            if (tbxRegisterPassword.Text == "Mật khẩu")
            {
                tbxRegisterPassword.PasswordChar = '*';
                ptbHideShowPassword2.Enabled = true;
                tbxRegisterPassword.Text = string.Empty;
            }
        }

        private void tbxRegisterPassword_Leave(object sender, EventArgs e)
        {
            if (tbxRegisterPassword.Text == string.Empty)
            {
                tbxRegisterPassword.PasswordChar = '\0';
                ptbHideShowPassword2.Enabled = false;
                tbxRegisterPassword.Text = "Mật khẩu";
            }
        }
        private void ptbHideShowPassword_Click(object sender, EventArgs e)
        {
            if (tbxLoginPassword.PasswordChar == '*')
            {
                tbxLoginPassword.PasswordChar = '\0';
                ptbHideShowPassword.Image = Properties.Resources.show;
            }
            else
            {
                tbxLoginPassword.PasswordChar = '*';
                ptbHideShowPassword.Image = Properties.Resources.hide;
            }
        }

        private void ptbHideShowPasswod2_Click(object sender, EventArgs e)
        {
            if (tbxRegisterPassword.PasswordChar == '*')
            {
                tbxRegisterPassword.PasswordChar = '\0';
                ptbHideShowPassword2.Image = Properties.Resources.show;
            }
            else
            {
                tbxRegisterPassword.PasswordChar = '*';
                ptbHideShowPassword2.Image = Properties.Resources.hide;
            }
        }
        private void tbxLoginUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void tbxLoginPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void tbxRegisterUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void tbxRegisterPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void ptbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        string ChuanHoaHoTen()
        {
            tbxRegisterFullName.Text = tbxRegisterFullName.Text.Trim();
            for (int i = 0; i < tbxRegisterFullName.Text.Length; i++)
            {
                if (tbxRegisterFullName.Text[i] == ' ' && tbxRegisterFullName.Text[i + 1] == ' ')
                {
                    tbxRegisterFullName.Text = tbxRegisterFullName.Text.Remove(i, 1);
                    i--;
                }
            }
            tbxRegisterFullName.Text = tbxRegisterFullName.Text.ToLower();
            char c = tbxRegisterFullName.Text[0];
            tbxRegisterFullName.Text = tbxRegisterFullName.Text.Remove(0, 1);
            tbxRegisterFullName.Text = tbxRegisterFullName.Text.Insert(0, c.ToString().ToUpper());
            for (int i = 1; i < tbxRegisterFullName.Text.Length; i++)
            {
                if (tbxRegisterFullName.Text[i] == ' ')
                {
                    char c1 = tbxRegisterFullName.Text[i + 1];
                    tbxRegisterFullName.Text = tbxRegisterFullName.Text.Remove(i + 1, 1);
                    tbxRegisterFullName.Text = tbxRegisterFullName.Text.Insert(i + 1, c1.ToString().ToUpper());
                }
            }
            return tbxRegisterFullName.Text;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (tbxRegisterFullName.Text == string.Empty || tbxRegisterUsername.Text == string.Empty || tbxRegisterPassword.Text == string.Empty || tbxRegisterFullName.Text == "Họ tên" || tbxRegisterUsername.Text == "Tài khoản" || tbxRegisterPassword.Text == "Mật khẩu" || rBtnMale.Checked == false && rBtnFemale.Checked == false)
            {
                MessageBox.Show("Thông tin phải được điền đầy đủ !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (DataProvider.ExecuteQuery("SELECT dbo.Accounts.Username FROM dbo.Accounts WHERE dbo.Accounts.Username = '" + tbxRegisterUsername.Text + "'").Rows.Count > 0)
                {
                    MessageBox.Show("Tên tài khoản đã được sử dụng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Random random = new Random();
                string salt = random.Next(100000, 999999).ToString();
                string sex = "Nam";
                sex = rBtnMale.Checked ? "Nam" : "Nữ";
                string encryptPassword = Encrypt("skagre" + tbxRegisterPassword.Text + salt);
                encryptPassword.Replace("'", "''");
                DataProvider.ExecuteQuery("INSERT INTO dbo.Accounts (Username, Password, FullName, Sex, Salt) VALUES ('" + tbxRegisterUsername.Text + "', '" + encryptPassword + "', N'" + tbxRegisterFullName.Text + "', N'" + sex + "','" + salt + "')");
                MessageBox.Show("Đăng ký thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxRegisterFullName.Text = "Họ tên";
                tbxRegisterUsername.Text = "Tài khoản";
                tbxRegisterPassword.Text = "Mật khẩu";
                tbxRegisterPassword.PasswordChar = '\0';
                rBtnMale.Checked = false;
                rBtnFemale.Checked = false;
            }
        }

        private void tbxRegisterFullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetter(e.KeyChar) && (Keys)e.KeyChar != Keys.Back && (Keys)e.KeyChar != Keys.Space)
            {
                e.Handled = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            progress.Visible = true;
            new Thread(() => {
                string salt = string.Empty;
                if (DataProvider.ExecuteQuery("SELECT Salt FROM dbo.Accounts WHERE dbo.Accounts.Username = '" + tbxLoginUsername.Text + "'") != null)
                {
                    foreach (DataRow d in DataProvider.ExecuteQuery("SELECT dbo.Accounts.Salt FROM dbo.Accounts WHERE dbo.Accounts.Username = '" + tbxLoginUsername.Text + "'").Rows)
                    {
                        salt = d["Salt"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Sai tên tài khoản hoặc mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string encryptPassword = Encrypt("skagre" + tbxLoginPassword.Text + salt);

                if (DataProvider.ExecuteQuery("SELECT dbo.Accounts.id FROM dbo.Accounts WHERE dbo.Accounts.Username = '" + tbxLoginUsername.Text + "' AND dbo.Accounts.Password = '" + encryptPassword + "'").Rows.Count > 0)
                {
                    string idAccount = string.Empty;
                    string permission = string.Empty;
                    string accountStatus = string.Empty;
                    foreach (DataRow dr in DataProvider.ExecuteQuery("SELECT dbo.Accounts.id, dbo.Accounts.Permission, dbo.Accounts.AccountStatus FROM dbo.Accounts WHERE dbo.Accounts.Username = '" + tbxLoginUsername.Text + "' AND dbo.Accounts.Password = '" + encryptPassword + "'").Rows)
                    {
                        idAccount = dr["id"].ToString();
                        permission = dr["Permission"].ToString();
                        accountStatus = dr["AccountStatus"].ToString();
                    }

                    if (accountStatus != "Hoạt động")
                    {
                        MessageBox.Show("Tài khoản của bạn đã bị khóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    fMain f = new fMain();
                    f.idAcc = int.Parse(idAccount);
                    f.Perminssion = permission;
                    progress.Visible = false;
                    

                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate ()
                        {
                            this.Hide();
                            f.ShowDialog();
                            this.Show();
                        });
                    }
                    else
                    {
                        this.Hide();
                        f.ShowDialog();
                        this.Show();
                    }
                   
                    

                    tbxLoginUsername.Text = "Tài khoản";
                    tbxLoginPassword.Text = "Mật khẩu";
                    tbxLoginPassword.PasswordChar = '\0';

                    this.ActiveControl = btnLogin;
                }
                else
                {
                    MessageBox.Show("Sai tên tài khoản hoặc mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    progress.Visible = false;
                }
            }).Start();
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

    }
}