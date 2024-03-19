using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection.PortableExecutable;
using System.Text;
using TiemChung.Entity;
using TiemChung.Model;
using TiemChung.Repository.Interface;

namespace TiemChung.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CTGoiTiemChungController : ControllerBase
    {
        private readonly ICTGoiTiemChungRepository _cTGoiTiemChungRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public CTGoiTiemChungController(ICTGoiTiemChungRepository CTGoiTiemChungRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _cTGoiTiemChungRepository = CTGoiTiemChungRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-ct-goi-tiem-chung-by-id")]
        public async Task<ActionResult<CTGoiTiemChungEntity>> GetCTGoiTiemChungById(string maGoiTiemChung, string maVaccine)
        {
            try
            {
                var entity = await _cTGoiTiemChungRepository.GetCTGoiTiemChungById(maGoiTiemChung,maVaccine);

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
        //[HttpGet]
        //[Route("/api/[controller]/get-ten-vaccine-by-ma-goi-tiem")]
        //public async Task<ActionResult<List<VaccineEntity>>> GetTenVaccineByMaGoiTiemChung(string keyId)
        //{
        //    try
        //    {
        //        var entity = await _cTGoiTiemChungRepository.GetTenVaccineByMaGoiTiemChung(keyId);

        //        return Ok(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        dynamic result = new BaseResponseModel<string>(
        //           statusCode: StatusCodes.Status500InternalServerError,
        //           code: "Failed!",
        //           message: ex.Message);
        //        return BadRequest(result);
        //    }
        //}
        [HttpGet]
        [Route("/api/[controller]/get-chi-tiet-tiem-chung-by-ma-goi-tiem")]
        public async Task<ActionResult<List<CTGoiTiemChungEntity>>> GetTenVaccineByMaGoiTiemChung(string keyId)
        {
            try
            {
                var vaccines = await _cTGoiTiemChungRepository.GetVaccinesByMaGoiTiemChung(keyId);

                return Ok(vaccines);
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
        [Route("/api/[controller]/get-all-ct-goi-tiem-chung")]
        public async Task<ActionResult<IEnumerable<CTGoiTiemChungEntity>>> GetAllCTGoiTiemChung()
        {
            try
            {
                var entity = await _cTGoiTiemChungRepository.GetAllCTGoiTiemChung();

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
        [Route("/api/[controller]/create-ct-goi-tiem-chung")]
        public async Task<ActionResult<string>> CreateCTGoiTiemChung(CTGoiTiemChungModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<CTGoiTiemChungEntity>(model);
                mapEntity.CreateBy = userId;
                var result = await _cTGoiTiemChungRepository.CreateCTGoiTiemChung(mapEntity);

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
        [Route("/api/[controller]/update-ct-goi-tiem-chung")]
        public async Task<ActionResult> UpdateCTGoiTiemChung(string maGoiTiemChung, string maVaccine, CTGoiTiemChungModel entity)
        {
            try
            {

                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<CTGoiTiemChungEntity>(entity);
                mapEntity.CreateBy = userId;
                await _cTGoiTiemChungRepository.UpdateCTGoiTiemChung(maGoiTiemChung, maVaccine, mapEntity);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Updated successfully!"));
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
        [Route("/api/[controller]/delete-ct-goi-tiem-chung")]
        public async Task<ActionResult<CTGoiTiemChungEntity>> DeleteCTGoiTiemChung(string maGoiTiemChung, string maVaccine)
        {

            try
            {

                await _cTGoiTiemChungRepository.DeleteCTGoiTiemChung(maGoiTiemChung, maVaccine, false);
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
