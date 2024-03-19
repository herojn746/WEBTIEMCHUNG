using TiemChung.Entity;

namespace TiemChung.Repository.Interface
{
    public interface ICTVaccineRepository
    {
        Task<ICollection<CTVaccineEntity>> GetAllCTVaccine();
        Task<CTVaccineEntity> GetCTVaccineById(string Id);
        Task<ICollection<CTVaccineEntity>> SearchCTVaccine(string searchKey);
        Task<string> CreateCTVaccine(CTVaccineEntity entity);
        Task UpdateCTVaccine(string id, CTVaccineEntity entity);
        Task<CTVaccineEntity> DeleteCTVaccine(string entity, bool isPhysical);
    }
}
