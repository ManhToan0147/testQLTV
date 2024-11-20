using QL_ThuVien.Main_UC.QLSach;
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
    public partial class UC_QLSach_Ribbon : UserControl
    {
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(userControl);
            userControl.BringToFront();
        }
        public UC_QLSach_Ribbon()
        {
            InitializeComponent();
            
        }

        private void UC_QLSach_Ribbon_Load(object sender, EventArgs e)
        {
            btnChiTietSach.Checked = true;
            var uc = new UC_ChiTietSach();
            addUserControl(uc);
            timer1.Start();
        }

        private void btnChiTietSach_Click(object sender, EventArgs e)
        {
            var uc = new UC_ChiTietSach();
            addUserControl(uc);
        }

        private void btnDauSach_Click_1(object sender, EventArgs e)
        {
            var uc = new UC_DauSach();
            addUserControl(uc);
        }

        private void btnCuonSach_Click(object sender, EventArgs e)
        {
            var uc = new UC_CuonSach();
            addUserControl(uc);
        }

        private bool isCollapsed;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isCollapsed)
            {
                btnDanhMuc.Image = Properties.Resources.caret_up;
                dropDown_DanhMuc.Height += 10;
                if (dropDown_DanhMuc.Size == dropDown_DanhMuc.MaximumSize)
                {
                    timer1.Stop();
                    isCollapsed = false;
                }
            }
            else
            {
                btnDanhMuc.Image = Properties.Resources.caret_down;
                dropDown_DanhMuc.Height -= 10;
                if (dropDown_DanhMuc.Size == dropDown_DanhMuc.MinimumSize)
                {
                    timer1.Stop();
                    isCollapsed = true;
                }
            }
        }

        private void btnDanhMuc_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void btnLoaiSach_Click(object sender, EventArgs e)
        {
            var f = new frmLoaiSach();
            f.ShowDialog();
        }

        private void btnChuDe_Click(object sender, EventArgs e)
        {
            var f = new frmChuDe();
            f.ShowDialog();
        }

        private void btnNXB_Click(object sender, EventArgs e)
        {
            var f = new frmNXB();
            f.ShowDialog();
        }

        private void btnKhoSach_Click(object sender, EventArgs e)
        {
            var f= new frmKhoSach();
            f.ShowDialog();
        }
    }
}
