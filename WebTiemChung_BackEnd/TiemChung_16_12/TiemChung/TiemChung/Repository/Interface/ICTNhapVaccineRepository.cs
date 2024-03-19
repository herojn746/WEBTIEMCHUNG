using TiemChung.Entity;

namespace TiemChung.Repository.Interface
{
    public interface ICTNhapVaccineRepository
    {
        Task<ICollection<CTNhapVaccineEntity>> GetAllCTNhapVaccine();
        Task<CTNhapVaccineEntity> GetCTNhapVaccineById(string maVaccine, string maNhapVaccine);
        //Task<ICollection<CTNhapVaccineEntity>> SearchCTNhapVaccine(string searchKey);
        Task<string> CreateCTNhapVaccine(CTNhapVaccineEntity entity);
        Task UpdateCTNhapVaccine(string maVaccine, string maNhapVaccine, CTNhapVaccineEntity entity);
        Task<CTNhapVaccineEntity> DeleteCTNhapVaccine(string maVaccine, string maNhapVaccine, bool isPhysical);

        Dictionary<string, int> GenerateNhapVaccineReport(DateTime startTime, DateTime endTime);
    }
}
