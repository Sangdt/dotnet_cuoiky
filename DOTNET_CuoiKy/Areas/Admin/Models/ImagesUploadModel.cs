using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_CuoiKy.Areas.Admin.Models
{
    public class ImagesUploadModel
    {
        public string idSP { get; set; }
        public string vitriHinh { get; set; }
        public IFormFile images { get; set; }
    }
}
