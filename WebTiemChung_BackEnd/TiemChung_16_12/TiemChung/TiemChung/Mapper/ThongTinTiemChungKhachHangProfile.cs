using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class ThongTinTiemChungKhachHangProfile:Profile
    {
        public ThongTinTiemChungKhachHangProfile()
        {
            CreateMap<ThongTinTiemChungEntity, ThongTinTiemChungKhachHangModel>()

                .ReverseMap();
        }
    }
}
