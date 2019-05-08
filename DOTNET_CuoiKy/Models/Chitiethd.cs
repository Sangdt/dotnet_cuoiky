using System;
using System.Collections.Generic;

namespace DOTNET_CuoiKy.Models
{
    public partial class Chitiethd
    {
        public int IdHd { get; set; }
        public int IdSp { get; set; }
        public string Soluong { get; set; }
        public float? GiaSp { get; set; }

        public virtual Hoadon IdHdNavigation { get; set; }
        public virtual Sanpham IdSpNavigation { get; set; }
    }
}
