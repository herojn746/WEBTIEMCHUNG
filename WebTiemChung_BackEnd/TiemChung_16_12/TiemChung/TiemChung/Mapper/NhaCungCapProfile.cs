using AutoMapper;
using TiemChung.Entity;
using TiemChung.Model;

namespace TiemChung.Mapper
{
    public class NhaCungCapProfile:Profile
    {
        public NhaCungCapProfile()
        {
            CreateMap<NhaCungCapEntity, NhaCungCapModel>()

                .ReverseMap();
        }
    }
}
