using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("NhaCungCap")]
    public class NhaCungCapEntity:Entity
    {
        public string TenNCC { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }

        public virtual ICollection<NhapVaccineEntity> NhapVaccine { get; set; }
    }
}
