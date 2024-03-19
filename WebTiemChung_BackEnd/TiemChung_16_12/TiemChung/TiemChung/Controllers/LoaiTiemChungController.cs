using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text;
using TiemChung.Entity;
using TiemChung.Model;
using TiemChung.Repository;
using TiemChung.Repository.Interface;

namespace TiemChung.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiTiemChungController : ControllerBase
    {
        private readonly ILoaiTiemChungRepository _LoaiTiemChungRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public LoaiTiemChungController(ILoaiTiemChungRepository LoaiTiemChungRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _LoaiTiemChungRepository = LoaiTiemChungRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-loai-tiem-chung-by-id")]
        public async Task<ActionResult<LoaiTiemChungEntity>> GetLoaiTiemChungById(string id)
        {
            try
            {
                var entity = await _LoaiTiemChungRepository.GetLoaiTiemChungById(id);

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
        [Route("/api/[controller]/get-all-loai-tiem-chung")]
        public async Task<ActionResult<IEnumerable<LoaiTiemChungEntity>>> GetAllLoaiTiemChung()
        {
            try
            {
                var entity = await _LoaiTiemChungRepository.GetAllLoaiTiemChung();

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
        [Route("/api/[controller]/search-loai-tiem-chung")]
        public async Task<ActionResult<IEnumerable<LoaiTiemChungEntity>>> SearchLoaiTiemChung(string searchKey)
        {
            try
            {
                var LoaiTiemChungList = await _LoaiTiemChungRepository.SearchLoaiTiemChung(searchKey);
                if (!LoaiTiemChungList.Any())
                {
                    return BadRequest("Not found!");
                }
                return Ok(LoaiTiemChungList);
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
        [Route("/api/[controller]/create-loai-tiem-chung")]
        public async Task<ActionResult<string>> CreateLoaiTiemChung(LoaiTiemChungModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var entity = _mapper.Map<LoaiTiemChungEntity>(model);
                entity.MaNhanVien=userId;
                var result = await _LoaiTiemChungRepository.CreateLoaiTiemChung(entity);

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
        [Route("/api/[controller]/update-loai-tiem-chung")]
        public async Task<ActionResult> UpdateLoaiTiemChung(string id, LoaiTiemChungModel model)
        {
            try
            {
                 
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var entity = _mapper.Map<LoaiTiemChungEntity>(model);
                entity.MaNhanVien = userId;
                await _LoaiTiemChungRepository.UpdateLoaiTiemChung(id, entity);

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
        [Route("/api/[controller]/delete-loai-tiem-chung")]
        public async Task<ActionResult<LoaiTiemChungEntity>> DeleteLoaiTiemChung(string keyId)
        {

            try
            {

                await _LoaiTiemChungRepository.DeleteLoaiTiemChung(keyId, false);
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
