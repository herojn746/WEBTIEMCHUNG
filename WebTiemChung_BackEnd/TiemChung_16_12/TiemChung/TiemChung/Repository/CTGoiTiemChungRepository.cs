using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using TiemChung.Entity;
using TiemChung.Repository.Interface;

namespace TiemChung.Repository
{
    public class CTGoiTiemChungRepository : ICTGoiTiemChungRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CTGoiTiemChungRepository(AppDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;

        }
        public async Task<string> CreateCTGoiTiemChung(CTGoiTiemChungEntity entity)
        {
            var existingCTGoiTiemChung = await _context.cTGoiTiemChungEntities
                .FirstOrDefaultAsync(c => c.MaVaccine == entity.MaVaccine && c.MaGoiTiemChung == entity.MaGoiTiemChung
                && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingCTGoiTiemChung != null)
            {
                throw new Exception(message: "Id already exists!");
            }

            var existingGoiTiemChung = await _context.goiTiemChungEntities
                .FirstOrDefaultAsync(c => c.Id == entity.MaGoiTiemChung && c.DeletedTime == null);

            if (existingGoiTiemChung == null)
            {
                throw new Exception("Mã gói tiêm chủng không tồn tại!");
            }

            var existingVaccine = await _context.vaccineEntities
                .FirstOrDefaultAsync(c => c.Id == entity.MaVaccine && c.DeletedTime == null);

            if (existingVaccine == null)
            {
                throw new Exception("Mã Vaccine không tồn tại!");
            }

            existingGoiTiemChung.TongTien = existingVaccine.GiaTien * entity.SoLuong;
            _context.goiTiemChungEntities.Update(existingGoiTiemChung);

            var mapEntity = _mapper.Map<CTGoiTiemChungEntity>(entity);

            await _context.cTGoiTiemChungEntities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return $"{entity.MaGoiTiemChung}-{entity.MaVaccine}";
        }

        public async Task<CTGoiTiemChungEntity> DeleteCTGoiTiemChung(string maGoiTiemChung, string maVaccine, bool isPhysical)
        {
            try
            {
                var entity = await _context.cTGoiTiemChungEntities.FirstOrDefaultAsync(c => c.MaVaccine == maVaccine && c.MaGoiTiemChung == maGoiTiemChung && c.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy chi tiết gói tiêm chung!");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.cTGoiTiemChungEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.cTGoiTiemChungEntities.Update(entity);
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

        public async Task<ICollection<CTGoiTiemChungEntity>> GetAllCTGoiTiemChung()
        {
            try
            {
                var entities = await _context.cTGoiTiemChungEntities
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

        public async Task<CTGoiTiemChungEntity> GetCTGoiTiemChungById(string maGoiTiemChung, string maVaccine)
        {
            var entity = await _context.cTGoiTiemChungEntities.FirstOrDefaultAsync(c => c.MaVaccine == maVaccine && c.MaGoiTiemChung == maGoiTiemChung && c.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

        //public async Task<string> GetTenVaccineByMaGoiTiemChung(string keyId)
        //{
        //    var query = from vaccine in _context.vaccineEntities
        //                join ctGoiTiemChung in _context.cTGoiTiemChungEntities on vaccine.Id equals ctGoiTiemChung.MaVaccine
        //                where ctGoiTiemChung.MaGoiTiemChung == keyId && ctGoiTiemChung.DeletedTime == null && vaccine.DeletedTime == null
        //                select vaccine.TenVaccine;

        //    var tenVaccine = await query.FirstOrDefaultAsync();

        //    if (tenVaccine is null)
        //    {
        //        throw new Exception("Not found or already deleted.");
        //    }

        //    return tenVaccine;
        //}
        //public async Task<List<string>> GetTenVaccineByMaGoiTiemChung(string keyId)
        //{
        //    var query = from vaccine in _context.vaccineEntities
        //                join ctGoiTiemChung in _context.cTGoiTiemChungEntities on vaccine.Id equals ctGoiTiemChung.MaVaccine
        //                where ctGoiTiemChung.MaGoiTiemChung == keyId && ctGoiTiemChung.DeletedTime == null && vaccine.DeletedTime == null
        //                select vaccine.TenVaccine;

        //    var tenVaccineList = await query.ToListAsync();

        //    if (tenVaccineList.Count == 0)
        //    {
        //        throw new Exception("Not found or already deleted.");
        //    }

        //    return tenVaccineList;
        //}

        public async Task<List<CTGoiTiemChungEntity>> GetVaccinesByMaGoiTiemChung(string keyId)
        {
            var query = from vaccine in _context.vaccineEntities
                        join ctGoiTiemChung in _context.cTGoiTiemChungEntities on vaccine.Id equals ctGoiTiemChung.MaVaccine
                        where ctGoiTiemChung.MaGoiTiemChung == keyId && ctGoiTiemChung.DeletedTime == null && vaccine.DeletedTime == null
                        select ctGoiTiemChung;

            var vaccineList = await query.ToListAsync();

            if (vaccineList.Count == 0)
            {
                throw new Exception("Not found or already deleted.");
            }

            return vaccineList;
        }

        public async Task UpdateCTGoiTiemChung(string maGoiTiemChung, string maVaccine, CTGoiTiemChungEntity entity)
        {

            if (entity == null)
            {
                throw new Exception(message: "The entity field is required!");
            }

            var existingCTGoiTiemChung = await _context.cTGoiTiemChungEntities
                .FirstOrDefaultAsync(c => c.MaVaccine == maVaccine && c.MaGoiTiemChung == maGoiTiemChung && c.DeletedTime == null);

            if (existingCTGoiTiemChung == null)
            {
                throw new Exception(message: "Không tìm thấy chi tiết gói tiêm chung!");
            }

            _context.Entry(existingCTGoiTiemChung).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
