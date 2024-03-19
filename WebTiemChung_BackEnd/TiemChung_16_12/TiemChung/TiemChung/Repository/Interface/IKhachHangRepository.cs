using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Repository.Interface
{
    public interface IKhachHangRepository
    {
        Task<ICollection<KhachHangEntity>> GetAllKhachHang();
        Task<KhachHangEntity> GetKhachHangById(string Id);
        Task< ICollection<KhachHangEntity>> SearchKhachHang(string searchKey);
        Task<string> CreateKhachHang(KhachHangModel entity);
        Task<dynamic> Login(LoginModel entity);
        //Task Logout();
        Task<dynamic> ChangePassword (ChangePasswordRequest c);
        Task  UpdateKhachHang(string id, KhachHangModel entity);
        Task<KhachHangEntity> DeleteKhachHang(string entity, bool isPhysical);
    }
}
