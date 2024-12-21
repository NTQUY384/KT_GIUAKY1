using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DE01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StudentDB studentDB = new StudentDB();
            List<SinhVien> sinhViens = studentDB.SinhViens.ToList();
            List<Lop> lops = studentDB.Lops.ToList();
            if (dtSinhVien.Columns.Count == 0)
            {
                dtSinhVien.Columns.Add("MaSV", "Mã SV");
                dtSinhVien.Columns.Add("HoTenSV", "Họ Tên SV");
                dtSinhVien.Columns.Add("NgaySinh", "Ngày Sinh");
                dtSinhVien.Columns.Add("Lop", "Tên Lớp");
            }

            combobox(lops);
            Grid(sinhViens);
        }


        private void combobox(List<Lop> listLop)
        {
            this.cbolop.DataSource = listLop;
            this.cbolop.DisplayMember = "TenLop"; 
            this.cbolop.ValueMember = "MaLop";   
        }


        private void Grid(List<SinhVien> list) { 
            dtSinhVien.Rows.Clear();
            foreach (var item in list)
            {
                int index = dtSinhVien.Rows.Add();
                dtSinhVien.Rows[index].Cells[0].Value = item.MaSV;
                dtSinhVien.Rows[index].Cells[1].Value = item.HoTenSV;
                dtSinhVien.Rows[index].Cells[2].Value = item.NgaySinh;
                dtSinhVien.Rows[index].Cells[3].Value = item.Lop?.TenLop ?? "N/A";
            }


        }

        private void btthem_Click(object sender, EventArgs e)
        {
            try
            {
                StudentDB studentDB = new StudentDB();

                SinhVien newStudent = new SinhVien
                {
                    MaSV = txtMaSV.Text.Trim(),
                    HoTenSV = txtHoTen.Text.Trim(),
                    NgaySinh = dtngaysinh.Value,
                    MaLop = cbolop.SelectedValue.ToString()
                };

                studentDB.SinhViens.Add(newStudent);
                studentDB.SaveChanges();

                List<SinhVien> sinhViens = studentDB.SinhViens.ToList();
                Grid(sinhViens);

                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btsua_Click(object sender, EventArgs e)
        {
            try
            {
                StudentDB studentDB = new StudentDB();
                string selectedMaSV = txtMaSV.Text.Trim();

                SinhVien existingStudent = studentDB.SinhViens.FirstOrDefault(sv => sv.MaSV == selectedMaSV);

                if (existingStudent != null)
                {
                    existingStudent.HoTenSV = txtHoTen.Text.Trim();
                    existingStudent.NgaySinh = dtngaysinh.Value;
                    existingStudent.MaLop = cbolop.SelectedValue.ToString();

                    studentDB.SaveChanges();

                    List<SinhVien> sinhViens = studentDB.SinhViens.ToList();
                    Grid(sinhViens);

                    MessageBox.Show("Sửa thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btxoa_Click(object sender, EventArgs e)
        {
            try
            {
                StudentDB studentDB = new StudentDB();
                string selectedMaSV = txtMaSV.Text.Trim();

                SinhVien existingStudent = studentDB.SinhViens.FirstOrDefault(sv => sv.MaSV == selectedMaSV);

                if (existingStudent != null)
                {
                    studentDB.SinhViens.Remove(existingStudent);
                    studentDB.SaveChanges();

                    List<SinhVien> sinhViens = studentDB.SinhViens.ToList();
                    Grid(sinhViens);

                    MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dtSinhVien.Rows[e.RowIndex];

                    txtMaSV.Text = selectedRow.Cells[0].Value?.ToString() ?? string.Empty;
                    txtHoTen.Text = selectedRow.Cells[1].Value?.ToString() ?? string.Empty;

                    if (DateTime.TryParse(selectedRow.Cells[2].Value?.ToString(), out DateTime ngaySinh))
                    {
                        dtngaysinh.Value = ngaySinh;
                    }
                    else
                    {
                        dtngaysinh.Value = DateTime.Now; 
                    }
                    cbolop.SelectedValue = selectedRow.Cells[3].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xử lý sự kiện CellClick: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btluu_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn lưu tất cả các thay đổi?",
                    "Xác nhận lưu",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    StudentDB studentDB = new StudentDB();
                    studentDB.SaveChanges();

                    MessageBox.Show("Lưu thay đổi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    List<SinhVien> sinhViens = studentDB.SinhViens.ToList();
                    Grid(sinhViens);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thay đổi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btkhong_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn hủy bỏ các thay đổi?","Xác nhận hủy", MessageBoxButtons.YesNo,MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    StudentDB studentDB = new StudentDB();
                    List<SinhVien> sinhViens = studentDB.SinhViens.ToList();

                    Grid(sinhViens);
                    ClearInputs();

                    MessageBox.Show("Đã hủy các thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hủy thay đổi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputs()
        {
            txtMaSV.Clear();
            txtHoTen.Clear();
            dtngaysinh.Value = DateTime.Now;
            cbolop.SelectedIndex = -1; 
        }
        private void btthoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?","Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close(); 
            }
        }

        private void TimKiem(string tenSinhVien)
        {
            try
            {
                StudentDB studentDB = new StudentDB();
                List<SinhVien> danhSach = studentDB.SinhViens
                    .Where(sv => sv.HoTenSV.Contains(tenSinhVien)) 
                    .ToList();

                if (danhSach.Count > 0)
                {
                    Grid(danhSach); 
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Grid(studentDB.SinhViens.ToList()); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void bttim_Click(object sender, EventArgs e)
        {
            string tenSinhVien = txttim.Text.Trim();

            if (!string.IsNullOrWhiteSpace(tenSinhVien))
            {
                TimKiem(tenSinhVien);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
