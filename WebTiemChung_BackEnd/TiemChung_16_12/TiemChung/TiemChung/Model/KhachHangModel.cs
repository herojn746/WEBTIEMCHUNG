using System.ComponentModel.DataAnnotations;

namespace TiemChung.Model
{
    public class KhachHangModel
    {
        public string Id { get; set; }
        public string TenKhachHang { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression("^(03|05|07|08|09)\\d{8}$", ErrorMessage = "Số điện thoại phải bắt đầu 03|05|07|08|09!")]
        public string SDT { get; set; }

        [RegularExpression("^[0-9]{9}$|^[0-9]{12}$", ErrorMessage = "Invalid ID")]
        [StringLength(12, MinimumLength = 9, ErrorMessage = "Số chứng minh phải là 9 hoặc 12 số!")]
        public string CMND { get; set; }
         
    }
}
