using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class NhapVaccineProfile:Profile
    {
        public NhapVaccineProfile()
        {
            CreateMap<NhapVaccineEntity, NhapVaccineModel>()

                .ReverseMap();
        }
    }
}
