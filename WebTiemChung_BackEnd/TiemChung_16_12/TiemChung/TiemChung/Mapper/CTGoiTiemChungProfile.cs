using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class CTGoiTiemChungProfile:Profile
    {
        public CTGoiTiemChungProfile()
        {
            CreateMap<CTGoiTiemChungEntity, CTGoiTiemChungModel>()

                .ReverseMap();
        }
    }
}
