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
    public class CTVaccineController : ControllerBase
    {
        private readonly ICTVaccineRepository _cTVaccine;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public CTVaccineController(ICTVaccineRepository CTVaccineRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _cTVaccine = CTVaccineRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-ct-vaccine-by-id")]
        public async Task<ActionResult<CTVaccineEntity>> GetCTVaccineById(string id)
        {
            try
            {
                var entity = await _cTVaccine.GetCTVaccineById(id);

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
        [Route("/api/[controller]/get-all-ct-vaccine")]
        public async Task<ActionResult<IEnumerable<CTVaccineEntity>>> GetAllCTVaccine()
        {
            try
            {
                var entity = await _cTVaccine.GetAllCTVaccine();

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
        [Route("/api/[controller]/search-ct-vaccine")]
        public async Task<ActionResult<IEnumerable<CTVaccineEntity>>> SearchCTVaccine(string searchKey)
        {
            try
            {
                var CTVaccineList = await _cTVaccine.SearchCTVaccine(searchKey);
                if (!CTVaccineList.Any())
                {
                    return BadRequest("Not found!");
                }
                return Ok(CTVaccineList);
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
        [Route("/api/[controller]/create-ct-vaccine")]
        public async Task<ActionResult<string>> CreateCTVaccine(CTVaccineModel model)
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

                var entity = _mapper.Map<CTVaccineEntity>(model);
                entity.CreateBy = userId;
                var result = await _cTVaccine.CreateCTVaccine(entity);

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
        [Route("/api/[controller]/update-ct-vaccine")]
        public async Task<ActionResult> UpdateCTVaccine(string id, CTVaccineModel entity)
        {
            try
            {

                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<CTVaccineEntity>(entity);

                mapEntity.CreateBy = userId;

                await _cTVaccine.UpdateCTVaccine(id, mapEntity);

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
        [Route("/api/[controller]/delete-ct-vaccine")]
        public async Task<ActionResult<CTVaccineEntity>> DeleteCTVaccine(string keyId)
        {

            try
            {

                await _cTVaccine.DeleteCTVaccine(keyId, false);
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
