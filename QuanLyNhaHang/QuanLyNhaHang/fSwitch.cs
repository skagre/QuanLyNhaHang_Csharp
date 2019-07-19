using Bunifu.Framework.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class fSwitch : DevExpress.XtraEditors.XtraForm
    {
        public fSwitch()
        {
            InitializeComponent();
        }

        private int checkClick = 0;
        private List<int> listID = new List<int>();
        private List<string> listName = new List<string>();

        private void fSwitch_Load(object sender, EventArgs e)
        {
            loadFpnl();
        }

        private void loadFpnl()
        {
            fpnl.Controls.Clear();
            foreach (cDining_Table item in cDining_Table.LoadAllTableList())
            {
                BunifuTileButton btn = new BunifuTileButton();
                btn.LabelText = item.DiningTableName;
                btn.Click += btn_Click;
                btn.Tag = item;

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

                fpnl.Controls.Add(btn);
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            checkClick++;
            BunifuTileButton button = sender as BunifuTileButton;

            int tableID = ((sender as BunifuTileButton).Tag as cDining_Table).ID;

            listID.Add(tableID);
            listName.Add(button.LabelText);

            button.BackColor = Color.SeaGreen;
            button.ForeColor = Color.Black;
            button.color = Color.SeaGreen;
            button.colorActive = Color.SeaGreen;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (checkClick != 2)
            {
                MessageBox.Show("Chỉ được chọn 2 bàn !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                loadFpnl();
                checkClick = 0;
                listID.Clear();
                listName.Clear();
                return;
            }

            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển bàn {0} qua bàn {1}", listName[0], listName[1]), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                DataProvider.ExecuteQuery("USP_SwitchTable @idTable1 , @idTabel2", new object[] { listID[0], listID[1] });
                MessageBox.Show("Chuyển bàn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadFpnl();
            checkClick = 0;
            listID.Clear();
            listName.Clear();
        }
    }
}