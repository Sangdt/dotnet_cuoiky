using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_CuoiKy.Areas.admin.Models
{
    public class UploadImage
    {
        public int IdsanPham { get; set; }
        public string TenSp { get; set; }
        public float? GiaSp { get; set; }
        public string Hinh1 { get; set; }
        public string Hinh2 { get; set; }
        public string Hinh3 { get; set; }
        public string Hinh4 { get; set; }
        public string MoTa { get; set; }
        public int? DanhMuc { get; set; }
    }
}
