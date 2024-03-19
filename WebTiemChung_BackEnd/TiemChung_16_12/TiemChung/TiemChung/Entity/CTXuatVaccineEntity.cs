using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("CTXuatVaccine")]
    public class CTXuatVaccineEntity/*:Entity*/
    {

        public DateTime NgayTao { get; set; }
        public int SoLuong { get; set; }

        //[ForeignKey("Vaccine")]
        //public string MaVaccine { get; set; }
        //public virtual VaccineEntity Vaccine { get; set; }

        //[ForeignKey("XuatVaccine")]
        //public string MaXuatVaccine { get; set; }
        //public virtual XuatVaccineEntity XuatVaccine { get; set; }

        [Key]
        [Column(Order = 0)]
        [ForeignKey("Vaccine")]
        public string MaVaccine { get; set; }
        public virtual VaccineEntity Vaccine { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("XuatVaccine")]
        public string MaXuatVaccine { get; set; }
        public virtual XuatVaccineEntity XuatVaccine { get; set; }

        public DateTime CreateTimes { get; set; }=DateTime.Now;
        public string? CreateBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
