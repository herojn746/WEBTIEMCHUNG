using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("LoaiVaccine")]
    public class LoaiVaccineEntity:Entity
    {
        public string TenLoai { get; set; }
        public virtual ICollection<VaccineEntity> Vaccine { get; set; }
    }
}
