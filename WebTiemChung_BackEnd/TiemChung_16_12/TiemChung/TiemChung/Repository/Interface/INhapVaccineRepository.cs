using TiemChung.Entity;

namespace TiemChung.Repository.Interface
{
    public interface INhapVaccineRepository
    {
        Task<ICollection<NhapVaccineEntity>> GetAllNhapVaccine();
        Task<NhapVaccineEntity> GetNhapVaccineById(string Id);
        Task<ICollection<NhapVaccineEntity>> SearchNhapVaccine(string searchKey);
        Task<string> CreateNhapVaccine(NhapVaccineEntity entity);
        Task UpdateNhapVaccine(string id, NhapVaccineEntity entity);
        Task<NhapVaccineEntity> DeleteNhapVaccine(string entity, bool isPhysical);
    }
}
