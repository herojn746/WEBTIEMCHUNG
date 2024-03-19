using AutoMapper;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using TiemChung.Entity;
using TiemChung.Model;
using TiemChung.Repository.Interface;

//
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace TiemChung.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CTXuatVaccineController : ControllerBase
    {
        private readonly ICTXuatVaccineRepository _cTXuatVaccineRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public CTXuatVaccineController(ICTXuatVaccineRepository CTXuatVaccineRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _cTXuatVaccineRepository = CTXuatVaccineRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-ct-xuat-vaccine-by-id")]
        public async Task<ActionResult<CTXuatVaccineEntity>> GetCTXuatVaccineById(string maVaccine, string maXuatVaccine)
        {
            try
            {
                var entity = await _cTXuatVaccineRepository.GetCTXuatVaccineById(maVaccine,maXuatVaccine);

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
        [Route("/api/[controller]/get-all-ct-xuat-vaccine")]
        public async Task<ActionResult<IEnumerable<CTXuatVaccineEntity>>> GetAllCTXuatVaccine()
        {
            try
            {
                var entity = await _cTXuatVaccineRepository.GetAllCTXuatVaccine();

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
        [Route("/api/[controller]/get-ct-xuat-vaccine-report")]
        public async Task<ActionResult<Dictionary<string, int>>> GenerateCTXuatVaccineReport(DateTime startTime, DateTime endTime)
        {
            try
            {
                var report = _cTXuatVaccineRepository.GenerateXuatVaccineReport(startTime, endTime);
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

        //[HttpGet]
        //[Route("/api/[controller]/search-ct-xuat-vaccine")]
        //public async Task<ActionResult<IEnumerable<CTXuatVaccineEntity>>> SearchCTXuatVaccine(string searchKey)
        //{
        //    try
        //    {
        //        var CTXuatVaccineList = await _cTXuatVaccineRepository.SearchCTXuatVaccine(searchKey);
        //        if (!CTXuatVaccineList.Any())
        //        {
        //            return BadRequest("Not found!");
        //        }
        //        return Ok(CTXuatVaccineList);
        //    }
        //    catch (Exception ex)
        //    {
        //        dynamic result = new BaseResponseModel<string>(
        //            statusCode: StatusCodes.Status500InternalServerError,
        //            code: "Failed!",
        //            message: ex.Message);
        //        return BadRequest(result);
        //    }
        //}

        [HttpPost]
        [Route("/api/[controller]/create-ct-xuat-vaccine")]
        public async Task<ActionResult<string>> CreateCTXuatVaccine(CTXuatVaccineModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<CTXuatVaccineEntity>(model);
                mapEntity.CreateBy = userId;
                var result = await _cTXuatVaccineRepository.CreateCTXuatVaccine(mapEntity);
                 
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
        [Route("/api/[controller]/update-ct-xuat-vaccine")]
        public async Task<ActionResult> UpdateCTXuatVaccine(string maVaccine, string maXuatVaccine, CTXuatVaccineModel entity)
        {
            try
            {

                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<CTXuatVaccineEntity>(entity);
                mapEntity.CreateBy = userId;
                await _cTXuatVaccineRepository.UpdateCTXuatVaccine(maVaccine,maXuatVaccine, mapEntity);

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
        [Route("/api/[controller]/delete-ct-xuat-vaccine")]
        public async Task<ActionResult<CTXuatVaccineEntity>> DeleteCTXuatVaccine(string maVaccine, string maXuatVaccine)
        {

            try
            {

                await _cTXuatVaccineRepository.DeleteCTXuatVaccine(maVaccine,maXuatVaccine, false);
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
        //=============================================
          

    }

}
