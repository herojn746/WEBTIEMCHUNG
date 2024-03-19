using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Repository.Interface
{
    public interface IThongTinTiemChungRepository
    {
        Task<ICollection<ThongTinTiemChungEntity>> GetAllThongTinTiemChung();
        Task<ICollection<ThongTinTiemChungEntity>> GetAllThongTinTiemChungKhachHang();
        Task<ICollection<ThongTinTiemChungEntity>> GetAllThongTinTiemChungByIdHoGD(string id);
        Task<int> ReportThongTinTiemChung(DateTime ngayBD, DateTime NgayKT);
        Task<ThongTinTiemChungEntity> GetThongTinTiemChungById(string Id);
        Task<ICollection<ThongTinTiemChungEntity>> SearchThongTinTiemChung(string searchKey);
        Task<string> CreateThongTinTiemChung(ThongTinTiemChungEntity entity);
        Task<string> CreateThongTinTiemChungKhachHang(ThongTinTiemChungEntity entity);
        Task<string> CreateThongTinTiemChungHoGiaDinh(ThongTinTiemChungEntity entity);
        Task UpdateThongTinTiemChung(string id, ThongTinTiemChungEntity entity);
        Task UpdateThongTinTiemChungNhanVien(string id, ThongTinTiemChungEntity entity);
        Task<ThongTinTiemChungEntity> DeleteThongTinTiemChung(string entity, bool isPhysical);
    }
}
