using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("HoGiaDinh")]
    public class HoGiaDinhEntity:Entity
    {
        public string TenChuHo { get; set; }
        public virtual ICollection<ThongTinTiemChungEntity>? ThongTinTiemChung { get; set; }
    }
}
