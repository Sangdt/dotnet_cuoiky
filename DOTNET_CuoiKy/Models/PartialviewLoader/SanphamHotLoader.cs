using DOTNET_CuoiKy.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_CuoiKy.Models.PartialviewLoader
{
    public class SanphamHotLoader : ViewComponent
    {
        static Random rnd = new Random();
        private readonly comdatabaseContext _context;
        public SanphamHotLoader(comdatabaseContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            //Select random san pham here
            int i = 4;
            List<Sanpham> splist = _context.Sanpham.ToList();
            List<Sanpham> dmLst = new List<Sanpham>();
            Sanpham sptemp = new Sanpham();
            while (i >= 0)
            {
                int r = rnd.Next(splist.Count);
                if (dmLst.Count <= 0)
                {
                    dmLst.Add(splist[r]);
                    sptemp = splist[r];
                    i--;
                }
                else if(sptemp!= splist[r])
                {
                    dmLst.Add(splist[r]);
                    sptemp = splist[r];
                    i--;
                }
            }
            return View("sanphamHotLoader", dmLst);
        }
    }
}
