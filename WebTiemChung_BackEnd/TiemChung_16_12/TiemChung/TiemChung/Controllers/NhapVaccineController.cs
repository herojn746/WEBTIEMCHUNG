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
    public class NhapVaccineController : ControllerBase
    {
        private readonly INhapVaccineRepository _nhapVaccineRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public NhapVaccineController(INhapVaccineRepository NhapVaccineRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _nhapVaccineRepository = NhapVaccineRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-nhap-vaccine-by-id")]
        public async Task<ActionResult<NhapVaccineEntity>> GetNhapVaccineById(string id)
        {
            try
            {
                var entity = await _nhapVaccineRepository.GetNhapVaccineById(id);

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
        [Route("/api/[controller]/get-all-nhap-vaccine")]
        public async Task<ActionResult<IEnumerable<NhapVaccineEntity>>> GetAllNhapVaccine()
        {
            try
            {
                var entity = await _nhapVaccineRepository.GetAllNhapVaccine();

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
        [Route("/api/[controller]/search-nhap-vaccine")]
        public async Task<ActionResult<IEnumerable<NhapVaccineEntity>>> SearchNhapVaccine(string searchKey)
        {
            try
            {
                var NhapVaccineList = await _nhapVaccineRepository.SearchNhapVaccine(searchKey);
                if (!NhapVaccineList.Any())
                {
                    return BadRequest("Not found!");
                }
                return Ok(NhapVaccineList);
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
        [Route("/api/[controller]/create-nhap-vaccine")]
        public async Task<ActionResult<string>> CreateNhapVaccine(NhapVaccineModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<NhapVaccineEntity>(model);
                mapEntity.MaNhanVien = userId;
                var result = await _nhapVaccineRepository.CreateNhapVaccine(mapEntity);

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
        [Route("/api/[controller]/update-nhap-vaccine")]
        public async Task<ActionResult> UpdateNhapVaccine(string id, NhapVaccineModel entity)
        {
            try
            {

                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<NhapVaccineEntity>(entity);
                mapEntity.MaNhanVien = userId;
                await _nhapVaccineRepository.UpdateNhapVaccine(id, mapEntity);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Nhập vaccine updated successfully!"));
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
        [Route("/api/[controller]/delete-nhap-vaccine")]
        public async Task<ActionResult<NhapVaccineEntity>> DeleteNhapVaccine(string keyId)
        {

            try
            {

                await _nhapVaccineRepository.DeleteNhapVaccine(keyId, false);
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
