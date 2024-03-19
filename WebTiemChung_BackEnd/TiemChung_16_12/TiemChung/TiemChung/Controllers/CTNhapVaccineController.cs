using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using TiemChung.Entity;
using TiemChung.Model;
using TiemChung.Repository;
using TiemChung.Repository.Interface;

namespace TiemChung.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CTNhapVaccineController : ControllerBase
    {
        private readonly ICTNhapVaccineRepository _cTNhapVaccineRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public CTNhapVaccineController(ICTNhapVaccineRepository CTNhapVaccineRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _cTNhapVaccineRepository = CTNhapVaccineRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-ct-nhap-vaccine-by-id")]
        public async Task<ActionResult<CTNhapVaccineEntity>> GetCTNhapVaccineById(string maVaccine, string maNhapVaccine)
        {
            try
            {
                var entity = await _cTNhapVaccineRepository.GetCTNhapVaccineById(maVaccine,maNhapVaccine);

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
        [Route("/api/[controller]/get-all-ct-nhap-vaccine")]
        public async Task<ActionResult<IEnumerable<CTNhapVaccineEntity>>> GetAllCTNhapVaccine()
        {
            try
            {
                var entity = await _cTNhapVaccineRepository.GetAllCTNhapVaccine();

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

         

        [HttpPost]
        [Route("/api/[controller]/create-ct-nhap-vaccine")]
        public async Task<ActionResult<string>> CreateCTNhapVaccine(CTNhapVaccineModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<CTNhapVaccineEntity>(model);
                mapEntity.CreateBy = userId;
                var result = await _cTNhapVaccineRepository.CreateCTNhapVaccine(mapEntity);

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
        [Route("/api/[controller]/update-ct-nhap-vaccine")]
        public async Task<ActionResult> UpdateCTNhapVaccine(string maVaccine, string maNhapVaccine, CTNhapVaccineModel entity)
        {
            try
            {

                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<CTNhapVaccineEntity>(entity);
                mapEntity.CreateBy = userId;
                await _cTNhapVaccineRepository.UpdateCTNhapVaccine(maVaccine,maNhapVaccine, mapEntity);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Chi tiết nhập vaccine updated successfully!"));
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
        [Route("/api/[controller]/delete-ct-nhap-vaccine")]
        public async Task<ActionResult<CTNhapVaccineEntity>> DeleteCTNhapVaccine(string maVaccine, string maNhapVaccine)
        {

            try
            {

                await _cTNhapVaccineRepository.DeleteCTNhapVaccine(maVaccine,maNhapVaccine, false);
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
        [HttpGet]
        [Route("/api/[controller]/get-ct-nhap-vaccine-report")]
        public async Task<ActionResult<Dictionary<string, int>>> GenerateCTXuatVaccineReport(DateTime startTime, DateTime endTime)
        {
            try
            {
                var report = _cTNhapVaccineRepository.GenerateNhapVaccineReport(startTime, endTime);
                return Ok(report);
            }
            catch (Exception ex)
            {
                var errorResponse = new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status500InternalServerError,
                    code: "Failed!",
                    message: ex.Message);

                return BadRequest(errorResponse);
            }
        }
    }
}
