using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class CTXuatVaccineProfile:Profile
    {
        public CTXuatVaccineProfile()
        {
            CreateMap<CTXuatVaccineEntity, CTXuatVaccineModel>()
                .ReverseMap();
        }
    }
}
