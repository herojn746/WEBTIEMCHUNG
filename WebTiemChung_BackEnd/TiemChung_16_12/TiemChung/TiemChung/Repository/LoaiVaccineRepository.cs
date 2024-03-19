using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiemChung.Entity;
using TiemChung.Repository.Interface;

namespace TiemChung.Repository
{
    public class LoaiVaccineRepository:ILoaiVaccineRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public LoaiVaccineRepository(AppDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;

        }
        public async Task<string> CreateLoaiVaccine(LoaiVaccineEntity entity)
        {
            var existingLoaiVaccine = await _context.loaiVaccineEntities
              .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingLoaiVaccine != null)
            {
                throw new Exception(message: "Id is already exist!");
            }

            var mapEntity = _mapper.Map<LoaiVaccineEntity>(entity);




            await _context.loaiVaccineEntities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return mapEntity.Id;
        }

        public async Task<LoaiVaccineEntity> DeleteLoaiVaccine(string id, bool isPhysical)
        {
            try
            {
                var entity = await _context.loaiVaccineEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy loai Vaccine.");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.loaiVaccineEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.loaiVaccineEntities.Update(entity);
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

        public async Task<ICollection<LoaiVaccineEntity>> GetAllLoaiVaccine()
        {
            try
            {
                var entities = await _context.loaiVaccineEntities
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

        public async Task<LoaiVaccineEntity> GetLoaiVaccineById(string id)
        {
            var entity = await _context.loaiVaccineEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

        public async Task<ICollection<LoaiVaccineEntity>> SearchLoaiVaccine(string searchKey)
        {
            var ListKH = await _context.loaiVaccineEntities.ToListAsync();

             
            var filteredList = ListKH.Where(c => (
                c.Id.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                c.TenLoai.Contains(searchKey, StringComparison.OrdinalIgnoreCase)

            ) && c.DeletedTime == null).ToList();

            return filteredList;
        }

        public async Task UpdateLoaiVaccine(string id, LoaiVaccineEntity entity)
        {

            if (entity == null)
            {
                throw new Exception(message: "The entity field is required!");
            }

            var existingLoaiVaccine = await _context.loaiVaccineEntities
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

            if (existingLoaiVaccine == null)
            {
                throw new Exception(message: "Không tìm thấy loại Vaccine!");
            }

            _context.Entry(existingLoaiVaccine).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

        }
    }
}
