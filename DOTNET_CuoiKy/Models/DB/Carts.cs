using System;
using System.Collections.Generic;

namespace DOTNET_CuoiKy.Models.DB
{
    public partial class Carts
    {
        public string AutoId { get; set; }
        public int UserId { get; set; }
        public int SpId { get; set; }
        public int? Quantity { get; set; }
        public string Total { get; set; }

        public virtual Sanpham Sp { get; set; }
        public virtual Khachhang User { get; set; }
    }
}
