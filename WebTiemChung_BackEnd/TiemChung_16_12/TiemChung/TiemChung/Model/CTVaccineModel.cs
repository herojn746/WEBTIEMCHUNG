using System.ComponentModel.DataAnnotations.Schema;

namespace TiemChung.Model
{
    public class CTVaccineModel
    {
        public string Id { get; set; }
        public string MoTa { get; set; }
        public string DoTuoi { get; set; }
        public string TanSo { get; set; }
        public string LieuLuong { get; set; }
        public string TrangThai { get; set; }
        public string MaVaccine { get; set; }
    }
}
