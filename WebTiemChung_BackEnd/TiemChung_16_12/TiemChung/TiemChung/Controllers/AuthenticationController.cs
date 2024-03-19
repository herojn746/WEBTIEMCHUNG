using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TiemChung.Entity;
using TiemChung.Model;
using TiemChung.Repository;
using TiemChung.Repository.Interface;

namespace TiemChung.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IKhachHangRepository _khachHangRepository;
        private readonly INhanVienRepository _nhanVienRepository;
        private readonly AppSetting _appSettings;
        private readonly AppDbContext _context;

        public AuthenticationController(IKhachHangRepository khachHangRepository, INhanVienRepository nhanVienRepository, IOptions<AppSetting> appSettings, AppDbContext Context)
        {
            _khachHangRepository = khachHangRepository;
            _nhanVienRepository = nhanVienRepository;
            _appSettings = appSettings.Value;
            _context = Context;
        }

        [HttpPost]
        [Route("/api/[controller]/login")]
        public async Task<IActionResult> Login(LoginModel model, [FromServices] IDistributedCache distributedCache)
        {
            try
            {
                var result = await _khachHangRepository.Login(model);

                if (result.GetType() == typeof(NhanVienEntity))
                {
                    // If the result is a NhanVienEntity object, return the token for the employee
                    var nhanVien = (NhanVienEntity)result;
                    var token = GenerateToken(nhanVien);

                    string id = nhanVien.Id;
                    byte[] userIdBytes = Encoding.UTF8.GetBytes(id);
                    await distributedCache.SetAsync("UserId", userIdBytes);
 
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Authenticate success",
                        Data = token
                    });
                }
                else
                {
                    // If the result is a KhachHangEntity object, return the token for the customer
                    var khachHang = (KhachHangEntity)result;
                    var token = GenerateToken(khachHang);
                    //save id
                    string id = khachHang.Id;
                    byte[] userIdBytes = Encoding.UTF8.GetBytes(id);
                    await distributedCache.SetAsync("UserId", userIdBytes);
 

                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Authenticate success",
                        Data = token
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        private string GenerateToken(KhachHangEntity nguoiDung)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, nguoiDung.TenKhachHang),
                new Claim(ClaimTypes.Role, nguoiDung.Role),
                new Claim("Id", nguoiDung.Id.ToString()),
                new Claim("TokenId", Guid.NewGuid().ToString())
            }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }

        //private async Task<TokenModel> GenerateToken(KhachHangEntity nguoiDung)
        //{
        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
        //    var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

        //    var tokenDescription = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //        {
        //        new Claim(ClaimTypes.Name, nguoiDung.TenKhachHang),
        //        new Claim(ClaimTypes.Role, nguoiDung.Role),
        //        new Claim("Id", nguoiDung.Id.ToString()),


        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    }),
        //        Expires = DateTime.UtcNow.AddMinutes(10),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
        //    };

        //    var token = jwtTokenHandler.CreateToken(tokenDescription);
        //    var accessToken = jwtTokenHandler.WriteToken(token);
        //    var refreshToken = GenerateRefreshToken();

        //    //return jwtTokenHandler.WriteToken(token);
        //    var refreshTokenEntity = new RefreshTokenEntity
        //    {
        //        Id = Guid.NewGuid(),
        //        JwtId = token.Id,
        //        UserId = nguoiDung.Id,
        //        Token = refreshToken,
        //        IsUsed = false,
        //        IsRevoked = false,
        //        IssuedAt = DateTime.UtcNow,
        //        ExpiredAt = DateTime.UtcNow.AddHours(1),
        //    };
        //    await _context.AddAsync(refreshTokenEntity);
        //    await _context.SaveChangesAsync();

        //    return new TokenModel
        //    {
        //        AccessToken = accessToken,
        //        RefreshToken = refreshToken,
        //    };

        //}



        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        private string GenerateToken(NhanVienEntity nguoiDung)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, nguoiDung.TenNhanVien),
                    new Claim(ClaimTypes.Role, nguoiDung.Role),
                    new Claim("Id", nguoiDung.Id.ToString()),
                    new Claim("TokenId", Guid.NewGuid().ToString())
            }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }



        [HttpPost]
        [Route("/api/[controller]/change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            try
            {
                await _khachHangRepository.ChangePassword(changePasswordRequest);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status201Created,
                    code: "Success!",
                    data: "Password changed successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status400BadRequest,
                    code: "Error",
                    message: ex.Message));
            }
        }

        private List<string> _tokenBlacklist = new List<string>();

        [HttpPost]
        [Route("/api/[controller]/logout")]
        public async Task<IActionResult> LogoutAsync([FromServices] IDistributedCache distributedCache)
        {
            byte[] userIdBytes = await distributedCache.GetAsync("UserId");
            if (userIdBytes != null)
            {
                string userId = Encoding.UTF8.GetString(userIdBytes);

                // Xóa giá trị UserId từ Distributed Cache
                await distributedCache.RemoveAsync("UserId");
                // Lấy mã token hiện tại từ header Authorization
                var currentToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Thêm mã token vào danh sách blacklist
                _tokenBlacklist.Add(currentToken);
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Logout successful"
            });
        }
    }
}

