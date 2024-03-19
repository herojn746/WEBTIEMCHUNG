using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class CTVaccineProfile:Profile
    {
        public CTVaccineProfile()
        {
            CreateMap<CTVaccineEntity, CTVaccineModel>()

                .ReverseMap();
        }
    }
}
