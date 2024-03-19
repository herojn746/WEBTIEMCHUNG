using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class HoGiaDinhProfile:Profile
    {
        public HoGiaDinhProfile()
        {
            CreateMap<HoGiaDinhEntity, HoGiaDinhModel>()

                .ReverseMap();
        }
    }
}
