using AutoMapper;
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
    public class HoGiaDinhController : ControllerBase
    {
        private readonly IHoGiaDinhRepository _hoGiaDinh;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public HoGiaDinhController(IHoGiaDinhRepository HoGiaDinhRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _hoGiaDinh = HoGiaDinhRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-ho-gia-dinh-by-id")]
        public async Task<ActionResult<HoGiaDinhEntity>> GetHoGiaDinhById(string id)
        {
            try
            {
                var entity = await _hoGiaDinh.GetHoGiaDinhById(id);

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
        [Route("/api/[controller]/get-all-ho-gia-dinh")]
        public async Task<ActionResult<IEnumerable<HoGiaDinhEntity>>> GetAllHoGiaDinh()
        {
            try
            {
                var entity = await _hoGiaDinh.GetAllHoGiaDinh();

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
        [Route("/api/[controller]/search-ho-gia-dinh")]
        public async Task<ActionResult<IEnumerable<HoGiaDinhEntity>>> SearchHoGiaDinh(string searchKey)
        {
            try
            {
                var HoGiaDinhList = await _hoGiaDinh.SearchHoGiaDinh(searchKey);
                if (!HoGiaDinhList.Any())
                {
                    return BadRequest("Not found!");
                }
                return Ok(HoGiaDinhList);
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
        [Route("/api/[controller]/create-ho-gia-dinh")]
        public async Task<ActionResult<string>> CreateHoGiaDinh(HoGiaDinhModel model)
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

                var entity = _mapper.Map<HoGiaDinhEntity>(model);
                entity.CreateBy = userId;
                var result = await _hoGiaDinh.CreateHoGiaDinh(entity);

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
        [Route("/api/[controller]/update-ho-gia-dinh")]
        public async Task<ActionResult> UpdateHoGiaDinh(string id, HoGiaDinhModel entity)
        {
            try
            {

                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<HoGiaDinhEntity>(entity);
                mapEntity.CreateBy = userId;
                await _hoGiaDinh.UpdateHoGiaDinh(id, mapEntity);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Update successfully!"));
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
        [Route("/api/[controller]/delete-ho-gia-dinh")]
        public async Task<ActionResult<HoGiaDinhEntity>> DeleteHoGiaDinh(string keyId)
        {

            try
            {

                await _hoGiaDinh.DeleteHoGiaDinh(keyId, false);
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
