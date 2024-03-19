using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("RefreshToken")]
    public class RefreshTokenEntity
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("KhachHang")]
        public string UserId { get; set; }
        public virtual KhachHangEntity KhachHang { get; set; }

        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
