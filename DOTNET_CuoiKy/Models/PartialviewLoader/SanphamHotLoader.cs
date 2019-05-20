using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_CuoiKy.Models.PartialviewLoader
{
    public class SanphamHotLoader : ViewComponent
    {
        private readonly comdbContext _context;
        public SanphamHotLoader(comdbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            //Select random san pham here
            List<Danhmuc> dmLst = _context.Danhmuc.ToList();

            return View("sanphamHotLoader", dmLst);
        }
    }
}
