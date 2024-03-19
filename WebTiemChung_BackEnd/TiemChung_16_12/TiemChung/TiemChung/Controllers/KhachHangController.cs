using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TiemChung.Entity;
using TiemChung.Model;
using TiemChung.Repository.Interface;
 
 

namespace TiemChung.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "KhachHang")]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangRepository _khachHangRepository;
        private readonly IMapper _mapper;
        private readonly AppSetting _appSettings;
         
        public KhachHangController(IKhachHangRepository khachHangRepository, IMapper mapper, IOptionsMonitor<AppSetting> optionsMonitor )
        {
            _khachHangRepository = khachHangRepository;
            _mapper = mapper;
            _appSettings = optionsMonitor.CurrentValue;


             
        }
        

        [HttpGet]
        [Route("/api/[controller]/get-khach-hang-by-id")]
        public async Task<ActionResult <KhachHangEntity>> GetKhachHangById(string id)
        {
            try
            {
                var entity = await _khachHangRepository.GetKhachHangById(id);

                return Ok(entity);
            }
            catch (Exception ex)
            {
                dynamic result = new BaseResponseModel<string>(
                   statusCode: StatusCodes.Status500InternalServerError,
                   code: "Failed!",
                   message: ex.Message);
                return BadRequest(result);
            }
        }
        //[Authorize(Roles = "KhachHang")]
        [HttpGet]
        [Route("/api/[controller]/get-all-khach-hang")]
        public async Task<ActionResult<IEnumerable<KhachHangEntity>>> GetAllKhachHang()
        {
            try
            {
                var entity = await _khachHangRepository.GetAllKhachHang();

                return Ok(entity);
            }
            catch (Exception ex)
            {
                dynamic result = new BaseResponseModel<string>(
                   statusCode: StatusCodes.Status500InternalServerError,
                   code: "Failed!",
                   message: ex.Message);
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("/api/[controller]/search-khach-hang")]
        public async Task<ActionResult<IEnumerable<KhachHangEntity>>> SearchKhachHang(string searchKey)
        {
            try
            {
                var khachHangList = await _khachHangRepository.SearchKhachHang(searchKey);
                if (!khachHangList.Any())
                {
                    return BadRequest("Not found!");
                }
                return Ok(khachHangList);
            }
            catch (Exception ex)
            {
                dynamic result = new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status500InternalServerError,
                    code: "Failed!",
                    message: ex.Message);
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("/api/[controller]/create-khach-hang")]
        public async Task<ActionResult<NhanVienEntity>> CreateKhachHang(KhachHangModel model)
        {
            try
            {

                //var entity = _mapper.Map<KhachHangEntity>(model);

                var result = await _khachHangRepository.CreateKhachHang(model);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status201Created,
                    code: "Success!",
                    data: result));
            }
            catch (Exception ex)
            {
                dynamic result;
                result = new BaseResponseModel<string>(
                   statusCode: StatusCodes.Status500InternalServerError,
                   code: "Failed!",
                   message: ex.Message);
                return BadRequest(result);
            }
        }
        //[HttpPost]
        //[Route("/api/[controller]/change-password")]
        //public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
        //{
        //    try
        //    {
        //        await _khachHangRepository.ChangePassword(changePasswordRequest);

        //        return Ok(new BaseResponseModel<string>(
        //            statusCode: StatusCodes.Status201Created,
        //            code: "Success!",
        //            data: "Password changed successfully!"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new BaseResponseModel<string>(
        //            statusCode: StatusCodes.Status400BadRequest,
        //            code: "Error",
        //            message: ex.Message));
        //    }
        //}
        //[HttpPost]
        //[Route("/api/[controller]/login")]
        //public async Task<IActionResult> Login(LoginModel model)
        //{
        //    try
        //    {
        //        var result = await _khachHangRepository.Login(model);

        //        if (result.GetType() == typeof(NhanVienEntity))
        //        {
        //            // If the result is a NhanVienEntity object, return the token for the employee
        //            var nhanVien = (NhanVienEntity)result;
        //            var token = GenerateToken(nhanVien);

        //            return Ok(new ApiResponse
        //            {
        //                Success = true,
        //                Message = "Authenticate success",
        //                Data = token
        //            });
        //        }
        //        else
        //        {
        //            // If the result is a KhachHangEntity object, return the token for the customer
        //            var khachHang = (KhachHangEntity)result;
        //            var token = GenerateToken(khachHang);

        //            return Ok(new ApiResponse
        //            {
        //                Success = true,
        //                Message = "Authenticate success",
        //                Data = token
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiResponse
        //        {
        //            Success = false,
        //            Message = ex.Message
        //        });
        //    }
        //}
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
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
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
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }

         
        [HttpPut]
        [Route("/api/[controller]/update-khach-hang")]
        public async Task<ActionResult> UpdateKhachHang(string id, KhachHangUpdate entity)
        {
            try
            {
                var khachHang = new KhachHangModel
                {
                    TenKhachHang = entity.TenKhachHang,
                    SDT = entity.SDT,
                    CMND = entity.CMND,
                     
                };

                await _khachHangRepository.UpdateKhachHang(id, khachHang);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Khach hang updated successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status400BadRequest,
                    code: "Error",
                    message: ex.Message));
            }
        }
        [HttpDelete]
        [Route("/api/[controller]/delete-khach-hang")]
        public async Task<ActionResult<KhachHangEntity>> DeleteKhachHang(string keyId)
        {
             
            try
            {
                  
                await  _khachHangRepository.DeleteKhachHang(keyId,false);
                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Delete successfully!"));
            }
            catch (Exception ex)
            {
                
                return BadRequest(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status400BadRequest,
                    code: "Error",
                    message: ex.Message));
            }
        }
        //[HttpPost]
        //[Route("/api/[controller]/logout")]
        //public async Task<ActionResult> Logout()
        //{
        //    try
        //    {
        //        // Get the token from the request header
        //        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        //        // Add the token to the blacklist
        //        await _tokenRepository.AddTokenToBlacklist(token);

        //        // Sign out the user
        //        await HttpContext.SignOutAsync();

        //        return Ok(new ApiResponse
        //        {
        //            Success = true,
        //            Message = "Logout success"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        dynamic result = new BaseResponseModel<string>(
        //            statusCode: StatusCodes.Status500InternalServerError,
        //            code: "Failed!",
        //            message: ex.Message
        //        );

        //        return BadRequest(result);
        //    }
        //}
        //private List<string> _tokenBlacklist = new List<string>();

        //[HttpPost]
        //[Route("/api/[controller]/logout")]
        //public IActionResult Logout()
        //{
        //    // Lấy mã token hiện tại từ header Authorization
        //    var currentToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        //    // Thêm mã token vào danh sách blacklist
        //    _tokenBlacklist.Add(currentToken);

        //    return Ok(new ApiResponse
        //    {
        //        Success = true,
        //        Message = "Logout successful"
        //    });
        //}


    }
}
