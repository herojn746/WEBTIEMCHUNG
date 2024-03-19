using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class VaccineProfile:Profile
    {
        public VaccineProfile()
        {
            CreateMap<VaccineEntity, VaccineModel>()

                .ReverseMap();
        }
    }
}
