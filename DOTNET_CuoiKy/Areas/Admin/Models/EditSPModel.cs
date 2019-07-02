using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_CuoiKy.Areas.Admin.Models
{
    public class EditSPModel
    {
        public int IdsanPham { get; set; }
        public string TenSp { get; set; }
        public float? GiaSp { get; set; }
        public string MoTa { get; set; }
        public int? DanhMuc { get; set; }

    }
}
