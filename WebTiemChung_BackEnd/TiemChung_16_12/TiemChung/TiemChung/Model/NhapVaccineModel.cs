using System.ComponentModel.DataAnnotations.Schema;
using TiemChung.Entity;

namespace TiemChung.Model
{
    public class NhapVaccineModel
    {
        public string Id { get; set; }
        public DateTime NgayTao { get; set; }
        public string MaNhaCungCap { get; set; }
        
    }
}
