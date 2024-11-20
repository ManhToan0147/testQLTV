using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_ThuVien
{
    public partial class frmSignIn : Form
    {
        public frmSignIn()
        {
            InitializeComponent();
        }

        int count = 0;
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (count == 3)
            {
                MessageBox.Show("Đăng nhập thất bại quá 3 lần, chương trình tự thoát", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            // Kiểm tra nếu email bị để trống
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            // Kiểm tra nếu mật khẩu bị để trống
            if (string.IsNullOrEmpty(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return;
            }

            // Kết nối với cơ sở dữ liệu và kiểm tra tài khoản
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=DESKTOP-HPGDAGQ\SQLEXPRESS;Initial Catalog=QuanLyThuVien3;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            // Truy vấn kiểm tra tài khoản và lấy thông tin quyền
            cmd.CommandText = "SELECT * FROM TaiKhoanDN WHERE Email = @email AND MatKhau = @matkhau";
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@matkhau", txtMatKhau.Text);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Lấy giá trị quyền từ kết quả truy vấn
                string quyen = ds.Tables[0].Rows[0]["Quyen"].ToString();
                string tendn = ds.Tables[0].Rows[0]["TenDangNhap"].ToString();
                string mathuthu = ds.Tables[0].Rows[0]["MaThuThu"].ToString();
                // Ẩn form đăng nhập
                this.Hide();

                if (quyen == "admin")
                {
                    MessageBox.Show($"Chào mừng {tendn} đã đăng nhập thành công với quyền {quyen}", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Hiển thị giao diện admin
                    frmLayout trangtru = new frmLayout(quyen, mathuthu);
                    trangtru.Show();
                }
                else if (quyen == "thuthu")
                {
                    // Hiển thị giao diện thủ thư
                    MessageBox.Show($"Chào mừng {tendn} đã đăng nhập thành công với quyền thủ thư", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmLayout thuThuForm = new frmLayout(quyen, mathuthu); // Thay "frmThuThu" bằng tên form của bạn
                    thuThuForm.Show();
                }
            }
            else
            {
                count++;
                // Hiển thị thông báo lỗi nếu email hoặc mật khẩu không đúng
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            if (txtMatKhau.PasswordChar == '*')
            {
                txtMatKhau.PasswordChar = '\0'; // Hiển thị ký tự bình thường
                btnTogglePassword.Image = Properties.Resources.eye__1_; // Đổi icon thành mắt mở
            }
            else
            {
                txtMatKhau.PasswordChar = '*'; // Ẩn ký tự bằng '*'
                btnTogglePassword.Image = Properties.Resources.eye_crossed__1_; // Đổi icon thành mắt gạch chéo
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có muốn thoát khỏi hệ thống không", "Xác nhận", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
