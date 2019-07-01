using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DOTNET_CuoiKy.Models.DB
{
    public partial class Sanpham
    {
        public Sanpham()
        {
            Carts = new HashSet<Carts>();
            Chitiethd = new HashSet<Chitiethd>();
        }

        public int IdsanPham { get; set; }
        public string TenSp { get; set; }
        public float? GiaSp { get; set; }
        public string Hinh1 { get; set; }
        public string Hinh2 { get; set; }
        public string Hinh3 { get; set; }
        public string Hinh4 { get; set; }
        public string MoTa { get; set; }
        public int? DanhMuc { get; set; }

        public virtual Danhmuc DanhMucNavigation { get; set; }
        public virtual ICollection<Carts> Carts { get; set; }
        public virtual ICollection<Chitiethd> Chitiethd { get; set; }
    }
}
