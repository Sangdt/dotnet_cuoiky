using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http.Headers;
using DOTNET_CuoiKy.Models.DB;
using DOTNET_CuoiKy.Areas.Admin.Models;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;

namespace DOTNET_CuoiKy.Areas.Admin.Controllers
{
    public class UploadimageController : Controller
    {
        private readonly comdatabaseContext db;
        //private readonly IHostingEnvironment _environment;
        public UploadimageController(comdatabaseContext context)//,IHostingEnvironment IHostingEnvironment)
        {
            db = context;
            //_environment = IHostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        private async Task<int> updateImagesPOSAsync(Sanpham spToUpdate,string viTrih,string newFileName, IFormFile images)
        { 
            //Luu vao db
            var PathDB = "images/Sanpham/" + newFileName;

            //kiem vi tri de luu tru hinh
            switch (viTrih)
            {
                case "1":
                    {
                        // Xoa hinh cu 
                        if (!string.IsNullOrEmpty(spToUpdate.Hinh1))
                        {
                            var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", spToUpdate.Hinh1);
                            if (System.IO.File.Exists(oldpath))
                            {
                                System.IO.File.Delete(oldpath);
                            }
                        }
                        // thay the duong dan hinh moi vao db
                        spToUpdate.Hinh1 = PathDB;

                        break;
                    }
                case "2":
                    {
                        // Xoa hinh cu 
                        if (!string.IsNullOrEmpty(spToUpdate.Hinh2))
                        {
                            var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", spToUpdate.Hinh2);
                            if (System.IO.File.Exists(oldpath))
                            {
                                System.IO.File.Delete(oldpath);
                            }
                        }
                        spToUpdate.Hinh2 = PathDB;
                        break;
                    }
                case "3":
                    {
                        if (!string.IsNullOrEmpty(spToUpdate.Hinh3))
                        {
                            var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", spToUpdate.Hinh3);
                            if (System.IO.File.Exists(oldpath))
                            {
                                System.IO.File.Delete(oldpath);
                            }
                        }
                        spToUpdate.Hinh3 = PathDB;
                        break;
                    }
                case "4":
                    {
                        // Xoa hinh cu 
                        if (!string.IsNullOrEmpty(spToUpdate.Hinh4))
                        {
                            var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", spToUpdate.Hinh4);
                            if (System.IO.File.Exists(oldpath))
                            {
                                System.IO.File.Delete(oldpath);
                            }
                        }
                        spToUpdate.Hinh4 = PathDB;
                        break;
                    }
            }
            db.Sanpham.Update(spToUpdate);

            await db.SaveChangesAsync();
            //path to create new file
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Sanpham", newFileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await images.CopyToAsync(stream);
            }

            return 0;
        }

        [HttpPost,ActionName("Upload")]
        public async Task<IActionResult> IndexAsync([FromForm] ImagesUploadModel uploadInfo)
        {
            var newFileName = string.Empty;

            if (uploadInfo.images != null)
            {
                var sptoUpdate = db.Sanpham.FirstOrDefault(sp=>sp.IdsanPham== int.Parse(uploadInfo.idSP.Trim()));

                var fileName = string.Empty;

                if (uploadInfo == null || uploadInfo.images == null || uploadInfo.images.Length == 0)
                    return Content("file not selected");

                fileName = ContentDispositionHeaderValue.Parse(uploadInfo.images.ContentDisposition).FileName.Trim('"');

                //Create Unique Filename (Guid)
                var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                //Getting file Extension
                var FileExtension = Path.GetExtension(fileName);

                // concating  FileName + FileExtension
                newFileName = myUniqueFileName + FileExtension;

                // Bat dau luu tru hinh
                await updateImagesPOSAsync(sptoUpdate, uploadInfo.vitriHinh, newFileName, uploadInfo.images);
               
            }
            return Json("OK baby");
        }
    }
}