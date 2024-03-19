using TiemChung.Entity;

namespace TiemChung.Repository.Interface
{
    public interface ILoaiVaccineRepository
    {
        Task<ICollection<LoaiVaccineEntity>> GetAllLoaiVaccine();
        Task<LoaiVaccineEntity> GetLoaiVaccineById(string Id);
        Task<ICollection<LoaiVaccineEntity>> SearchLoaiVaccine(string searchKey);
        Task<string> CreateLoaiVaccine(LoaiVaccineEntity entity);
        Task UpdateLoaiVaccine(string id, LoaiVaccineEntity entity);
        Task<LoaiVaccineEntity> DeleteLoaiVaccine(string entity, bool isPhysical);
    }
}
