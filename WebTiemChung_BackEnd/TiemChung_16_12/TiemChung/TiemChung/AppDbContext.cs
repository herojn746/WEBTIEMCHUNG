using Microsoft.EntityFrameworkCore;
using TiemChung.Entity;

namespace TiemChung
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //}

        //------mở ra khi migrations database 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CTXuatVaccineEntity>()
            .HasKey(c => new { c.MaVaccine, c.MaXuatVaccine });

            modelBuilder.Entity<CTNhapVaccineEntity>()
            .HasKey(c => new { c.MaVaccine, c.MaNhapVaccine });

            modelBuilder.Entity<CTGoiTiemChungEntity>()
            .HasKey(c => new { c.MaGoiTiemChung, c.MaVaccine });

            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("server=HIEU\\MINHHIEU;database=TiemChung;uid=sa;pwd=123;TrustServerCertificate=True");
        }
        public DbSet<GoiTiemChungEntity> goiTiemChungEntities { get; set; }
        public DbSet<CTGoiTiemChungEntity> cTGoiTiemChungEntities { get; set; }
        public DbSet<CTNhapVaccineEntity> cTNhapVaccineEntities { get; set; }
        public DbSet<CTVaccineEntity> cTVaccineEntities  { get; set; }
        public DbSet<CTXuatVaccineEntity> cTXuatVaccineEntities  { get; set; }
        public DbSet<KhachHangEntity> khachHangEntities { get; set; }
        public DbSet<LoaiTiemChungEntity> loaiTiemChungEntities  { get; set; }
        public DbSet<LoaiVaccineEntity> loaiVaccineEntities { get; set; }
        public DbSet<NhaCungCapEntity > nhaCungCapEntities  { get; set; }
        public DbSet<NhanVienEntity>nhanVienEntities  { get; set; }
        public DbSet<NhapVaccineEntity> nhapVaccineEntities  { get; set; }
        public DbSet<ThongTinTiemChungEntity> thongTinTiemChungEnities { get; set; }
        public DbSet<VaccineEntity> vaccineEntities  { get; set; }
        public DbSet<XuatVaccineEntity> xuatVaccineEntities  { get; set; }
        public DbSet<RefreshTokenEntity> refreshTokenEntities { get; set; }
        public DbSet<HoGiaDinhEntity> hoGiaDinhEntities { get; set; }
    }
}
