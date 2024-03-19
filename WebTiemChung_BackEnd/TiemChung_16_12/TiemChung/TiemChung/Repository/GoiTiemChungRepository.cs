using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiemChung.Entity;
using TiemChung.Model;
using TiemChung.Repository.Interface;

namespace TiemChung.Repository
{
    public class GoiTiemChungRepository:IGoiTiemChungRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public GoiTiemChungRepository(AppDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;

        }
        public async Task<string> CreateGoiTiemChung(GoiTiemChungEntity entity)
        {
            var existingGoiTiemChung = await _context.goiTiemChungEntities
                .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingGoiTiemChung != null)
            {
                throw new Exception(message: "Id already exists!");
            }

            //var giaVaccine = await _context.vaccineEntities
            //    .FirstOrDefaultAsync(c => c.Id == entity.MaVaccine && c.DeletedTime == null);

            //if (giaVaccine == null)
            //{
            //    throw new Exception(message: "Invalid Vaccine Id!");
            //}

            var mapEntity = _mapper.Map<GoiTiemChungEntity>(entity);
            //float giaGiam = giaVaccine.GiaTien * (mapEntity.GiamGia > 0 ? mapEntity.GiamGia : 0);
            //mapEntity.TongTien = giaVaccine.GiaTien - giaGiam;

            await _context.goiTiemChungEntities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return mapEntity.Id;
        }

        public async Task<GoiTiemChungEntity> DeleteGoiTiemChung(string id, bool isPhysical)
        {
            try
            {
                var entity = await _context.goiTiemChungEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy chi tiết loại tiêm chủng.");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.goiTiemChungEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.goiTiemChungEntities.Update(entity);
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

        public async Task<ICollection<GoiTiemChungEntity>> GetAllGoiTiemChung()
        {
            try
            {
                var entities = await _context.goiTiemChungEntities
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
        //=================================================================
        //public async Task<ICollection<GoiTiemChungEntity>> GetAllGoiTiemChungKhachHang()
        //{
        //    try
        //    {
        //        var entities = await _context.goiTiemChungEntities
        //             .Where(c => c.DeletedTime == null)
        //             .ToListAsync();

        //        if (entities is null)
        //        {
        //            throw new Exception("Empty list!");
        //        }
        //        return entities;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        //=================================================================
        public async Task<GoiTiemChungEntity> GetGoiTiemChungById(string id)
        {
            var entity = await _context.goiTiemChungEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }
        public async Task<ICollection<GoiTiemChungEntity>> GetMaLoaiTiemChungById(string id)
        {
            var entities = await _context.goiTiemChungEntities
                     .Where(c => c.MaLoaiTiemChung == id && c.DeletedTime == null)
                     .ToListAsync();
            if (entities is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entities;
        }
        public async Task<ICollection<GoiTiemChungEntity>> SearchGoiTiemChung(string searchKey)
        {
            var ListKH = await _context.goiTiemChungEntities.ToListAsync();

            // Filter the list and materialize the results
            var filteredList = ListKH.Where(c => (
                c.Id.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                c.MoTa.Contains(searchKey, StringComparison.OrdinalIgnoreCase)
                
            ) && c.DeletedTime == null).ToList();

            return filteredList;
        }

        public async Task UpdateGoiTiemChung(string id, GoiTiemChungEntity entity)
        {

            if (entity == null)
            {
                throw new Exception(message: "The entity field is required!");
            }

            var existingGoiTiemChung = await _context.goiTiemChungEntities
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

            if (existingGoiTiemChung == null)
            {
                throw new Exception(message: "Không tìm thấy loại tiêm chủng!");
            }

            _context.Entry(existingGoiTiemChung).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

        }
    }
}
