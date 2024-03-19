using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Repository.Interface
{
    public interface ILoaiTiemChungRepository
    {
        Task<ICollection<LoaiTiemChungEntity>> GetAllLoaiTiemChung();
        Task<LoaiTiemChungEntity> GetLoaiTiemChungById(string Id);
        Task<ICollection<LoaiTiemChungEntity>> SearchLoaiTiemChung(string searchKey);
        Task<string> CreateLoaiTiemChung(LoaiTiemChungEntity entity);
        Task UpdateLoaiTiemChung(string id, LoaiTiemChungEntity entity);
        Task<LoaiTiemChungEntity> DeleteLoaiTiemChung(string entity, bool isPhysical);
    }
}
