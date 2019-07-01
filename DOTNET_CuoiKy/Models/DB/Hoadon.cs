using System;
using System.Collections.Generic;

namespace DOTNET_CuoiKy.Models.DB
{
    public partial class Hoadon
    {
        public Hoadon()
        {
            Chitiethd = new HashSet<Chitiethd>();
        }

        public int Idhoadon { get; set; }
        public int IdNguoimua { get; set; }
        public string Nguoinhan { get; set; }
        public string TongTien { get; set; }
        public int? SoLuong { get; set; }
        public DateTime? NgayTao { get; set; }
        public string TinhTrang { get; set; }

        public virtual Khachhang IdNguoimuaNavigation { get; set; }
        public virtual ICollection<Chitiethd> Chitiethd { get; set; }
    }
}
