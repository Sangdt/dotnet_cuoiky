using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DOTNET_CuoiKy.Models.DB
{
    public partial class Admin
    {
        
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Nhập email bạn ie")]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
