using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiemChung.Entity;
using TiemChung.Repository.Interface;

namespace TiemChung.Repository
{
    public class HoGiaDinhRepository:IHoGiaDinhRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public HoGiaDinhRepository(AppDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;

        }
        public async Task<string> CreateHoGiaDinh(HoGiaDinhEntity entity)
        {
            var existingHoGiaDinh = await _context.hoGiaDinhEntities
              .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingHoGiaDinh != null)
            {
                throw new Exception(message: "Id is already exist!");
            }

            var mapEntity = _mapper.Map<HoGiaDinhEntity>(entity);




            await _context.hoGiaDinhEntities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return mapEntity.Id;
        }

        public async Task<HoGiaDinhEntity> DeleteHoGiaDinh(string id, bool isPhysical)
        {
            try
            {
                var entity = await _context.hoGiaDinhEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy hộ gia đình.");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.hoGiaDinhEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.hoGiaDinhEntities.Update(entity);
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

        public async Task<ICollection<HoGiaDinhEntity>> GetAllHoGiaDinh()
        {
            try
            {
                var entities = await _context.hoGiaDinhEntities
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

        public async Task<HoGiaDinhEntity> GetHoGiaDinhById(string id)
        {
            var entity = await _context.hoGiaDinhEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

        public async Task<ICollection<HoGiaDinhEntity>> SearchHoGiaDinh(string searchKey)
        {
            var ListKH = await _context.hoGiaDinhEntities.ToListAsync();


            var filteredList = ListKH.Where(c => (
                c.Id.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                c.TenChuHo.Contains(searchKey, StringComparison.OrdinalIgnoreCase)

            ) && c.DeletedTime == null).ToList();

            return filteredList;
        }

        public async Task UpdateHoGiaDinh(string id, HoGiaDinhEntity entity)
        {

            if (entity == null)
            {
                throw new Exception(message: "The entity field is required!");
            }

            var existingHoGiaDinh = await _context.hoGiaDinhEntities
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

            if (existingHoGiaDinh == null)
            {
                throw new Exception(message: "Không tìm thấy hộ gia đình!");
            }

            _context.Entry(existingHoGiaDinh).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

        }
    }
}
