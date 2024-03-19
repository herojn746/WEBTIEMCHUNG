using StackExchange.Redis;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TiemChung.Entity;

namespace TiemChung.Model
{
    public class CTGoiTiemChungModel
    {
        public string MaGoiTiemChung { get; set; }
        public string MaVaccine { get; set; }
        public int SoLuong { get; set; }
    }
}
