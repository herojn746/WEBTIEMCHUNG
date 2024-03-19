using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Repository.Interface
{
    public interface INhaCungCapRepository
    {
        Task<ICollection<NhaCungCapEntity>> GetAllNhaCungCap();
        Task<NhaCungCapEntity> GetNhaCungCapById(string Id);
        Task<ICollection<NhaCungCapEntity>> SearchNhaCungCap(string searchKey);
        Task<string> CreateNhaCungCap(NhaCungCapEntity entity);
        Task UpdateNhaCungCap(string id, NhaCungCapEntity entity);
        Task<NhaCungCapEntity> DeleteNhaCungCap(string entity, bool isPhysical);
    }
}
