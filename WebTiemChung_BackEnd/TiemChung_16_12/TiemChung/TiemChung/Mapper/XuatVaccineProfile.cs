using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class XuatVaccineProfile:Profile
    {
        public XuatVaccineProfile()
        {
            CreateMap<XuatVaccineEntity, XuatVaccineModel>()

                .ReverseMap();
        }
    }
}
