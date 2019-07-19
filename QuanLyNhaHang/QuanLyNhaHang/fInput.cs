
using System;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class fInput : DevExpress.XtraEditors.XtraForm
    {
        public fInput()
        {
            InitializeComponent();
        }

        public int amount { get { return int.Parse(tbxInput.Text); } }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            tbxInput.Text = 0.ToString();
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbxInput.Text != string.Empty)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Hãy nhập số lượng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbxInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && (Keys)e.KeyChar != Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void fInput_Load(object sender, EventArgs e)
        {
            tbxInput.Text = "1";
        }
    }
}