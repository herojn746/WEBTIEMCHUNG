using AutoMapper;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using TiemChung.Entity;
using TiemChung.Repository.Interface;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace TiemChung.Repository
{
    public class CTXuatVaccineRepository : ICTXuatVaccineRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CTXuatVaccineRepository(AppDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;

        }


        public async Task<string> CreateCTXuatVaccine(CTXuatVaccineEntity entity)
        {
            var existingCTXuatVaccine = await _context.cTXuatVaccineEntities
                .FirstOrDefaultAsync(c => c.MaVaccine == entity.MaVaccine && c.MaXuatVaccine == entity.MaXuatVaccine);

            if (existingCTXuatVaccine != null)
            {
                existingCTXuatVaccine.SoLuong += entity.SoLuong;
                _context.cTXuatVaccineEntities.Update(existingCTXuatVaccine);

                var existingMaVaccine = await _context.vaccineEntities
                    .FirstOrDefaultAsync(c => c.Id == entity.MaVaccine && c.DeletedTime == null);
                if (existingMaVaccine != null)
                {
                    existingMaVaccine.SoLuongTon -= entity.SoLuong;
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

                var existingMaXuat = await _context.xuatVaccineEntities
                    .FirstOrDefaultAsync(c => c.Id == entity.MaXuatVaccine && c.DeletedTime == null);
                if (existingMaXuat == null)
                {
                    throw new Exception(message: "Mã xuất Vaccine không tồn tại!");
                }

                if (existingMaVaccine.SoLuongTon < entity.SoLuong)
                {
                    throw new Exception($"Không đủ số lượng vaccine để xuất! (Số lượng hiện tại trong kho: {existingMaVaccine.SoLuongTon})");
                }

                 
                existingMaVaccine.SoLuongTon -= entity.SoLuong;
                _context.vaccineEntities.Update(existingMaVaccine);

                // Thêm phiếu xuất vaccine mới
                var mapEntity = _mapper.Map<CTXuatVaccineEntity>(entity);
                await _context.cTXuatVaccineEntities.AddAsync(mapEntity);
            }

            await _context.SaveChangesAsync();

            return $"{entity.MaXuatVaccine}-{entity.MaVaccine}";
        }

        public Dictionary<string, int> GenerateXuatVaccineReport(DateTime startTime, DateTime endTime)
        {
             
            var ctxuatVaccines = _context.cTXuatVaccineEntities
                .Where(c => c.NgayTao >= startTime && c.NgayTao <= endTime)
                .ToList();

            var vaccineReport = new Dictionary<string, int>();

            foreach (var ctxuatVaccine in ctxuatVaccines)
            {
                string maVaccine = ctxuatVaccine.MaVaccine;

                if (vaccineReport.ContainsKey(maVaccine))
                {
                    vaccineReport[maVaccine] += ctxuatVaccine.SoLuong;
                }
                else
                {
                    vaccineReport[maVaccine] = ctxuatVaccine.SoLuong;
                }
            }

            return vaccineReport;
        }
        public async Task<CTXuatVaccineEntity> DeleteCTXuatVaccine(string maVaccine,string maXuatVaccine, bool isPhysical)
        {
            try
            {
                var entity = await _context.cTXuatVaccineEntities.FirstOrDefaultAsync(e => e.MaVaccine == maVaccine && e.MaXuatVaccine==maXuatVaccine  && e.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy Id chi tiết xuất vaccine!");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.cTXuatVaccineEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.cTXuatVaccineEntities.Update(entity);
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

        public async Task<ICollection<CTXuatVaccineEntity>> GetAllCTXuatVaccine()
        {
            try
            {
                var entities = await _context.cTXuatVaccineEntities
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

        public async Task<CTXuatVaccineEntity> GetCTXuatVaccineById(string maVaccine, string maXuatVaccine)
        {
            var entity = await _context.cTXuatVaccineEntities.FirstOrDefaultAsync(e => e.MaVaccine == maVaccine && e.MaXuatVaccine == maXuatVaccine && e.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

         

        public async Task UpdateCTXuatVaccine(string maVaccine, string maXuatVaccine, CTXuatVaccineEntity entity)
        {

            if (entity == null)
            {
                throw new Exception(message: "Thông tin cập nhật rỗng!");
            }

            var existingCTXuatVaccine = await _context.cTXuatVaccineEntities
                .FirstOrDefaultAsync(e => e.MaVaccine == maVaccine && e.MaXuatVaccine == maXuatVaccine && e.DeletedTime == null);
            if (existingCTXuatVaccine == null)
            {
                throw new Exception(message: "Không tìm thấy chi tiết xuất vaccine!");
            }

            var existingMaVaccine = await _context.vaccineEntities
                .FirstOrDefaultAsync(c => c.Id == maVaccine && c.DeletedTime == null);
            if (existingMaVaccine == null)
            {
                throw new Exception(message: "Mã Vaccine không tồn tại!");
            }

            var existingMaXuat = await _context.xuatVaccineEntities
                .FirstOrDefaultAsync(c => c.Id == maXuatVaccine && c.DeletedTime == null);
            if (existingMaXuat == null)
            {
                throw new Exception(message: "Mã xuất Vaccine không tồn tại!");
            }

            if (existingCTXuatVaccine.SoLuong > entity.SoLuong)
            {
                existingMaVaccine.SoLuongTon = (existingMaVaccine.SoLuongTon != null ? existingMaVaccine.SoLuongTon : 0)

                    + (existingCTXuatVaccine.SoLuong - entity.SoLuong);

            }
            else
            {
                if (existingMaVaccine.SoLuongTon < (entity.SoLuong - existingCTXuatVaccine.SoLuong))
                {
                    throw new Exception(message: $"Số lượng hiện tại trong kho không đủ để thực hiện cập nhật này! Số lượng hiện tại kho đang là: {existingMaVaccine.SoLuongTon}");
                }
                else
                {
                    existingMaVaccine.SoLuongTon = (existingMaVaccine.SoLuongTon != null ? existingMaVaccine.SoLuongTon : 0)

                        - (entity.SoLuong - existingCTXuatVaccine.SoLuong);
                }

            }

            _context.vaccineEntities.Update(existingMaVaccine);

            _context.Entry(existingCTXuatVaccine).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        //=============================================






    }
}
