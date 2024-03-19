using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class KhachHangProfile:Profile
    {
        public KhachHangProfile()
        {
            CreateMap<KhachHangEntity, KhachHangModel>()

                .ReverseMap();
        }
    }
}
