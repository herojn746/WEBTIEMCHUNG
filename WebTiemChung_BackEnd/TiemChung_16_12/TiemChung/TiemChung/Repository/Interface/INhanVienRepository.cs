using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Repository.Interface
{
    public interface INhanVienRepository
    {
        Task<ICollection<NhanVienEntity>> GetAllNhanVien();
        Task<NhanVienEntity> GetNhanVienById(string Id);
        Task<ICollection<NhanVienEntity>> SearchNhanVien(string searchKey);
        Task<string> CreateNhanVien(NhanVienModel entity);
        Task<NhanVienEntity> Login(LoginModel entity);
        //Task ChangePassword(ChangePasswordRequest c);
        Task UpdateNhanVien(string id, NhanVienModel entity);
        Task<NhanVienEntity> DeleteNhanVien(string entity, bool isPhysical);
    }
}
