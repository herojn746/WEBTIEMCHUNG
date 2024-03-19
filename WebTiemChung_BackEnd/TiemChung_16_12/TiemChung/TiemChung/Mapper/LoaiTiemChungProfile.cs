using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class LoaiTiemChungProfile:Profile
    {
        public LoaiTiemChungProfile()
        {
            CreateMap<LoaiTiemChungEntity, LoaiTiemChungModel>()

                .ReverseMap();
        }
    }
}
