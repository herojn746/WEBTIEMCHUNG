using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class NhanVienProfile:Profile
    {
        public NhanVienProfile()
        {
            CreateMap<NhanVienEntity, NhanVienModel>()

                .ReverseMap();
        }
    }
}
