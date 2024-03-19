namespace TiemChung.Mapper
{
    public static class AutoMapper
    {
        public static IServiceCollection AddAutoMapperServices(this IServiceCollection services)
        {
            services.AddAutoMapper(c =>
            {
                 
                c.AddProfile(new KhachHangProfile());
                c.AddProfile(new NhanVienProfile());
                c.AddProfile(new LoaiTiemChungProfile());
                c.AddProfile(new ThongTinTiemChungProfile());
                c.AddProfile(new GoiTiemChungProfile());
                c.AddProfile(new VaccineProfile());
                c.AddProfile(new LoaiVaccineProfile());
                c.AddProfile(new NhaCungCapProfile());
                c.AddProfile(new NhapVaccineProfile());
                c.AddProfile(new CTNhapVaccineProfile());
                c.AddProfile(new CTVaccineProfile());
                c.AddProfile(new ThongTinTiemChungKhachHangProfile());
                c.AddProfile(new CTGoiTiemChungProfile());
                c.AddProfile(new HoGiaDinhProfile());
                c.AddProfile(new ThongTinTiemChungGiaDinhProfile());
            });
            return services;
        }
    }
}
