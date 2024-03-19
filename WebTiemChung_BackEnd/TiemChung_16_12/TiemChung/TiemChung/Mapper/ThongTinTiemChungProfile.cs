using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class ThongTinTiemChungProfile: Profile
    {
        public ThongTinTiemChungProfile()
        {
            CreateMap<ThongTinTiemChungEntity, ThongTinTiemChungModel>()

                .ReverseMap();
        }
    }
}
