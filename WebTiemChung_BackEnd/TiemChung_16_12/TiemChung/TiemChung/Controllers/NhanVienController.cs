using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TiemChung.Entity;
using TiemChung.Model;
using TiemChung.Repository;
using TiemChung.Repository.Interface;

namespace TiemChung.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly INhanVienRepository _NhanVienRepository;
        private readonly IMapper _mapper;
        private readonly AppSetting _appSettings;

        public NhanVienController(INhanVienRepository NhanVienRepository, IMapper mapper, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _NhanVienRepository = NhanVienRepository;
            _mapper = mapper;
            _appSettings = optionsMonitor.CurrentValue;



        }
        [HttpGet]
        [Route("/api/[controller]/get-nhan-vien-by-id")]
        public async Task<ActionResult<NhanVienEntity>> GetNhanVienById(string id)
        {
            try
            {
                var entity = await _NhanVienRepository.GetNhanVienById(id);

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
        //[Authorize(Roles = "NhanVien")]
        [HttpGet]
        [Route("/api/[controller]/get-all-nhan-vien")]
        public async Task<ActionResult<IEnumerable<NhanVienEntity>>> GetAllNhanVien()
        {
            try
            {
                var entity = await _NhanVienRepository.GetAllNhanVien();

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
        [Route("/api/[controller]/search-nhan-vien")]
        public async Task<ActionResult<IEnumerable<NhanVienEntity>>> SearchNhanVien(string searchKey)
        {
            try
            {
                var NhanVienList = await _NhanVienRepository.SearchNhanVien(searchKey);
                if (!NhanVienList.Any())
                {
                    return BadRequest("Not found!");
                }
                return Ok(NhanVienList);
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
        [Route("/api/[controller]/create-nhan-vien")]
        public async Task<ActionResult<NhanVienEntity>> CreateNhanVien(NhanVienModel model)
        {
            try
            {

                //var entity = _mapper.Map<NhanVienEntity>(model);

                var result = await _NhanVienRepository.CreateNhanVien(model);

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
        //        await _NhanVienRepository.ChangePassword(changePasswordRequest);

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
        //public async Task<ActionResult<NhanVienEntity>> Login(LoginModel model)
        //{
        //    try
        //    {
        //        NhanVienEntity user = await _NhanVienRepository.Login(model);



        //        if (user == null)
        //        {
        //            return Ok(new ApiResponse
        //            {
        //                Success = false,
        //                Message = "Invalid username/password"
        //            });
        //        }

        //        return Ok(new ApiResponse
        //        {
        //            Success = true,
        //            Message = "Authenticate success",
        //            Data = GenerateToken(user)
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

        private string GenerateToken(NhanVienEntity nguoiDung)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, nguoiDung.TenNhanVien),
                    new Claim(ClaimTypes.Role, nguoiDung.Role),
                    new Claim("Id", nguoiDung.Id.ToString()),

                    //roles

                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }
        
        [HttpPut]
        [Route("/api/[controller]/update-nhan-vien")]
        public async Task<ActionResult> UpdateNhanVien(string id, NhanVienUpdate entity)
        {
            try
            {
                var NhanVien = new NhanVienModel
                {
                    TenNhanVien = entity.TenNhanVien,
                    SDT = entity.SDT,
                    CMND = entity.CMND,
                    Role = entity.Role
                };

                await _NhanVienRepository.UpdateNhanVien(id, NhanVien);

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
        [Route("/api/[controller]/delete-nhan-vien")]
        public async Task<ActionResult<NhanVienEntity>> DeleteNhanVien(string keyId)
        {

            try
            {

                await _NhanVienRepository.DeleteNhanVien(keyId, false);
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
