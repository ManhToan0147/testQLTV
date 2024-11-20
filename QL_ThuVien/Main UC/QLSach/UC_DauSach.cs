using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;

namespace QL_ThuVien.Main_UC.QLSach
{
    public partial class UC_DauSach : UserControl
    {
        string strCon = @"Data Source=DESKTOP-HPGDAGQ\SQLEXPRESS;Initial Catalog=QuanLyThuVien3;Integrated Security=True;Encrypt=False";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataTable dt;
        DataView dv;
        bool addNewFlag = false;
        public UC_DauSach()
        {
            InitializeComponent();
        }

        private void LoadComboBox(ComboBox cbo, string tableName, string Ma, string TenMa)
        {
            try
            {
                using (con = new SqlConnection(strCon))
                {
                    con.Open();
                    string sql = $"SELECT * FROM {tableName}";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cbo.DataSource = dt;
                    cbo.ValueMember = Ma;
                    cbo.DisplayMember = TenMa;
                    cbo.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text;
            dv.RowFilter = $"TenDauSach like '%{search}%'";
        }

        private void UC_DauSach_Load(object sender, EventArgs e)
        {
            dgvDSDauSach.ColumnHeadersDefaultCellStyle.Font = new Font(dgvDSDauSach.Font, FontStyle.Bold);
            ShowDauSach();
            LoadComboBox(cboMaLoaiSach, "LoaiSach", "MaLoaiSach", "TenLoaiSach");
            LoadComboBox(cboMaChuDe, "ChuDe", "MaChuDe", "TenChuDe");
            LoadComboBox(cboMaNXB, "NXB", "MaNXB", "TenNXB");
            LoadComboBox(cboMaKho, "KhoSach", "MaKho", "TenKho");
        }

        private void ShowDauSach()
        {
            using (con = new SqlConnection(strCon))
            {
                string sql = "Select * from DauSach";
                adapter = new SqlDataAdapter(sql, con);
                dt = new DataTable();
                adapter.Fill(dt);
            }
            dv = new DataView(dt);
            dgvDSDauSach.DataSource = dv;
        }

        private void TaoMaDS()
        {
            if (cboMaLoaiSach.SelectedValue == null)
            {
                return;
            }
            string selectedLoaiSach = cboMaLoaiSach.SelectedValue.ToString();
            using (con = new SqlConnection(strCon))
            {
                con.Open();
                string sql = $"SELECT MAX(MaDauSach) FROM DauSach WHERE MaDauSach like '%{selectedLoaiSach}%'";
                SqlCommand cmd = new SqlCommand(sql, con);
                var result = cmd.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                {
                    string maxMaDauSach = result.ToString();
                    int number = int.Parse(maxMaDauSach.Substring(selectedLoaiSach.Length));
                    ++number;
                    txtMaDauSach.Text = selectedLoaiSach + number.ToString("D2");
                }
                else
                {
                    txtMaDauSach.Text = selectedLoaiSach + "01";
                }
            }
        }

        private void cboMaLoaiSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (addNewFlag)
            {
                TaoMaDS();
            } 
        }

        string selectedMaDauSach;
        private void NapCT()
        {
            if (dgvDSDauSach.CurrentCell != null && dgvDSDauSach.CurrentCell.RowIndex >= 0)
            {
                int i = dgvDSDauSach.CurrentRow.Index;
                selectedMaDauSach = dgvDSDauSach.Rows[i].Cells["MaDauSach"].Value.ToString();
                txtMaDauSach.Text = selectedMaDauSach;
                txtMaDauSach.Enabled = string.IsNullOrEmpty(txtMaDauSach.Text);

                txtTenDauSach.Text = dgvDSDauSach.Rows[i].Cells["TenDauSach"].Value.ToString();
                txtNamXB.Text = dgvDSDauSach.Rows[i].Cells["NamXuatBan"].Value.ToString();
                txtGiaBia.Text = dgvDSDauSach.Rows[i].Cells["GiaBia"].Value.ToString();
                txtSoTrang.Text = dgvDSDauSach.Rows[i].Cells["SoTrang"].Value.ToString();

                cboMaLoaiSach.SelectedValue = dgvDSDauSach.Rows[i].Cells["MaLoaiSach"].Value.ToString();
                cboMaChuDe.SelectedValue = dgvDSDauSach.Rows[i].Cells["MaChuDe"].Value.ToString();
                cboMaNXB.SelectedValue = dgvDSDauSach.Rows[i].Cells["MaNXB"].Value.ToString();
                cboMaKho.SelectedValue = dgvDSDauSach.Rows[i].Cells["MaKho"].Value.ToString();
            }
        }

        private void dgvDSDauSach_SelectionChanged(object sender, EventArgs e)
        {
            NapCT();
        }

        private void btnTaoMoi_Click(object sender, EventArgs e)
        {
            TaoMaDS();
            txtTenDauSach.Text = "";
            txtNamXB.Text = "";
            txtGiaBia.Text = "";
            txtSoTrang.Text = "";

            cboMaLoaiSach.SelectedIndex = 0;
            cboMaChuDe.SelectedIndex = 0;
            cboMaNXB.SelectedIndex = 0;
            cboMaKho.SelectedIndex = 0;

            txtTenDauSach.Focus();
            addNewFlag = true;
        }
        void DoSQL(string sql)
        {
            using (con = new SqlConnection(strCon))
            {
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (addNewFlag)
                {
                    string maDauSach = txtMaDauSach.Text.Trim();
                    string tenDauSach = txtTenDauSach.Text.Trim();
                    string namXB = txtNamXB.Text.Trim();
                    string giaBia = txtGiaBia.Text.Trim();
                    string soTrang = txtSoTrang.Text.Trim();
                    string maLoaiSach = cboMaLoaiSach.SelectedValue?.ToString();
                    string maChuDe = cboMaChuDe.SelectedValue?.ToString();
                    string maNXB = cboMaNXB.SelectedValue?.ToString();
                    string maKho = cboMaKho.SelectedValue?.ToString();

                    string sql = $"INSERT INTO DauSach VALUES ('{maDauSach}', N'{tenDauSach}', {namXB}, {giaBia}," +
                        $" {soTrang}, '{maLoaiSach}', '{maChuDe}', '{maNXB}', '{maKho}')";
                    DoSQL(sql);
                    MessageBox.Show($"Đã thêm thành công bản ghi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowDauSach();
                    addNewFlag = false;

                    // Tìm dòng chứa mã của bản ghi vừa thêm
                    foreach (DataGridViewRow row in dgvDSDauSach.Rows)
                    {
                        if (row.Cells["MaDauSach"].Value.ToString() == maDauSach)
                        {
                            dgvDSDauSach.ClearSelection();
                            dgvDSDauSach.CurrentCell = row.Cells[0]; // Đặt cell đầu tiên của dòng vừa thêm làm cell hiện tại
                            NapCT();
                            dgvDSDauSach.FirstDisplayedScrollingRowIndex = row.Index; // Cuộn đến dòng vừa thêm
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi khi them du lieu " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSDauSach.SelectedRows.Count > 0)
            {
                DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn xóa các bản ghi đã chọn ?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (rs == DialogResult.Yes)
                {
                    int currentIndex = dgvDSDauSach.CurrentRow.Index;
                    int count = 0;
                    foreach (DataGridViewRow row in dgvDSDauSach.SelectedRows)
                    {
                        string maDauSach = row.Cells["MaDauSach"].Value.ToString();
                        try
                        {
                            string sql = $"DELETE FROM DauSach WHERE MaDauSach = '{maDauSach}'";
                            using (con = new SqlConnection(strCon))
                            {
                                con.Open();
                                cmd = new SqlCommand(sql, con);
                                if (cmd.ExecuteNonQuery() >0)
                                {
                                    count++;
                                }
                            }

                            MessageBox.Show($"Đã xóa {count} bản ghi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ShowDauSach();
                            int beforeRowIndex = currentIndex - 1;
                            dgvDSDauSach.ClearSelection();
                            dgvDSDauSach.CurrentCell = dgvDSDauSach.Rows[beforeRowIndex].Cells[0];
                            NapCT();
                            dgvDSDauSach.FirstDisplayedScrollingRowIndex = beforeRowIndex;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Đầu sách liên quan nhiều tới bảng dữ liệu khác");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Chưa chọn bản ghi nào để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            //Them nhiều bản ghi
            //if (addNewFlag == false)
            //{
            //    int n = dgvDSDauSach.RowCount;
            //    for (int i = 0; i < n - 1; i++)
            //    {
            //        string maDauSach = dgvDSDauSach.Rows[i].Cells["MaDauSach"].Value.ToString();
            //        string tenDauSach = dgvDSDauSach.Rows[i].Cells["TenDauSach"].Value.ToString();
            //        string namXB = dgvDSDauSach.Rows[i].Cells["NamXuatBan"].Value.ToString();
            //        string giaBia = dgvDSDauSach.Rows[i].Cells["GiaBia"].Value.ToString();
            //        string soTrang = dgvDSDauSach.Rows[i].Cells["SoTrang"].Value.ToString();

            //        string maLoaiSach = dgvDSDauSach.Rows[i].Cells["MaLoaiSach"].Value.ToString();
            //        string maChuDe = dgvDSDauSach.Rows[i].Cells["MaChuDe"].Value.ToString();
            //        string maNXB = dgvDSDauSach.Rows[i].Cells["MaNXB"].Value.ToString();
            //        string maKho = dgvDSDauSach.Rows[i].Cells["MaKho"].Value.ToString();

            //        string sql = $"UPDATE DauSach SET " +
            //            $"TenDauSach = N'{tenDauSach}', " +
            //            $"NamXuatBan = {namXB}, " +
            //            $"GiaBia = {giaBia}, " +
            //            $"SoTrang = {soTrang}, " +
            //            $"MaLoaiSach = '{maLoaiSach}', " +
            //            $"MaChuDe = '{maChuDe}', " +
            //            $"MaNXB = '{maNXB}', " +
            //            $"MaKho = '{maKho}' " +
            //            $"WHERE MaDauSach = '{maDauSach}'";
            //        DoSQL(sql);
            //    }
            //    MessageBox.Show($"Đã cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            
            //Sua tren GroupBox

            if (string.IsNullOrEmpty(selectedMaDauSach))
            {
                MessageBox.Show("Chưa chọn bản ghi để sửa", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information); 
                return;
            }

            int currentIndex = dgvDSDauSach.CurrentRow.Index;

            using (con = new SqlConnection(strCon))
            {
                con.Open();
                string sql = $"UPDATE DauSach SET " +
                        $"TenDauSach = N'{txtTenDauSach.Text.Trim()}', " +
                        $"NamXuatBan = {txtNamXB.Text.Trim()}, " +
                        $"GiaBia = {txtGiaBia.Text.Trim()}, " +
                        $"SoTrang = {txtSoTrang.Text.Trim()}, " +
                        $"MaLoaiSach = '{cboMaLoaiSach.SelectedValue}', " +
                        $"MaChuDe = '{cboMaChuDe.SelectedValue}', " +
                        $"MaNXB = '{cboMaNXB.SelectedValue}', " +
                        $"MaKho = '{cboMaKho.SelectedValue}' " +
                        $"WHERE MaDauSach = '{selectedMaDauSach}'";
                cmd = new SqlCommand(sql, con);
                try
                {
                    int kq = cmd.ExecuteNonQuery();
                    if (kq > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật " + ex.Message);
                }
            }
            ShowDauSach();
            dgvDSDauSach.ClearSelection();
            dgvDSDauSach.CurrentCell = dgvDSDauSach.Rows[currentIndex].Cells[0];
            NapCT();
            dgvDSDauSach.FirstDisplayedScrollingRowIndex = currentIndex;
        }

        private void txtNamXB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ngăn không cho nhập ký tự không hợp lệ
                errorProvider1.SetError((Control)sender, "Chỉ được nhập số!");
            }
            else
            {
                errorProvider1.SetError((Control)sender, ""); // Xóa thông báo lỗi nếu nhập đúng
            }
        }

        private void txtGiaBia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ngăn không cho nhập ký tự không hợp lệ
                errorProvider1.SetError((Control)sender, "Chỉ được nhập số!");
            }
            else
            {
                errorProvider1.SetError((Control)sender, ""); // Xóa thông báo lỗi nếu nhập đúng
            }
        }

        private void txtSoTrang_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ngăn không cho nhập ký tự không hợp lệ
                errorProvider1.SetError((Control)sender, "Chỉ được nhập số!");
            }
            else
            {
                errorProvider1.SetError((Control)sender, ""); // Xóa thông báo lỗi nếu nhập đúng
            }
        }
    }
}
