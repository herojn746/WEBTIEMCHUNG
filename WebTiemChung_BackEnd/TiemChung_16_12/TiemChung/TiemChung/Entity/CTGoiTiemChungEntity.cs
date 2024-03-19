using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("CTGoiTiemChung")]
    public class CTGoiTiemChungEntity
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("GoiTiemChung")]
        public string MaGoiTiemChung { get; set; }
        public virtual GoiTiemChungEntity GoiTiemChung { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Vaccine")]
        public string MaVaccine { get; set; }
        public virtual VaccineEntity Vaccine { get; set; }

        public int SoLuong {get; set; }
        public DateTime CreateTimes { get; set; } = DateTime.Now;
        public string? CreateBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

    }
}
