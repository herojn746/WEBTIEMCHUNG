using TiemChung.Entity;

namespace TiemChung.Repository.Interface
{
    public interface ICTXuatVaccineRepository
    {
        Task<ICollection<CTXuatVaccineEntity>> GetAllCTXuatVaccine();
        Task<CTXuatVaccineEntity> GetCTXuatVaccineById(string maVaccine, string maXuatVaccine);
        //Task<ICollection<CTXuatVaccineEntity>> SearchCTXuatVaccine(string searchKey);
        Task<string> CreateCTXuatVaccine(CTXuatVaccineEntity entity);
        Task UpdateCTXuatVaccine(string maVaccine, string maXuatVaccine, CTXuatVaccineEntity entity);
        Task<CTXuatVaccineEntity> DeleteCTXuatVaccine(string maVaccine, string maXuatVaccine, bool isPhysical);

        Dictionary<string, int> GenerateXuatVaccineReport(DateTime startTime, DateTime endTime);

         
    }
}
