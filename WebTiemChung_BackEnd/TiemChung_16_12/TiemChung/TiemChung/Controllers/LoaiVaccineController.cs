using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using TiemChung.Entity;
using TiemChung.Model;
using TiemChung.Repository.Interface;

namespace TiemChung.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "NhanVien,QuanLy")]
    public class LoaiVaccineController : ControllerBase
    {
        private readonly ILoaiVaccineRepository _loaiVaccine;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public LoaiVaccineController(ILoaiVaccineRepository LoaiVaccineRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _loaiVaccine = LoaiVaccineRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-loai-vaccine-by-id")]
        public async Task<ActionResult<LoaiVaccineEntity>> GetLoaiVaccineById(string id)
        {
            try
            {
                var entity = await _loaiVaccine.GetLoaiVaccineById(id);

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
        [Route("/api/[controller]/get-all-loai-vaccine")]
        public async Task<ActionResult<IEnumerable<LoaiVaccineEntity>>> GetAllLoaiVaccine()
        {
            try
            {
                var entity = await _loaiVaccine.GetAllLoaiVaccine();

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
        [Route("/api/[controller]/search-loai-vaccine")]
        public async Task<ActionResult<IEnumerable<LoaiVaccineEntity>>> SearchLoaiVaccine(string searchKey)
        {
            try
            {
                var LoaiVaccineList = await _loaiVaccine.SearchLoaiVaccine(searchKey);
                if (!LoaiVaccineList.Any())
                {
                    return BadRequest("Not found!");
                }
                return Ok(LoaiVaccineList);
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
        [HttpPost]
        [Route("/api/[controller]/create-loai-vaccine")]
        public async Task<ActionResult<string>> CreateLoaiVaccine(LoaiVaccineModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);
                
                if (string.IsNullOrEmpty(userId))
                {
                    var errorMessage = "Lỗi đăng nhập!";
                    return Ok(new BaseResponseModel<string>(
                   statusCode: StatusCodes.Status400BadRequest,
                   code: "Failed!",
                   data: errorMessage));
                }
 
                var entity = _mapper.Map<LoaiVaccineEntity>(model);
                entity.CreateBy = userId;
                var result = await _loaiVaccine.CreateLoaiVaccine(entity);

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
        [HttpPut]
        [Route("/api/[controller]/update-loai-vaccine")]
        public async Task<ActionResult> UpdateLoaiVaccine(string id, LoaiVaccineModel entity)
        {
            try
            {

                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<LoaiVaccineEntity>(entity);
                mapEntity.CreateBy = userId;
                await _loaiVaccine.UpdateLoaiVaccine(id, mapEntity);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Loại Vaccine cập nhật thành công!"));
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
        [Route("/api/[controller]/delete-loai-vaccine")]
        public async Task<ActionResult<LoaiVaccineEntity>> DeleteLoaiVaccine(string keyId)
        {

            try
            {

                await _loaiVaccine.DeleteLoaiVaccine(keyId, false);
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
    }
}
