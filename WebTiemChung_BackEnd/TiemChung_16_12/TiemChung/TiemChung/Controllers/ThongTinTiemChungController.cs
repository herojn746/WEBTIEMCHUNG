using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class ThongTinTiemChungController : ControllerBase
    {
        private readonly IThongTinTiemChungRepository _thongTinTiemChung;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        public ThongTinTiemChungController(IThongTinTiemChungRepository ThongTinTiemChungRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _thongTinTiemChung = ThongTinTiemChungRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("/api/[controller]/get-thong-tin-tiem-chung-by-id/{id}")]
        public async Task<ActionResult<ThongTinTiemChungEntity>> GetThongTinTiemChungById(string id)
        {
            try
            {
                var entity = await _thongTinTiemChung.GetThongTinTiemChungById(id);

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
        [Route("/api/[controller]/get-all-thong-tin-tiem-chung")]
        public async Task<ActionResult<IEnumerable<ThongTinTiemChungEntity>>> GetAllThongTinTiemChung()
        {
            try
            {
                var entity = await _thongTinTiemChung.GetAllThongTinTiemChung();

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
        [Route("/api/[controller]/get-report-thong-tin-tiem-chung")]
        public async Task<ActionResult<IEnumerable<ThongTinTiemChungEntity>>> ReportThongTinTiemChung(DateTime ngayBD, DateTime NgayKT)
        {
            try
            {
                var entity = await _thongTinTiemChung.ReportThongTinTiemChung(ngayBD,NgayKT);

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
        //============================================================
        //[Authorize(Roles = "NhanVien")]
        [HttpGet]
        [Route("/api/[controller]/get-all-thong-tin-tiem-chung-khach-hang")]
        public async Task<ActionResult<IEnumerable<ThongTinTiemChungEntity>>> GetAllThongTinTiemChungKhachHang()
        {
            try
            {
                var entity = await _thongTinTiemChung.GetAllThongTinTiemChungKhachHang();

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
        [Route("/api/[controller]/get-all-thong-tin-tiem-chung-gia-dinh")]
        public async Task<ActionResult<IEnumerable<ThongTinTiemChungEntity>>> GetAllThongTinTiemChungGiaDinh(string maHoGiaDinh)
        {
            try
            {
                var entity = await _thongTinTiemChung.GetAllThongTinTiemChungByIdHoGD(maHoGiaDinh);


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
        //============================================================

        [HttpGet]
        [Route("/api/[controller]/search-thong-tin-tiem-chung")]
        public async Task<ActionResult<IEnumerable<ThongTinTiemChungEntity>>> SearchThongTinTiemChung(string searchKey)
        {
            try
            {
                var ThongTinTiemChungList = await _thongTinTiemChung.SearchThongTinTiemChung(searchKey);
                if (!ThongTinTiemChungList.Any())
                {
                    return BadRequest("Not found!");
                }
                return Ok(ThongTinTiemChungList);
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
        //khach hang đang ky
        [HttpPost]
        [Route("/api/[controller]/create-thong-tin-tiem-chung-khach-hang")]
        public async Task<ActionResult<string>> CreateThongTinTiemChungKhachHang(ThongTinTiemChungKhachHangModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var entity = _mapper.Map<ThongTinTiemChungEntity>(model);
                entity.Id= Guid.NewGuid().ToString("N");
                entity.MaKhachHang = userId;
                var result = await _thongTinTiemChung.CreateThongTinTiemChung(entity);

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

        //khach hang đang ky cho hộ gia đình
        [HttpPost]
        [Route("/api/[controller]/create-thong-tin-tiem-chung-gia-dinh")]
        public async Task<ActionResult<string>> CreateThongTinTiemChungHoGiaDinh(ThongTinTiemChungGiaDinhModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var entity = _mapper.Map<ThongTinTiemChungEntity>(model);
                entity.Id = Guid.NewGuid().ToString("N");
                entity.MaKhachHang = userId;
                var result = await _thongTinTiemChung.CreateThongTinTiemChung(entity);

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
        //============================================

        [HttpPost]
        [Route("/api/[controller]/create-thong-tin-tiem-chung")]
        public async Task<ActionResult<string>> CreateThongTinTiemChung(ThongTinTiemChungModel model)
        {
            try
            {
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<ThongTinTiemChungEntity>(model);
                mapEntity.MaKhachHang = userId;
                var result = await _thongTinTiemChung.CreateThongTinTiemChung(mapEntity);

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
        [Route("/api/[controller]/update-thong-tin-tiem-chung")]
        public async Task<ActionResult> UpdateThongTinTiemChung(string id, ThongTinTiemChungModel entity)
        {
            try
            {
                
                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                var mapEntity = _mapper.Map<ThongTinTiemChungEntity>(entity);
                mapEntity.MaKhachHang = userId;
                await _thongTinTiemChung.UpdateThongTinTiemChung(id, mapEntity);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Thông tin tiêm chủng updated successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status400BadRequest,
                    code: "Error",
                    message: ex.Message));
            }
        }
        [HttpPut]
        [Route("/api/[controller]/update-thong-tin-tiem-chung-nhan-vien/{id}")]
        public async Task<ActionResult> UpdateThongTinTiemChungNhanVien(string id, ThongTinTiemChungNhanVienModel entity)
        {
            try
            {

                byte[] userIdBytes = await _distributedCache.GetAsync("UserId"); // Lấy giá trị UserId từ Distributed Cache
                string userId = Encoding.UTF8.GetString(userIdBytes);

                //var mapEntity = _mapper.Map<ThongTinTiemChungEntity>(entity);
                ThongTinTiemChungEntity mapEntity = new ThongTinTiemChungEntity()
                {
                    NgayTiem=entity.NgayTiem,
                    GioTiem=entity.GioTiem,
                    //KetQua=entity.KetQua,
                    TrangThai=entity.TrangThai,
                    HTTruocTiem = entity.HTTruocTiem,
                    HTSauTiem = entity.HTSauTiem,
                    //MaLoaiTiemChung= entity.MaLoaiTiemChung,
                };
                mapEntity.CreateBy = userId;
                await _thongTinTiemChung.UpdateThongTinTiemChungNhanVien(id, mapEntity);

                return Ok(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status200OK,
                    code: "Success!",
                    data: "Thông tin tiêm chủng updated successfully!"));
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
        [Route("/api/[controller]/delete-thong-tin-tiem-chung/{id}")]
        public async Task<ActionResult<ThongTinTiemChungEntity>> DeleteThongTinTiemChung(string id)
        {

            try
            {

                await _thongTinTiemChung.DeleteThongTinTiemChung(id, false);
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
