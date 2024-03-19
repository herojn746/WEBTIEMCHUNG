using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Repository.Interface
{
    public interface IGoiTiemChungRepository
    {
        Task<ICollection<GoiTiemChungEntity>> GetAllGoiTiemChung();
        Task<GoiTiemChungEntity> GetGoiTiemChungById(string Id);
        Task<ICollection<GoiTiemChungEntity>> SearchGoiTiemChung(string searchKey);
        Task<string> CreateGoiTiemChung(GoiTiemChungEntity entity);
        Task UpdateGoiTiemChung(string id, GoiTiemChungEntity entity);
        Task<GoiTiemChungEntity> DeleteGoiTiemChung(string entity, bool isPhysical);

        Task<ICollection<GoiTiemChungEntity>> GetMaLoaiTiemChungById(string Id);
    }
}
