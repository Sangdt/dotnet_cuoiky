using System;
using System.Collections.Generic;

namespace DOTNET_CuoiKy.Models.DB
{
    public partial class Danhmuc
    {
        public Danhmuc()
        {
            Sanpham = new HashSet<Sanpham>();
        }

        public int IddanhMuc { get; set; }
        public string TenDm { get; set; }

        public virtual ICollection<Sanpham> Sanpham { get; set; }
    }
}
