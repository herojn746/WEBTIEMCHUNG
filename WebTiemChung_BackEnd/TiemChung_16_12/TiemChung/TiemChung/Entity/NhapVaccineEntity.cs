using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("NhapVaccine")]
    public class NhapVaccineEntity:Entity
    {
        public DateTime NgayTao { get; set; }
        public virtual ICollection<CTNhapVaccineEntity> CTNhapVaccine { get; set; }
 

        [ForeignKey("NhaCungCap")]
        public string MaNhaCungCap { get; set; }
        public virtual NhaCungCapEntity NhaCungCap { get; set; }

        [ForeignKey("NhanVien")]
        public string MaNhanVien { get; set; }
        public virtual NhanVienEntity NhanVien { get; set; }
    }
}
