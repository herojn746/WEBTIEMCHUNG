using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using TiemChung.Entity;
using TiemChung.Model;
using TiemChung.Repository.Interface;
namespace TiemChung.Repository
{
    public class ThongTinTiemChungRepository : IThongTinTiemChungRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public ThongTinTiemChungRepository(AppDbContext Context, IMapper mapper, IDistributedCache distributedCache)
        {
            _context = Context;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }
        // khach hang tạo lịch tiem
        public async Task<string> CreateThongTinTiemChungKhachHang(ThongTinTiemChungEntity entity)
        {
            var existingThongTinTiemChung = await _context.thongTinTiemChungEnities
              .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingThongTinTiemChung != null)
            {
                throw new Exception(message: "Id is already exist!");
            }

            var mapEntity = _mapper.Map<ThongTinTiemChungEntity>(entity);
            await _context.thongTinTiemChungEnities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return mapEntity.Id;
        }
        // gia dinh
        public async Task<string> CreateThongTinTiemChungHoGiaDinh(ThongTinTiemChungEntity entity)
        {
            var existingThongTinTiemChung = await _context.thongTinTiemChungEnities
              .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingThongTinTiemChung != null)
            {
                throw new Exception(message: "Id is already exist!");
            }

            var mapEntity = _mapper.Map<ThongTinTiemChungEntity>(entity);
            await _context.thongTinTiemChungEnities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return mapEntity.Id;
        }
        //=========================================================
        public async Task<string> CreateThongTinTiemChung(ThongTinTiemChungEntity entity)
        {
            var existingThongTinTiemChung = await _context.thongTinTiemChungEnities
              .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingThongTinTiemChung != null)
            {
                throw new Exception(message: "Id is already exist!");
            }

            var mapEntity = _mapper.Map<ThongTinTiemChungEntity>(entity);
            await _context.thongTinTiemChungEnities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return mapEntity.Id;
        }

        public async Task<ThongTinTiemChungEntity> DeleteThongTinTiemChung(string id, bool isPhysical)
        {
            try
            {
                var entity = await _context.thongTinTiemChungEnities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy thông tin tiêm chủng.");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.thongTinTiemChungEnities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.thongTinTiemChungEnities.Update(entity);
                    }

                    await _context.SaveChangesAsync();

                }
                return entity;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> ReportThongTinTiemChung(DateTime ngayBD, DateTime NgayKT)
        {
            try
            {

                var entities = await _context.thongTinTiemChungEnities
                   .Where(c => c.DeletedTime != null && c.DeletedTime >= ngayBD && c.DeletedTime <= NgayKT)
                   /*.ToListAsync()*/
                   .CountAsync();
                if (entities == 0)
                {
                    throw new Exception("Không có khách hàng đã hủy!");
                }

                return entities;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICollection<ThongTinTiemChungEntity>> GetAllThongTinTiemChung()
        {
            try
            {
                var entities = await _context.thongTinTiemChungEnities
                     .Where(c => c.DeletedTime == null)
                     .ToListAsync();

                if (entities is null)
                {
                    throw new Exception("Empty list!");
                }
                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //==========================================================================
        // lấy thông tin mà khách hàng đó đã đăng ký
        public async Task<ICollection<ThongTinTiemChungEntity>> GetAllThongTinTiemChungKhachHang()
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var entities = await _context.thongTinTiemChungEnities
                     .Where(c => c.DeletedTime == null && c.MaKhachHang == userId)
                     .ToListAsync();

                if (entities is null)
                {
                    throw new Exception("Empty list!");
                }
                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //==========================================================================


        public async Task<ThongTinTiemChungEntity> GetThongTinTiemChungById(string id)
        {
            var entity = await _context.thongTinTiemChungEnities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

        public async Task<ICollection<ThongTinTiemChungEntity>> GetAllThongTinTiemChungByIdHoGD(string id)
        {
            try
            {
                var entities = await _context.thongTinTiemChungEnities
                     .Where(c =>c.MaHoGiaDinh==id && c.DeletedTime == null)
                     .ToListAsync();

                if (entities is null)
                {
                    throw new Exception("Empty list!");
                }
                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICollection<ThongTinTiemChungEntity>> SearchThongTinTiemChung(string searchKey)
        {
            var ListKH = await _context.thongTinTiemChungEnities.ToListAsync();

            // Filter the list and materialize the results
            var filteredList = ListKH.Where(c => (
                c.Id.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                c.MaKhachHang.Contains(searchKey, StringComparison.OrdinalIgnoreCase)  
            ) && c.DeletedTime == null).ToList();

            return filteredList;
        }

        public async Task UpdateThongTinTiemChung(string id, ThongTinTiemChungEntity entity)
        {

            if (entity == null)
            {
                throw new Exception(message: "The entity field is required!");
            }

            var existingThongTinTiemChung = await _context.thongTinTiemChungEnities
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

            if (existingThongTinTiemChung == null)
            {
                throw new Exception(message: "Không tìm thấy thông tin tiêm chủng!");
            }
            if (existingThongTinTiemChung.NgayDangKy > entity.NgayTiem)
            {
                throw new Exception(message: "Ngày tiêm phải sau ngày đăng ký");
            }

            _context.Entry(existingThongTinTiemChung).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            
        }
        public async Task UpdateThongTinTiemChungNhanVien(string id, ThongTinTiemChungEntity entity)
        {

            if (entity == null)
            {
                throw new Exception(message: "The entity field is required!");
            }

            var existingThongTinTiemChung = await _context.thongTinTiemChungEnities
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

            if (existingThongTinTiemChung == null)
            {
                throw new Exception(message: "Không tìm thấy thông tin tiêm chủng!");
            }
            if (existingThongTinTiemChung.NgayDangKy > entity.NgayTiem)
            {
                throw new Exception(message: "Ngày tiêm phải sau ngày đăng ký");
            }
            existingThongTinTiemChung.NgayTiem = entity.NgayTiem;
            existingThongTinTiemChung.GioTiem = entity.GioTiem;
            existingThongTinTiemChung.KetQua = entity.KetQua;
            existingThongTinTiemChung.TrangThai = entity.TrangThai;
            existingThongTinTiemChung.HTTruocTiem = entity.HTTruocTiem;
            existingThongTinTiemChung.HTSauTiem = entity.HTSauTiem;
            //existingThongTinTiemChung.MaLoaiTiemChung = entity.MaLoaiTiemChung;

            //_context.Entry(existingThongTinTiemChung).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

        }
    }
}
