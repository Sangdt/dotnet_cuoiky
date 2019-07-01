using System;
using System.Collections.Generic;

namespace DOTNET_CuoiKy.Models.DB
{
    public partial class Carts
    {
        public string AutoId { get; set; }
        public string CartId { get; set; }
        public int SpId { get; set; }
        public int? Quantity { get; set; }

        public virtual Sanpham Sp { get; set; }
    }
}
