using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class ThongTinTiemChungGiaDinhProfile:Profile
    {
        public ThongTinTiemChungGiaDinhProfile()
        {
            CreateMap<ThongTinTiemChungEntity, ThongTinTiemChungGiaDinhModel>()

                .ReverseMap();
        }
    }
}
