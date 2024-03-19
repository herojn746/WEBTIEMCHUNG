using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiemChung.Entity;
using TiemChung.Repository.Interface;
namespace TiemChung.Repository
{
    public class NhapVaccineRepository:INhapVaccineRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public NhapVaccineRepository(AppDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;

        }
        public async Task<string> CreateNhapVaccine(NhapVaccineEntity entity)
        {
            var existingNhapVaccine = await _context.nhapVaccineEntities
              .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingNhapVaccine != null)
            {
                throw new Exception(message: "Id is already exist!");
            }

            var mapEntity = _mapper.Map<NhapVaccineEntity>(entity);

            await _context.nhapVaccineEntities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return mapEntity.Id;
        }

        public async Task<NhapVaccineEntity> DeleteNhapVaccine(string id, bool isPhysical)
        {
            try
            {
                var entity = await _context.nhapVaccineEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy Id nhập vaccine!");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.nhapVaccineEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.nhapVaccineEntities.Update(entity);
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

        public async Task<ICollection<NhapVaccineEntity>> GetAllNhapVaccine()
        {
            try
            {
                var entities = await _context.nhapVaccineEntities
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

        public async Task<NhapVaccineEntity> GetNhapVaccineById(string id)
        {
            var entity = await _context.nhapVaccineEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

        public async Task<ICollection<NhapVaccineEntity>> SearchNhapVaccine(string searchKey)
        {
            var ListKH = await _context.nhapVaccineEntities.ToListAsync();

            // Filter the list and materialize the results
            var filteredList = ListKH.Where(c => (
                c.Id.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                 
                c.NgayTao.ToString().Contains(searchKey, StringComparison.OrdinalIgnoreCase)
            ) && c.DeletedTime == null).ToList();

            return filteredList;
        }

        public async Task UpdateNhapVaccine(string id, NhapVaccineEntity entity)
        {

            if (entity == null)
            {
                throw new Exception(message: "The entity field is required!");
            }

            var existingNhapVaccine = await _context.nhapVaccineEntities
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

            if (existingNhapVaccine == null)
            {
                throw new Exception(message: "Không tìm thấy nhập vaccine!");
            }

            _context.Entry(existingNhapVaccine).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
