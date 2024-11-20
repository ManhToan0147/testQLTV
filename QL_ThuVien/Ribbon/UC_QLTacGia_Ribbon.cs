using QL_ThuVien.Main_UC.QLDocGia;
using QL_ThuVien.Main_UC.QLTacGia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_ThuVien.Ribbon
{
    public partial class UC_QLTacGia_Ribbon : UserControl
    {
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(userControl);
            userControl.BringToFront();
        }

        public UC_QLTacGia_Ribbon()
        {
            InitializeComponent();
        }

        private void UC_QLTacGia_Ribbon_Load(object sender, EventArgs e)
        {
            btnTacGia.Checked = true;
            var uc = new UC_QLTacGia();
            addUserControl(uc);
        }

        private void btnTacGia_Click_1(object sender, EventArgs e)
        {
            var uc = new UC_QLTacGia();
            addUserControl(uc);
        }

        private void btnTacGiaSach_Click(object sender, EventArgs e)
        {
            var uc = new UC_TacGiaSach();
            addUserControl(uc);
        }
    }
}
