using System.ComponentModel.DataAnnotations.Schema;
using TiemChung.Entity;

namespace TiemChung.Model
{
    public class ThongTinTiemChungModel
    {
        public string Id { get; set; }
        public DateTime NgayDangKy { get; set; }
        public DateTime? NgayTiem { get; set; }
        public int LanTiem { get; set; }
        public DateTime? GioTiem { get; set; }
        public string DiaDiemTiem { get; set; }
        public string? KetQua { get; set; }
        public string? TrangThai { get; set; }
        public string? HTTruocTiem { get; set; }
        public string? HTSauTiem { get; set; }
        public string MaGoiTiemChung { get; set; }
        //public string MaKhachHang { get; set; }
        //public string MaLoaiTiemChung { get; set; }

    }
}
