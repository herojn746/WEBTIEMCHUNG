using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiemChung.Entity;
using TiemChung.Repository.Interface;

namespace TiemChung.Repository
{
    public class VaccineRepository:IVaccineRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public VaccineRepository(AppDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;

        }
        
        public async Task<string> CreateVaccine(VaccineEntity entity)
        {
            var existingVaccine = await _context.vaccineEntities
                .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingVaccine != null)
            {
                throw new Exception(message: "Vaccine with the provided Id already exists!");
            }

            var existingLoaiVaccine = await _context.loaiVaccineEntities
                .FirstOrDefaultAsync(c => c.Id == entity.MaLoaiVaccine && c.DeletedTime == null);

            if (existingLoaiVaccine == null)
            {
                throw new Exception(message: "Mã loại Vaccine không tồn tại hoặc đã bị xóa!");
            }

            var mapEntity = _mapper.Map<VaccineEntity>(entity);
            await _context.vaccineEntities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return mapEntity.Id;
        }
        public async Task<VaccineEntity> DeleteVaccine(string id, bool isPhysical)
        {
            try
            {
                var entity = await _context.vaccineEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy Vaccine.");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.vaccineEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.vaccineEntities.Update(entity);
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

        public async Task<ICollection<VaccineEntity>> GetAllVaccine()
        {
            try
            {
                var entities = await _context.vaccineEntities
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

        public async Task<VaccineEntity> GetVaccineById(string id)
        {
            var entity = await _context.vaccineEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

        public async Task<ICollection<VaccineEntity>> SearchVaccine(string searchKey)
        {
            var ListKH = await _context.vaccineEntities.ToListAsync();

            // Filter the list and materialize the results
            var filteredList = ListKH.Where(c => (
                c.Id.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                c.TenVaccine.Contains(searchKey, StringComparison.OrdinalIgnoreCase)||
                c.NhaSanXuat.Contains(searchKey, StringComparison.OrdinalIgnoreCase)

            ) && c.DeletedTime == null).ToList();

            return filteredList;
        }

        public async Task UpdateVaccine(string id, VaccineEntity entity)
        {

            if (entity == null)
            {
                throw new Exception(message: "The entity field is required!");
            }

            var existingVaccine = await _context.vaccineEntities
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

            if (existingVaccine == null)
            {
                throw new Exception(message: "Không tìm thấy Vaccine!");
            }

            _context.Entry(existingVaccine).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

        }
    }
}
