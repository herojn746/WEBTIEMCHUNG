using TiemChung.Entity;

namespace TiemChung.Repository.Interface
{
    public interface ICTGoiTiemChungRepository
    {
        Task<ICollection<CTGoiTiemChungEntity>> GetAllCTGoiTiemChung();
        Task<CTGoiTiemChungEntity> GetCTGoiTiemChungById(string maVaccine, string maNhapVaccine);
        Task<List<CTGoiTiemChungEntity>> GetVaccinesByMaGoiTiemChung(string keyId);
        Task<string> CreateCTGoiTiemChung(CTGoiTiemChungEntity entity);
        Task UpdateCTGoiTiemChung(string maVaccine, string maNhapVaccine, CTGoiTiemChungEntity entity);
        Task<CTGoiTiemChungEntity> DeleteCTGoiTiemChung(string maVaccine, string maNhapVaccine, bool isPhysical);


    }
}
