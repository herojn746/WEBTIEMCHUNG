using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("ThongTinTiemChung")]
    public class ThongTinTiemChungEntity:Entity
    {
        public DateTime NgayDangKy { get; set; }
        public DateTime? NgayTiem { get; set; }
        public int LanTiem { get; set; }
        public DateTime? GioTiem { get; set; }
        public string DiaDiemTiem { get; set; }
        public string? CMND { get; set; }
        public string? TenNguoiDK { get; set; }
        public string? KetQua { get; set; }
        public string? TrangThai { get; set; }
        public string? HTTruocTiem { get; set; }
        public string? HTSauTiem { get; set; }

        [ForeignKey("KhachHang")]
        public string MaKhachHang { get; set; }
        public virtual KhachHangEntity KhachHang { get; set; }

        [ForeignKey("GoiTiemChung")]
        public string MaGoiTiemChung { get; set; }
        public virtual GoiTiemChungEntity GoiTiemChung { get; set; }

        [ForeignKey("HoGiaDinh")]
        public string? MaHoGiaDinh { get; set; }
        public virtual HoGiaDinhEntity? HoGiaDinh { get; set; }


    }
}
