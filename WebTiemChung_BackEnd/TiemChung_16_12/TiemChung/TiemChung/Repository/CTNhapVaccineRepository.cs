using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiemChung.Entity;
using TiemChung.Repository.Interface;
namespace TiemChung.Repository
{
    public class CTNhapVaccineRepository : ICTNhapVaccineRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CTNhapVaccineRepository(AppDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;

        }
        public async Task<string> CreateCTNhapVaccine(CTNhapVaccineEntity entity)
        {
            var existingCTNhapVaccine = await _context.cTNhapVaccineEntities
                .FirstOrDefaultAsync(c => c.MaVaccine == entity.MaVaccine && c.MaNhapVaccine == entity.MaNhapVaccine && (c.DeletedTime == null || c.DeletedTime != null));
            if (existingCTNhapVaccine != null)
            {
                existingCTNhapVaccine.SoLuong += entity.SoLuong;
                _context.cTNhapVaccineEntities.Update(existingCTNhapVaccine);

                var existingMaVaccine = await _context.vaccineEntities
                    .FirstOrDefaultAsync(c => c.Id == entity.MaVaccine && c.DeletedTime == null);
                if (existingMaVaccine != null)
                {
                    existingMaVaccine.SoLuongTon += entity.SoLuong;
                    _context.vaccineEntities.Update(existingMaVaccine);
                }
            }
            else
            {
                var existingMaVaccine = await _context.vaccineEntities
                    .FirstOrDefaultAsync(c => c.Id == entity.MaVaccine && c.DeletedTime == null);
                if (existingMaVaccine == null)
                {
                    throw new Exception(message: "Mã Vaccine không tồn tại!");
                }

                var existingMaNhap = await _context.nhapVaccineEntities
                    .FirstOrDefaultAsync(c => c.Id == entity.MaNhapVaccine && c.DeletedTime == null);
                if (existingMaNhap == null)
                {
                    throw new Exception(message: "Mã nhập Vaccine không tồn tại!");
                }

                existingMaVaccine.SoLuongTon = (existingMaVaccine.SoLuongTon != null ? existingMaVaccine.SoLuongTon : 0) + entity.SoLuong;
                _context.vaccineEntities.Update(existingMaVaccine);

                var mapEntity = _mapper.Map<CTNhapVaccineEntity>(entity);
                await _context.cTNhapVaccineEntities.AddAsync(mapEntity);
            }

            await _context.SaveChangesAsync();

            return $"{entity.MaVaccine}-{entity.MaNhapVaccine}";
        }

