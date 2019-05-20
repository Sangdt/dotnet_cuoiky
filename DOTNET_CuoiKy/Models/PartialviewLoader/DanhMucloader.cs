using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_CuoiKy.Models.PartialviewLoader
{
    public class DanhMucloader : ViewComponent
    {
        private readonly comdbContext _context;
        public DanhMucloader(comdbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            List<Danhmuc> dmLst =_context.Danhmuc.ToList();

            return View("danhmucLoader", dmLst);
        }
    }
}
