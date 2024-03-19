using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class ThongTinTiemChungNhanVienProfile:Profile
    {
        public ThongTinTiemChungNhanVienProfile()
        {
            CreateMap<ThongTinTiemChungEntity, ThongTinTiemChungNhanVienModel>()

                  .ForMember(d => d.NgayTiem,map=>map.MapFrom(p=>p.NgayTiem))
                  .ForMember(d => d.GioTiem, map => map.MapFrom(p => p.GioTiem))
                  //.ForMember(d => d.KetQua, map => map.MapFrom(p => p.KetQua))
                  .ForMember(d => d.TrangThai, map => map.MapFrom(p => p.TrangThai))
                  .ForMember(d => d.HTTruocTiem, map => map.MapFrom(p => p.HTTruocTiem))
                  .ForMember(d => d.HTSauTiem, map => map.MapFrom(p => p.HTSauTiem))
                  //.ForMember(d => d.MaLoaiTiemChung, map => map.MapFrom(p => p.MaLoaiTiemChung))
                .ReverseMap();
        }
    }
}
