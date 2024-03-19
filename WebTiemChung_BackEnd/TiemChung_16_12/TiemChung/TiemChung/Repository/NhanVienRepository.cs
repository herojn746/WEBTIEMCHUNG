using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TiemChung.Entity;
using TiemChung.Model;
using TiemChung.Repository.Interface;

namespace TiemChung.Repository
{
    public class NhanVienRepository : INhanVienRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public NhanVienRepository(AppDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;

        }
        public async Task<string> CreateNhanVien(NhanVienModel entity)
        {
            var existingNhanVien = await _context.nhanVienEntities
                .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingNhanVien != null)
            {
                throw new Exception(message: "Account name is already exist!");
            }

            var mapEntity = _mapper.Map<NhanVienEntity>(entity);
            mapEntity.Password = EncryptPassword(entity.Password);

            await _context.nhanVienEntities.AddAsync(mapEntity);
            await _context.SaveChangesAsync();

            return mapEntity.Id;
        }


        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                // Generate a random salt
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                // Create a new instance of the bcrypt hashing algorithm
                var bcrypt = new Rfc2898DeriveBytes(password, salt, 10000);

                // Generate the hash value
                byte[] hash = bcrypt.GetBytes(20);

                // Combine the salt and hash into a single string
                byte[] storedPassword = new byte[36];
                Array.Copy(salt, 0, storedPassword, 0, 16);
                Array.Copy(hash, 0, storedPassword, 16, 20);

                string encryptedPassword = Convert.ToBase64String(storedPassword);
                return encryptedPassword;
            }
        }

        public async Task<NhanVienEntity> Login(LoginModel entity)
        {
            try
            {
                var user = _context.nhanVienEntities.SingleOrDefault(p => p.Id == entity.Username);
                if (user is null)
                {
                    throw new Exception(message: "Login unsuccessful!");
                }

                // Mã hóa mật khẩu nhập vào và so sánh với mật khẩu đã được mã hóa trong cơ sở dữ liệu
                var encryptedPassword = EncryptPassword(entity.Password);
                var storedPassword = Convert.FromBase64String(user.Password);

                if (!CompareEncryptedPasswords(entity.Password, storedPassword))
                {
                    throw new Exception(message: "Login unsuccessful!");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool CompareEncryptedPasswords(string password, byte[] storedPassword)
        {
            var salt = new byte[16];
            Array.Copy(storedPassword, 0, salt, 0, 16);

            var bcrypt = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = bcrypt.GetBytes(20);

            return storedPassword.Skip(16).SequenceEqual(hash);
        }

        //public async Task ChangePassword(ChangePasswordRequest entity)
        //{
        //    var user = await _context.nhanVienEntities.FirstOrDefaultAsync(c => c.Id == entity.Username && c.DeletedTime == null);

        //    if (user == null)
        //    {
        //        throw new Exception(message: "User not found!");
        //    }

        //    // Mã hóa mật khẩu hiện tại và so sánh với mật khẩu đã được mã hóa trong cơ sở dữ liệu
        //    var encryptedCurrentPassword = EncryptPassword(entity.CurrentPassword);
        //    var storedPassword = Convert.FromBase64String(user.Password);

        //    if (!CompareEncryptedPasswords(entity.CurrentPassword, storedPassword))
        //    {
        //        throw new Exception(message: "Current password is incorrect!");
        //    }

        //    // Mã hóa mật khẩu mới và cập nhật vào cơ sở dữ liệu
        //    var encryptedNewPassword = EncryptPassword(entity.NewPassword);
        //    user.Password = encryptedNewPassword;

        //    await _context.SaveChangesAsync();
        //}
        public async Task<NhanVienEntity> DeleteNhanVien(string id, bool isPhysical)
        {
            try
            {
                var entity = await _context.nhanVienEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Customer not found.");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.nhanVienEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.nhanVienEntities.Update(entity);
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



        public async Task<ICollection<NhanVienEntity>> GetAllNhanVien()
        {
            var entities = await _context.nhanVienEntities
                .Where(c => c.DeletedTime == null)
                .ToListAsync();

            return entities;
        }
        public async Task<ICollection<NhanVienEntity>> SearchNhanVien(string searchKey)
        {
            var ListKH = await _context.nhanVienEntities.ToListAsync();

            // Filter the list and materialize the results
            var filteredList = ListKH.Where(c => (
                c.Id.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                c.TenNhanVien.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                c.SDT.Contains(searchKey, StringComparison.OrdinalIgnoreCase)
            ) && c.DeletedTime == null).ToList();

            return filteredList;
        }


        public async Task<NhanVienEntity> GetNhanVienById(string id)
        {
            var entity = await _context.nhanVienEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
            if (entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

        //public async Task UpdateNhanVien(string id, NhanVienModel entity)
        //{
        //    var existingNhanVien = await _context.nhanVienEntities
        //        .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

        //    if (existingNhanVien == null)
        //    {
        //        throw new Exception(message: "Account not found!");
        //    }

        //    existingNhanVien.SDT = entity.SDT;
        //    existingNhanVien.CMND = entity.CMND;
        //    existingNhanVien.TenNhanVien = entity.TenNhanVien;
        //    existingNhanVien.ChucVu = entity.ChucVu.ToString();
        //    _context.nhanVienEntities.Update(existingNhanVien);
        //    await _context.SaveChangesAsync();
        //}
        //public async Task UpdateNhanVien(string id, NhanVienModel entity)
        //{
        //    var existingNhanVien = await _context.nhanVienEntities
        //        .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

        //    if (existingNhanVien == null)
        //    {
        //        throw new Exception(message: "Account not found!");
        //    }

        //    existingNhanVien.SDT = entity.SDT;
        //    existingNhanVien.CMND = entity.CMND;
        //    existingNhanVien.TenNhanVien = entity.TenNhanVien;

        //    // Kiểm tra giá trị của ChucVuEnum
        //    if (entity.ChucVu == ChucVuEnum.NhanVien)
        //    {
        //        existingNhanVien.ChucVu = entity.ChucVu.ToString();
        //    }
        //    else
        //    {
        //        throw new Exception(message: "Invalid ChucVu value!");
        //    }

        //    _context.nhanVienEntities.Update(existingNhanVien);
        //    await _context.SaveChangesAsync();
        //}
        public async Task UpdateNhanVien(string id, NhanVienModel entity)
        {
            // Check if the entity field is provided
            if (entity == null)
            {
                throw new Exception(message: "The entity field is required!");
            }
            var existingNhanVien = await _context.nhanVienEntities
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

            if (existingNhanVien == null)
            {
                throw new Exception(message: "Account not found!");
            }

            existingNhanVien.SDT = entity.SDT;
            existingNhanVien.CMND = entity.CMND;
            existingNhanVien.TenNhanVien = entity.TenNhanVien;
            existingNhanVien.Role = entity.Role;

            _context.nhanVienEntities.Update(existingNhanVien);
            await _context.SaveChangesAsync();
        }

    }
}