        public async Task<CTNhapVaccineEntity> DeleteCTNhapVaccine(string maVaccine, string maNhapVaccine, bool isPhysical)
        {
            try
            {
                var entity = await _context.cTNhapVaccineEntities.FirstOrDefaultAsync(c => c.MaVaccine == maVaccine && c.MaNhapVaccine == maNhapVaccine && c.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy Id chi tiết nhập vaccine!");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.cTNhapVaccineEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.cTNhapVaccineEntities.Update(entity);
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

        public async Task<ICollection<CTNhapVaccineEntity>> GetAllCTNhapVaccine()
        {
            try
            {
                var entities = await _context.cTNhapVaccineEntities
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

        public async Task<CTNhapVaccineEntity> GetCTNhapVaccineById(string maVaccine, string maNhapVaccine)
        {
            var entity = await _context.cTNhapVaccineEntities.FirstOrDefaultAsync(c => c.MaVaccine == maVaccine && c.MaNhapVaccine == maNhapVaccine && c.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

        //public async Task<ICollection<CTNhapVaccineEntity>> SearchCTNhapVaccine(string searchKey)
        //{
        //    if (searchKey is null)
        //    {
        //        throw new Exception("Search key rỗng");
        //    }

        //    var ListKH = await _context.cTNhapVaccineEntities.ToListAsync();

        //    // Filter the list and materialize the results
        //    var filteredList = ListKH.Where(c => (
        //        c.Id.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
        //        c.SoLuong.ToString().Contains(searchKey, StringComparison.OrdinalIgnoreCase)
        //    ) && c.DeletedTime == null).ToList();

        //    return filteredList;
        //}
        public Dictionary<string, int> GenerateNhapVaccineReport(DateTime startTime, DateTime endTime)
        {

            var ctNhapVaccines = _context.cTNhapVaccineEntities
                .Where(c => c.NgayTao >= startTime && c.NgayTao <= endTime)
                .ToList();

            // Tạo một từ điển để lưu trữ số lượng vaccine đã xuất của từng MaVaccine
            var vaccineReport = new Dictionary<string, int>();

            // Lặp qua danh sách CTNhapVaccineEntity
            foreach (var ctNhapVaccine in ctNhapVaccines)
            {
                string maVaccine = ctNhapVaccine.MaVaccine;

                // Nếu MaVaccine đã tồn tại trong báo cáo, cộng dồn số lượng đã xuất
                if (vaccineReport.ContainsKey(maVaccine))
                {
                    vaccineReport[maVaccine] += ctNhapVaccine.SoLuong;
                }
                // Nếu MaVaccine chưa tồn tại trong báo cáo, thêm mới và gán số lượng đã xuất
                else
                {
                    vaccineReport[maVaccine] = ctNhapVaccine.SoLuong;
                }
            }

            return vaccineReport;
        }
        public async Task UpdateCTNhapVaccine(string maVaccine, string maNhapVaccine, CTNhapVaccineEntity entity)
        {

            if (entity == null)
            {
                throw new Exception(message: "Thông tin cập nhật rỗng!");
            }

            var existingCTNhapVaccine = await _context.cTNhapVaccineEntities
                .FirstOrDefaultAsync(c => c.MaVaccine == maVaccine && c.MaNhapVaccine == maNhapVaccine && c.DeletedTime == null);

            if (existingCTNhapVaccine == null)
            {
                throw new Exception(message: "Không tìm thấy chi tiết nhập vaccine!");
            }

            var existingMaVaccine = await _context.vaccineEntities
               .FirstOrDefaultAsync(c => c.Id == entity.MaVaccine && c.DeletedTime == null);
            if (existingMaVaccine == null)
            {
                throw new Exception(message: "Mã Vaccine không tồn tại!");
            }

            var existingMaNhap = await _context.nhapVaccineEntities
                .FirstOrDefaultAsync(c => c.Id == entity.MaNhapVaccine && c.DeletedTime == null);
            if (existingMaNhap == null)
            {
                throw new Exception(message: "Mã nhập Vaccine không tồn tại!");
            }

            if (existingCTNhapVaccine.SoLuong > entity.SoLuong)
            {
                existingMaVaccine.SoLuongTon = (existingMaVaccine.SoLuongTon != null ? existingMaVaccine.SoLuongTon : 0)

                    - (existingCTNhapVaccine.SoLuong - entity.SoLuong);
                if (existingMaVaccine.SoLuongTon < 0)
                {
                    throw new Exception(message: "Số lượng trong kho không đủ để thực hiện cập nhật!");
                }
            }
            else
            {
                if (existingMaVaccine.SoLuongTon < (entity.SoLuong - existingCTNhapVaccine.SoLuong))
                {
                    throw new Exception(message: $"Số lượng hiện tại trong kho không đủ để thực hiện cập nhật này! Số lượng hiện tại kho đang là: {existingMaVaccine.SoLuongTon}");
                }
                else
                {
                    existingMaVaccine.SoLuongTon = (existingMaVaccine.SoLuongTon != null ? existingMaVaccine.SoLuongTon : 0)

                        + (entity.SoLuong - existingCTNhapVaccine.SoLuong);
                }

            }

            _context.Entry(existingCTNhapVaccine).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
