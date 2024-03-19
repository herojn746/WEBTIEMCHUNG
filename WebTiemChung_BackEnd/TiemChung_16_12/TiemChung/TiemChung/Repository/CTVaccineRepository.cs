using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiemChung.Entity;
using TiemChung.Repository.Interface;

namespace TiemChung.Repository
{
    public class CTVaccineRepository:ICTVaccineRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CTVaccineRepository(AppDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;

        }
        public async Task<string> CreateCTVaccine(CTVaccineEntity entity)
        {
            var existingCTVaccine = await _context.cTVaccineEntities
              .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null|| c.DeletedTime != null));

            if (existingCTVaccine != null)
            {
                throw new Exception(message: "Id is already exist!");
            }

            var mapEntity = _mapper.Map<CTVaccineEntity>(entity);

            await _context.cTVaccineEntities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return mapEntity.Id;
        }

        public async Task<CTVaccineEntity> DeleteCTVaccine(string id, bool isPhysical)
        {
            try
            {
                var entity = await _context.cTVaccineEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy Id nhập chi tiết vaccine!");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.cTVaccineEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.cTVaccineEntities.Update(entity);
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

        public async Task<ICollection<CTVaccineEntity>> GetAllCTVaccine()
        {
            try
            {
                var entities = await _context.cTVaccineEntities
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

        public async Task<CTVaccineEntity> GetCTVaccineById(string id)
        {
            var entity = await _context.cTVaccineEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

        public async Task<ICollection<CTVaccineEntity>> SearchCTVaccine(string searchKey)
        {
            var ListKH = await _context.cTVaccineEntities.ToListAsync();

            // Filter the list and materialize the results
            var filteredList = ListKH.Where(c => (
                c.Id.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                c.DoTuoi.ToString().Contains(searchKey, StringComparison.OrdinalIgnoreCase)||
                c.MoTa.Contains(searchKey, StringComparison.OrdinalIgnoreCase)
            ) && c.DeletedTime == null).ToList();

            return filteredList;
        }

        public async Task UpdateCTVaccine(string id, CTVaccineEntity entity)
        {

            if (entity == null)
            {
                throw new Exception(message: "The entity field is required!");
            }

            var existingCTVaccine = await _context.cTVaccineEntities
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

            if (existingCTVaccine == null)
            {
                throw new Exception(message: "Không tìm thấy chi tiết vaccine!");
            }

            _context.Entry(existingCTVaccine).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
