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
    public class VaccineController : ControllerBase
    {
        private readonly IVaccineRepository _vaccine;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public VaccineController(IVaccineRepository VaccineRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _vaccine = VaccineRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-vaccine-by-id")]
        public async Task<ActionResult<VaccineEntity>> GetVaccineById(string id)
        {
            try
            {
                var entity = await _vaccine.GetVaccineById(id);

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
        [Route("/api/[controller]/get-all-vaccine")]
        public async Task<ActionResult<IEnumerable<VaccineEntity>>> GetAllVaccine()
        {
            try
            {
                var entity = await _vaccine.GetAllVaccine();

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
        [Route("/api/[controller]/search-vaccine")]
        public async Task<ActionResult<IEnumerable<VaccineEntity>>> SearchVaccine(string searchKey)
        {
            try
            {
                var VaccineList = await _vaccine.SearchVaccine(searchKey);
                if (!VaccineList.Any())
                {
                    return BadRequest("Not found!");
                }
                return Ok(VaccineList);
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
        [Route("/api/[controller]/create-vaccine")]
        public async Task<ActionResult<string>> CreateVaccine(VaccineModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var entity = _mapper.Map<VaccineEntity>(model);
                entity.CreateBy = userId;
                var result = await _vaccine.CreateVaccine(entity);

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
        [Route("/api/[controller]/update-vaccine")]
        public async Task<ActionResult> UpdateVaccine(string id, VaccineModel entity)
        {
            try
            {

                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<VaccineEntity>(entity);
                mapEntity.CreateBy = userId;
                await _vaccine.UpdateVaccine(id, mapEntity);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Vaccine cập nhật thành công!"));
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
        [Route("/api/[controller]/delete-vaccine")]
        public async Task<ActionResult<VaccineEntity>> DeleteVaccine(string keyId)
        {

            try
            {

                await _vaccine.DeleteVaccine(keyId, false);
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
