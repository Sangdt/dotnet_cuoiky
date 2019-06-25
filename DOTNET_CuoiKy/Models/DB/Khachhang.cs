using System;
using System.Collections.Generic;

namespace DOTNET_CuoiKy.Models.DB
{
    public partial class Khachhang
    {
        public Khachhang()
        {
            Carts = new HashSet<Carts>();
            Hoadon = new HashSet<Hoadon>();
        }

        public int IdKhachHang { get; set; }
        public string NameKh { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public int SoDiethoai { get; set; }

        public virtual ICollection<Carts> Carts { get; set; }
        public virtual ICollection<Hoadon> Hoadon { get; set; }
    }
}
