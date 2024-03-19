using TiemChung.Entity;

namespace TiemChung.Repository.Interface
{
    public interface IXuatVaccineRepository
    {
        Task<ICollection<XuatVaccineEntity>> GetAllXuatVaccine();
        Task<XuatVaccineEntity> GetXuatVaccineById(string Id);
        //Task<ICollection<XuatVaccineEntity>> SearchXuatVaccine(string searchKey);
        Task<string> CreateXuatVaccine(XuatVaccineEntity entity);
        Task UpdateXuatVaccine(string id, XuatVaccineEntity entity);
        Task<XuatVaccineEntity> DeleteXuatVaccine(string entity, bool isPhysical);
    }
}
