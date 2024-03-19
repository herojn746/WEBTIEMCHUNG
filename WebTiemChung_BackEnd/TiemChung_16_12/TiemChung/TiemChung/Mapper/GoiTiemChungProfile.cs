using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class GoiTiemChungProfile:Profile
    {
        public GoiTiemChungProfile()
        {
            CreateMap<GoiTiemChungEntity, GoiTiemChungModel>()

                .ReverseMap();
        }
    }
}
