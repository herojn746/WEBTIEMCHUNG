using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TiemChung.Entity;
using TiemChung.Model;
  
using TiemChung.Repository.Interface;

using System.Security.Cryptography;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;

namespace TiemChung.Repository
{
    public class KhachHangRepository : IKhachHangRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly AppSetting _appSettings;
        private readonly IDistributedCache _distributedCache;
        public KhachHangRepository(AppDbContext Context, IMapper mapper, IOptions<AppSetting> appSettings, IDistributedCache distributedCache)
        {
            _context = Context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _distributedCache = distributedCache;
        }

        
        public async Task<string> CreateKhachHang(KhachHangModel entity)
        {
            var existingKhachHang = await _context.khachHangEntities
                .FirstOrDefaultAsync(c => c.Id == entity.Id && (c.DeletedTime == null || c.DeletedTime != null));

            if (existingKhachHang != null)
            {
                throw new Exception(message: "Account name is already exist!");
            }

            var mapEntity = _mapper.Map<KhachHangEntity>(entity);
            mapEntity.Password = EncryptPassword(entity.Password);

            await _context.khachHangEntities.AddAsync(mapEntity);
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

        //public async Task<KhachHangEntity> Login(LoginModel entity)
        //{
        //    try
        //    {
        //        var user = _context.khachHangEntities.SingleOrDefault(p => p.Id == entity.Username);
        //        if (user is null)
        //        {
        //            throw new Exception(message: "Login unsuccessful!");
        //        }

        //        // Mã hóa mật khẩu nhập vào và so sánh với mật khẩu đã được mã hóa trong cơ sở dữ liệu
        //        var encryptedPassword = EncryptPassword(entity.Password);
        //        var storedPassword = Convert.FromBase64String(user.Password);

        //        if (!CompareEncryptedPasswords(entity.Password, storedPassword))
        //        {
        //            throw new Exception(message: "Login unsuccessful!");
        //        }

        //        return user;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<dynamic> Login(LoginModel entity)
        {
            try
            {
                var nhanVien = await _context.nhanVienEntities.SingleOrDefaultAsync(p => p.Id == entity.Username);
                var khachHang = await _context.khachHangEntities.SingleOrDefaultAsync(p => p.Id == entity.Username);

                if (nhanVien is not null)
                {
                    var encryptedPassword = EncryptPassword(entity.Password);
                    var storedPassword = Convert.FromBase64String(nhanVien.Password);

                    if (!CompareEncryptedPasswords(entity.Password, storedPassword))
                    {
                        throw new Exception(message: "Login unsuccessful!");
                    }


                    return nhanVien;
                }
                else if (khachHang is not null)
                {
                    var encryptedPassword = EncryptPassword(entity.Password);
                    var storedPassword = Convert.FromBase64String(khachHang.Password);

                    if (!CompareEncryptedPasswords(entity.Password, storedPassword))
                    {
                        throw new Exception("Login unsuccessful!");
                    }

                     
                    return  khachHang ;
                }
                else
                {
                    throw new Exception("Login unsuccessful!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error during login: " + ex.Message);
            }
        }

        //===================================
        public async Task<dynamic> ChangePassword(ChangePasswordRequest entity)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var nhanVien = await _context.nhanVienEntities.SingleOrDefaultAsync(p => p.Id == userId);
                var khachHang = await _context.khachHangEntities.SingleOrDefaultAsync(p => p.Id == userId);

                if (nhanVien is not null)
                {
                    var encryptedPassword = EncryptPassword(entity.CurrentPassword);
                    var storedPassword = Convert.FromBase64String(nhanVien.Password);

                    if (!CompareEncryptedPasswords(entity.CurrentPassword, storedPassword))
                    {
                        throw new Exception(message: "Current password is incorrect!");
                    }

                    var encryptedNewPassword = EncryptPassword(entity.NewPassword);
                    nhanVien.Password = encryptedNewPassword;

                   return await _context.SaveChangesAsync();
                    
                }
                else if (khachHang is not null)
                {
                    var encryptedPassword = EncryptPassword(entity.CurrentPassword);
                    var storedPassword = Convert.FromBase64String(khachHang.Password);

                    if (!CompareEncryptedPasswords(entity.CurrentPassword, storedPassword))
                    {
                        throw new Exception("Current password is incorrect");
                    }


                    var encryptedNewPassword = EncryptPassword(entity.NewPassword);
                    khachHang.Password = encryptedNewPassword;

                   return await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Change password unsuccessful!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error during password: " + ex.Message);
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
        //    var user = await _context.khachHangEntities.FirstOrDefaultAsync(c => c.Id == entity.Username && c.DeletedTime == null);

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
        public async  Task<KhachHangEntity> DeleteKhachHang(string id, bool isPhysical)
        {
            try
            {
                var entity = await _context.khachHangEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
                if (entity == null)
                {
                    throw new Exception("Customer not found.");
                }
                else
                {
                    if (isPhysical)
                    {
                        _context.khachHangEntities.Remove(entity);
                    }
                    else
                    {
                        entity.DeletedTime = DateTimeOffset.Now;
                        _context.khachHangEntities.Update(entity);
                    }

                    await _context.SaveChangesAsync();

                }
                return  entity;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<ICollection<KhachHangEntity>> GetAllKhachHang()
        {
            var entities = await _context.khachHangEntities
                .Where(c => c.DeletedTime == null)
                .ToListAsync();

            return entities;
        }
        public async Task<ICollection<KhachHangEntity>> SearchKhachHang(string searchKey)
        {
            var ListKH = await _context.khachHangEntities.ToListAsync();

            // Filter the list and materialize the results
            var filteredList = ListKH.Where(c => (
                c.Id.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                c.TenKhachHang.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                c.SDT.Contains(searchKey, StringComparison.OrdinalIgnoreCase)
            ) && c.DeletedTime == null).ToList();

            return filteredList;
        }


        public async  Task<KhachHangEntity> GetKhachHangById(string id)
        {
            var entity= await _context.khachHangEntities.FirstOrDefaultAsync(e => e.Id == id && e.DeletedTime == null);
            if(entity is null)
            {
                throw new Exception("not found or already deleted.");
            }
            return entity;
        }

        public async Task UpdateKhachHang(string id, KhachHangModel entity)
        {
            var existingKhachHang = await _context.khachHangEntities
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedTime == null);

            if (existingKhachHang == null)
            {
                throw new Exception(message: "Account not found!");
            }

            existingKhachHang.SDT = entity.SDT;
            existingKhachHang.CMND = entity.CMND;
            existingKhachHang.TenKhachHang = entity.TenKhachHang;
            
            _context.khachHangEntities.Update(existingKhachHang);
            await _context.SaveChangesAsync();
        }
        //public async Task Logout()
        //{
        //    // Find the user in the database
        //    var user = await _context.khachHangEntities.FindAsync(userId);

        //    if (user == null)
        //    {
        //        return NotFound(new ApiResponse
        //        {
        //            Success = false,
        //            Message = "User not found"
        //        });
        //    }

        //    // Invalidate the user's token by setting its expiration date to the past
        //    user.TokenExpiration = DateTime.UtcNow.AddMinutes(-1);
        //    await _context.SaveChangesAsync();

        //}
    }
}
