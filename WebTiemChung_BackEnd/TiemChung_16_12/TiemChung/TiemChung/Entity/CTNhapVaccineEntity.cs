using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("CTNhapVaccine")]
    public class CTNhapVaccineEntity/*:Entity*/
    {
        public DateTime NgayTao { get; set; }
        public int SoLuong { get; set; }

        [Key]
        [Column(Order = 0)]
        [ForeignKey("Vaccine")]
        public string MaVaccine { get; set; }
        public virtual VaccineEntity Vaccine { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("NhapVaccine")]
        public string MaNhapVaccine { get; set; }
        public virtual NhapVaccineEntity NhapVaccine { get; set; }

        public DateTime CreateTimes { get; set; } = DateTime.Now;
        public string? CreateBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
