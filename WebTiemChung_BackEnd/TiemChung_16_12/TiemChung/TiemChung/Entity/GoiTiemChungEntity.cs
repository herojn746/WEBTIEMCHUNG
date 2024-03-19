using System.ComponentModel.DataAnnotations.Schema;
using TiemChung.Model;

namespace TiemChung.Entity
{
    [Table("GoiTiemChung")]
    public class GoiTiemChungEntity:Entity
    {
        public string MoTa { get; set; }
        public float GiamGia { get; set; }
        public float TongTien { get; set; }

        [ForeignKey("LoaiTiemChung")]
        public string MaLoaiTiemChung { get; set; }
        public virtual LoaiTiemChungEntity LoaiTiemChung { get; set; }

        public virtual ICollection<CTGoiTiemChungEntity>? CTGoiTiemChung { get; set; }
        public virtual ICollection<ThongTinTiemChungEntity>? ThongTinTiemChung { get; set; }
        


    }
}
