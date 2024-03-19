using System.ComponentModel.DataAnnotations.Schema;
using TiemChung.Entity;

namespace TiemChung.Model
{
    public class LoaiTiemChungModel
    {
        public string Id { get; set; }
        public string TenLoaiTiem { get; set; }
        public string MoTa { get; set; }
        public float GiaTien { get; set; }
        public string TrangThai { get; set; }
        //public string MaNhanVien { get; set; }
        

    }
}
