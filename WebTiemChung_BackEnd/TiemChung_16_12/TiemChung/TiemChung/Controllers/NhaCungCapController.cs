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
    public class NhaCungCapController : ControllerBase
    {
        private readonly INhaCungCapRepository _nhaCungCapRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public NhaCungCapController(INhaCungCapRepository NhaCungCapRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _nhaCungCapRepository = NhaCungCapRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-nha-cung-cap-by-id")]
        public async Task<ActionResult<NhaCungCapEntity>> GetNhaCungCapById(string id)
        {
            try
            {
                var entity = await _nhaCungCapRepository.GetNhaCungCapById(id);

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
        [Route("/api/[controller]/get-all-nha-cung-cap")]
        public async Task<ActionResult<IEnumerable<NhaCungCapEntity>>> GetAllNhaCungCap()
        {
            try
            {
                var entity = await _nhaCungCapRepository.GetAllNhaCungCap();

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
        [Route("/api/[controller]/search-nha-cung-cap")]
        public async Task<ActionResult<IEnumerable<NhaCungCapEntity>>> SearchNhaCungCap(string searchKey)
        {
            try
            {
                var NhaCungCapList = await _nhaCungCapRepository.SearchNhaCungCap(searchKey);
                if (!NhaCungCapList.Any())
                {
                    return BadRequest("Not found!");
                }
                return Ok(NhaCungCapList);
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
        [Route("/api/[controller]/create-nha-cung-cap")]
        public async Task<ActionResult<string>> CreateNhaCungCap(NhaCungCapModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<NhaCungCapEntity>(model);
                mapEntity.CreateBy = userId;
                var result = await _nhaCungCapRepository.CreateNhaCungCap(mapEntity);

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
        [Route("/api/[controller]/update-nha-cung-cap")]
        public async Task<ActionResult> UpdateNhaCungCap(string id, NhaCungCapModel entity)
        {
            try
            {

                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<NhaCungCapEntity>(entity);
                mapEntity.CreateBy = userId;
                await _nhaCungCapRepository.UpdateNhaCungCap(id, mapEntity);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Nhà cung cấp updated successfully!"));
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
        [Route("/api/[controller]/delete-nha-cung-cap")]
        public async Task<ActionResult<NhaCungCapEntity>> DeleteNhaCungCap(string keyId)
        {

            try
            {

                await _nhaCungCapRepository.DeleteNhaCungCap(keyId, false);
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
