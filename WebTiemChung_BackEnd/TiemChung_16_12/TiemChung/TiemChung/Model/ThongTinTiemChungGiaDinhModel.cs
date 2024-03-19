using System.ComponentModel.DataAnnotations;

namespace TiemChung.Model
{
    public class ThongTinTiemChungGiaDinhModel
    {
        public DateTime NgayDangKy { get; set; }
        public int LanTiem { get; set; }
        public string DiaDiemTiem { get; set; }
        public string TrangThai { get; set; }

        [RegularExpression("^[0-9]{9}$|^[0-9]{12}$", ErrorMessage = "Invalid ID")]
        [StringLength(12, MinimumLength = 9, ErrorMessage = "Số chứng minh phải là 9 hoặc 12 số!")]
        public string? CMND { get; set; }
        public string? TenNguoiDK { get; set; }
        public string MaGoiTiemChung { get; set; }
        public string? MaHoGiaDinh { get; set; }
    }
}
