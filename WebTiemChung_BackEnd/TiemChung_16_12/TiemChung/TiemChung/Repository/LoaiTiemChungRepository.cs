using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiemChung.Entity;
using TiemChung.Model;
using TiemChung.Repository.Interface;

namespace TiemChung.Repository
{
    public class LoaiTiemChungRepository : ILoaiTiemChungRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public LoaiTiemChungRepository(AppDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;

        }
        public async Task<string> CreateLoaiTiemChung(LoaiTiemChungEntity entity)
        {
            var existingLoaiTiemChung = await _context.loaiTiemChungEntities
              .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingLoaiTiemChung != null)
            {
                throw new Exception(message: "Id is already exist!");
            }

            var mapEntity = _mapper.Map<LoaiTiemChungEntity>(entity);
            //LoaiTiemChungEntity mapEntity=new LoaiTiemChungEntity()
            //{
            //    Id = entity.Id,
            //    TenLoaiTiem=entity.TenLoaiTiem,
            //    MoTa=entity.MoTa,
            //    GiaTien=entity.GiaTien,
            //    TrangThai=entity.TrangThai,
            //    MaNhanVien=entity.MaNhanVien,
            //};



            await _context.loaiTiemChungEntities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return mapEntity.Id;
        }

        public async Task<LoaiTiemChungEntity> DeleteLoaiTiemChung(string id, bool isPhysical)
        {
            try
            {
                var entity = await _context.loaiTiemChungEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Loai tiem chung not found.");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.loaiTiemChungEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.loaiTiemChungEntities.Update(entity);
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

        public async Task<ICollection<LoaiTiemChungEntity>> GetAllLoaiTiemChung()
        {
            try
            { 
                var entities = await _context.loaiTiemChungEntities
                     .Where(c => c.DeletedTime == null)
                     .ToListAsync();

                if (entities is null)
                {
                    throw new Exception("Empty list!");
                }
                return entities;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoaiTiemChungEntity> GetLoaiTiemChungById(string id)
        {
            var entity = await _context.loaiTiemChungEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

        public async Task<ICollection<LoaiTiemChungEntity>> SearchLoaiTiemChung(string searchKey)
        {
            var ListKH = await _context.loaiTiemChungEntities.ToListAsync();

            // Filter the list and materialize the results
            var filteredList = ListKH.Where(c => (
                c.Id.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                c.TenLoaiTiem.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                c.GiaTien.ToString().Contains(searchKey, StringComparison.OrdinalIgnoreCase)
            ) && c.DeletedTime == null).ToList();

            return filteredList;
        }

        public async Task UpdateLoaiTiemChung(string id, LoaiTiemChungEntity entity)
        {
             
            if (entity == null)
            {
                throw new Exception(message: "The entity field is required!");
            }

            var existingLoaiTiemChung = await _context.loaiTiemChungEntities
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

            if (existingLoaiTiemChung == null)
            {
                throw new Exception(message: "Loai tiem chung not found!");
            }

            existingLoaiTiemChung.TenLoaiTiem = entity.TenLoaiTiem;
            existingLoaiTiemChung.MoTa = entity.MoTa;
            existingLoaiTiemChung.GiaTien = entity.GiaTien;
            existingLoaiTiemChung.TrangThai = entity.TrangThai;

            _context.loaiTiemChungEntities.Update(existingLoaiTiemChung);
            await _context.SaveChangesAsync();
        }
    }
}
