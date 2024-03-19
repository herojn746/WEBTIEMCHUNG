using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class LoaiVaccineProfile:Profile
    {
        public LoaiVaccineProfile()
        {
            CreateMap<LoaiVaccineEntity, LoaiVaccineModel>()

                .ReverseMap();
        }
    }
}
