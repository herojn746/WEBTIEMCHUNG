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
    public class GoiTiemChungController : ControllerBase
    {
        private readonly IGoiTiemChungRepository _goiTiemChung;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public GoiTiemChungController(IGoiTiemChungRepository GoiTiemChungRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _goiTiemChung = GoiTiemChungRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-goi-tiem-chung-by-id")]
        public async Task<ActionResult<GoiTiemChungEntity>> GetGoiTiemChungById(string id)
        {
            try
            {
                var entity = await _goiTiemChung.GetGoiTiemChungById(id);

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
        [Route("/api/[controller]/get-all-goi-tiem-chung")]
        public async Task<ActionResult<IEnumerable<GoiTiemChungEntity>>> GetAllGoiTiemChung()
        {
            try
            {
                var entity = await _goiTiemChung.GetAllGoiTiemChung();

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
        [Route("/api/[controller]/get-all-ma-loai-tiem-chung-by-id")]
        public async Task<ActionResult<IEnumerable<GoiTiemChungEntity>>> GetAllMaLoaiTiemChung(string id)
        {
            try
            {
                var entity = await _goiTiemChung.GetMaLoaiTiemChungById(id);

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
        [Route("/api/[controller]/search-goi-tiem-chung")]
        public async Task<ActionResult<IEnumerable<GoiTiemChungEntity>>> SearchGoiTiemChung(string searchKey)
        {
            try
            {
                var GoiTiemChungList = await _goiTiemChung.SearchGoiTiemChung(searchKey);
                if (!GoiTiemChungList.Any())
                {
                    return BadRequest("Not found!");
                }
                return Ok(GoiTiemChungList);
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
        [Route("/api/[controller]/create-goi-tiem-chung")]
        public async Task<ActionResult<string>> CreateGoiTiemChung(GoiTiemChungModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var entity = _mapper.Map<GoiTiemChungEntity>(model);
                entity.CreateBy = userId;
                var result = await _goiTiemChung.CreateGoiTiemChung(entity);

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
        [Route("/api/[controller]/update-goi-tiem-chung")]
        public async Task<ActionResult> UpdateGoiTiemChung(string id, GoiTiemChungModel entity)
        {
            try
            {

                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity=_mapper.Map<GoiTiemChungEntity>(entity);
                mapEntity.CreateBy = userId;
                await _goiTiemChung.UpdateGoiTiemChung(id, mapEntity);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Chi tiết loại tiêm chủng cập nhật thành công!"));
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
        [Route("/api/[controller]/delete-goi-tiem-chung")]
        public async Task<ActionResult<GoiTiemChungEntity>> DeleteGoiTiemChung(string keyId)
        {

            try
            {

                await _goiTiemChung.DeleteGoiTiemChung(keyId, false);
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
