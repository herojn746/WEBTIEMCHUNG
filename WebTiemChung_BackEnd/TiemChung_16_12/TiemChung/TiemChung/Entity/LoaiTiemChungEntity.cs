using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("LoaiTiemChung")]
    public class LoaiTiemChungEntity:Entity
    {
        public string TenLoaiTiem { get; set; }
        public string MoTa { get; set; }
        public float GiaTien { get; set; }
        public string TrangThai { get; set; }

        public virtual ICollection<GoiTiemChungEntity>? GoiTiemChung { get; set; }

        [ForeignKey("NhanVien")]
        public string MaNhanVien { get; set; }
        public virtual NhanVienEntity NhanVien { get; set; }
    }
}
