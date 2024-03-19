using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Entity
{
    [Table("Vaccine")]
    public class VaccineEntity:Entity
    {
        public string TenVaccine { get; set; }
        public string NhaSanXuat { get; set; }
        public int SoLuongTon { get; set; }
        public DateTime NgaySX { get; set; }
        public DateTime NgayHetHan { get; set; }
        public float GiaTien { get; set; }

        public virtual ICollection<CTVaccineEntity> CTVaccine { get; set; }
        public virtual ICollection<CTGoiTiemChungEntity> CTGoiTiemChung { get; set; }
        public virtual ICollection<CTXuatVaccineEntity> CTXuatVaccine { get; set; }
        public virtual ICollection<CTNhapVaccineEntity> CTNhapVaccine { get; set; }



        [ForeignKey("LoaiVaccine")]
        public string MaLoaiVaccine { get; set; }
        public virtual LoaiVaccineEntity LoaiVaccine { get; set; }
    }
}
