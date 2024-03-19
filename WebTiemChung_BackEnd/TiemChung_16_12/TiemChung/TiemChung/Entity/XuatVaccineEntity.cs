using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("XuatVaccine")]
    public class XuatVaccineEntity:Entity
    {
        public DateTime NgayTao { get; set; }

        public virtual ICollection<CTXuatVaccineEntity> CTXuatVaccine { get; set; }

        [ForeignKey("NhanVien")]
        public string MaNhanVien { get; set; }
        public virtual NhanVienEntity NhanVien { get; set; }
    }
}
