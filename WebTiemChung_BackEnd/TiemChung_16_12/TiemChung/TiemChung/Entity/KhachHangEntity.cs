using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("KhachHang")]
    public class KhachHangEntity:Entity
    {
        public string TenKhachHang { get; set; }
        public string Password { get; set; }
        public string SDT { get; set; }
        public string CMND { get; set; }

        public string Role { get; set; } = "KhachHang";
        public virtual ICollection<ThongTinTiemChungEntity>? ThongTinTiemChung { get; set; }
        public virtual ICollection<RefreshTokenEntity>? RefreshTokens { get; set; }
    }
}
