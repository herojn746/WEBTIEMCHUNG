using TiemChung.Entity;

namespace TiemChung.Repository.Interface
{
    public interface IHoGiaDinhRepository
    {
        Task<ICollection<HoGiaDinhEntity>> GetAllHoGiaDinh();
        Task<HoGiaDinhEntity> GetHoGiaDinhById(string Id);
        Task<ICollection<HoGiaDinhEntity>> SearchHoGiaDinh(string searchKey);
        Task<string> CreateHoGiaDinh(HoGiaDinhEntity entity);
        Task UpdateHoGiaDinh(string id, HoGiaDinhEntity entity);
        Task<HoGiaDinhEntity> DeleteHoGiaDinh(string entity, bool isPhysical);
    }
}
