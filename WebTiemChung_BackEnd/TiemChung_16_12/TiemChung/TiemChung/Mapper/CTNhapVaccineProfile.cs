using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class CTNhapVaccineProfile:Profile
    {
        public CTNhapVaccineProfile()
        {
            CreateMap<CTNhapVaccineEntity, CTNhapVaccineModel>()

                .ReverseMap();
        }
    }
}
