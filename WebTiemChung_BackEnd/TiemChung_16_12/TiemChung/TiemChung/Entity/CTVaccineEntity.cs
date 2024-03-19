using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("ChiTietVaccine")]
    public class CTVaccineEntity:Entity
    {
        public string MoTa { get; set; }
        public string DoTuoi { get; set; }
        public string TanSo { get; set; }
        public string LieuLuong { get; set; }
        public string TrangThai { get; set; }

        [ForeignKey("Vaccine")]
        public string MaVaccine { get; set; }
        public virtual VaccineEntity Vaccine { get; set; }
    }
}
