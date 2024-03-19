using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiemChung.Entity;
using TiemChung.Repository.Interface;

namespace TiemChung.Repository
{
    public class XuatVaccineRepository:IXuatVaccineRepository 
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public XuatVaccineRepository(AppDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;

        }

        public async Task<string> CreateXuatVaccine(XuatVaccineEntity entity)
        {
            var existingXuatVaccine = await _context.xuatVaccineEntities
                .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingXuatVaccine != null)
            {
                throw new Exception(message: "XuatVaccine with the provided Id already exists!");
            }

            var existingLoaiXuatVaccine = await _context.nhanVienEntities
                .FirstOrDefaultAsync(c => c.Id == entity.MaNhanVien && c.DeletedTime == null);

            if (existingLoaiXuatVaccine == null)
            {
                throw new Exception(message: "Mã loại XuatVaccine không tồn tại hoặc đã bị xóa!");
            }

            var mapEntity = _mapper.Map<XuatVaccineEntity>(entity);
            await _context.xuatVaccineEntities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return mapEntity.Id;
        }
        public async Task<XuatVaccineEntity> DeleteXuatVaccine(string id, bool isPhysical)
        {
            try
            {
                var entity = await _context.xuatVaccineEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Không tìm thấy XuatVaccine.");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.xuatVaccineEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.xuatVaccineEntities.Update(entity);
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

        public async Task<ICollection<XuatVaccineEntity>> GetAllXuatVaccine()
        {
            try
            {
                var entities = await _context.xuatVaccineEntities
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

        public async Task<XuatVaccineEntity> GetXuatVaccineById(string id)
        {
            var entity = await _context.xuatVaccineEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

        //public async Task<ICollection<XuatVaccineEntity>> SearchXuatVaccine(string searchKey)
        //{
        //    var ListKH = await _context.xuatVaccineEntities.ToListAsync();

        //    // Filter the list and materialize the results
        //    var filteredList = ListKH.Where(c => (
        //        c.Id.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
        //        c.TenXuatVaccine.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
        //        c.NhaSanXuat.Contains(searchKey, StringComparison.OrdinalIgnoreCase)

        //    ) && c.DeletedTime == null).ToList();

        //    return filteredList;
        //}

        public async Task UpdateXuatVaccine(string id, XuatVaccineEntity entity)
        {

            if (entity == null)
            {
                throw new Exception(message: "The entity field is required!");
            }

            var existingXuatVaccine = await _context.xuatVaccineEntities
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

            if (existingXuatVaccine == null)
            {
                throw new Exception(message: "Không tìm thấy xuat Vaccine!");
            }

            _context.Entry(existingXuatVaccine).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

        }

    }
}
