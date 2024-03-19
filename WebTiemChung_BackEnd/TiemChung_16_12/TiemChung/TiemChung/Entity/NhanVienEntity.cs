using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("NhanVien")]
    public class NhanVienEntity:Entity
    {
        public string TenNhanVien { get; set; }
        public string Password { get; set; }
        public string SDT { get; set; }
        public string CMND { get; set; }
        public string Role { get; set; }


        public virtual ICollection<LoaiTiemChungEntity> LoaiTiemChung { get; set; }
        public virtual ICollection<XuatVaccineEntity> XuatVaccine { get; set; }
        public virtual ICollection<NhapVaccineEntity> NhapVaccine { get; set; }

    }
    

}
