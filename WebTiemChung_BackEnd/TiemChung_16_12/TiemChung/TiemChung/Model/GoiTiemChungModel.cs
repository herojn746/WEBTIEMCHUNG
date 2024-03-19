using System.ComponentModel.DataAnnotations.Schema;
using TiemChung.Entity;

namespace TiemChung.Model
{
    public class GoiTiemChungModel
    {
        public string Id { get; set; }
        public string MoTa { get; set; }
        public float GiamGia { get; set; }
        public string MaLoaiTiemChung { get; set; }
        //public string MaTTTiemChung { get; set; }

    }
}
