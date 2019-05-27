using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DOTNET_CuoiKy.Models.Client
{
    //Dùng cho cả đăng ký và đăng nhập
    public partial class LoginRegisterModel
    {
        [Required]
        [Display(Name ="Số điện thoại hoặc email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Nhập email hay số điện thôi bạn ie")]
        public string userName { get; set; }

        [Required]
        [Display(Name = "Mật khẩu của bẹn")]
        [DataType(DataType.Password)]
        public string passWord { get; set; }
        public LoginRegisterModel() { }
    }
}
