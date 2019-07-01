using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DOTNET_CuoiKy.Areas.admin.Models;
namespace DOTNET_CuoiKy.Areas.admin.Controllers
{
    public class Image
    {
        private IHostingEnvironment _hostingEnv;
        public Image(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        //[HttpPost]
        //public async Task<IActionResult> up(UploadImage model, IFormFile ImageFile)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var filename = ContentDispositionHeaderValue.Parse(ImageFile.ContentDisposition).FileName.Trim('"');
        //        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", ImageFile.FileName);
        //        using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
        //        {
        //            await ImageFile.CopyToAsync(stream);
        //        }
        //        model.ImageFile = filename;
        //        _context.Add(model);
        //    }
        //}
    }
}
