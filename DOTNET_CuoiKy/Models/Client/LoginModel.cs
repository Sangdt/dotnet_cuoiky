using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DOTNET_CuoiKy.Models.Client
{
    public partial class LoginModel
    {
        [Required]
        [Display(Name ="Số điện thoại hoặc email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Please enter a valid email address or phone number")]
        public string userName { get; set; }

        [Required]
        [Display(Name = "Mật khẩu của bẹn")]
        [DataType(DataType.Password)]
        public string passWord { get; set; }
        public LoginModel() { }
    }
}
