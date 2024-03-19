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
    public class XuatVaccineController : ControllerBase
    {
        private readonly IXuatVaccineRepository _XuatVaccine;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public XuatVaccineController(IXuatVaccineRepository XuatVaccineRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _XuatVaccine = XuatVaccineRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-xuat-vaccine-by-id")]
        public async Task<ActionResult<XuatVaccineEntity>> GetXuatVaccineById(string id)
        {
            try
            {
                var entity = await _XuatVaccine.GetXuatVaccineById(id);

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
        [Route("/api/[controller]/get-all-xuat-vaccine")]
        public async Task<ActionResult<IEnumerable<XuatVaccineEntity>>> GetAllXuatVaccine()
        {
            try
            {
                var entity = await _XuatVaccine.GetAllXuatVaccine();

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
        //[Route("/api/[controller]/search-XuatVaccine")]
        //public async Task<ActionResult<IEnumerable<XuatVaccineEntity>>> SearchXuatVaccine(string searchKey)
        //{
        //    try
        //    {
        //        var XuatVaccineList = await _XuatVaccine.SearchXuatVaccine(searchKey);
        //        if (!XuatVaccineList.Any())
        //        {
        //            return BadRequest("Not found!");
        //        }
        //        return Ok(XuatVaccineList);
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
        [Route("/api/[controller]/create-xuat-vaccine")]
        public async Task<ActionResult<string>> CreateXuatVaccine(XuatVaccineModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var entity = _mapper.Map<XuatVaccineEntity>(model);
                entity.MaNhanVien = userId;
                var result = await _XuatVaccine.CreateXuatVaccine(entity);

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
        [Route("/api/[controller]/update-xuat-vaccine")]
        public async Task<ActionResult> UpdateXuatVaccine(string id, XuatVaccineModel entity)
        {
            try
            {

                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<XuatVaccineEntity>(entity);
                mapEntity.MaNhanVien = userId;
                await _XuatVaccine.UpdateXuatVaccine(id, mapEntity);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Cập nhật thành công!"));
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
        [Route("/api/[controller]/delete-xuat-vaccine")]
        public async Task<ActionResult<XuatVaccineEntity>> DeleteXuatVaccine(string keyId)
        {

            try
            {

                await _XuatVaccine.DeleteXuatVaccine(keyId, false);
                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Xóa thành công!"));
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
