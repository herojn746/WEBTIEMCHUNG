namespace TiemChung.Model
{
    public class VaccineModel
    {
        public string Id { get; set; }
        public string TenVaccine { get; set; }
        public string NhaSanXuat { get; set; }
        public int SoLuongTon { get; set; }
        public DateTime NgaySX { get; set; }
        public DateTime NgayHetHan { get; set; }
        public string MaLoaiVaccine { get; set; }
        public float GiaTien { get; set; }

    }
}
