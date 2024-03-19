using System.ComponentModel.DataAnnotations.Schema;
using TiemChung.Entity;

namespace TiemChung.Model
{
    public class CTNhapVaccineModel
    {
        //public string Id { get; set; }
        public DateTime NgayTao { get; set; }
        public int SoLuong { get; set; }
        public string MaVaccine { get; set; }
        public string MaNhapVaccine { get; set; }
    }
}
