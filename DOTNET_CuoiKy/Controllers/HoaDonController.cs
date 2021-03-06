﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DOTNET_CuoiKy.Models.Client;
using DOTNET_CuoiKy.Models.DB;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_CuoiKy.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HoaDonController : Controller
    {
        comdatabaseContext _context;
        public HoaDonController(comdatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        //Method to create an Invoke
        Hoadon HoadonCreator(InvoceModel model)
        {
            Hoadon hoadon;
            // If this is the first one in db
            if (_context.Hoadon.Count() <= 0)
            {
                hoadon = new Hoadon()
                {
                    Idhoadon = 1000,
                    IdNguoimua = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    Nguoinhan = model.tenNguoiNhan,
                    TongTien = model.toltal,
                    SoLuong = model.Quantity,
                    Ghichu = model.GhiChu,
                    NgayTao = DateTime.Now,
                    Diachi= model.DiaChi,
                    TinhTrang = "Đang Xử Lý",
                };
            }
            else
            {
                // else just add up the value
                int idToAdd = _context.Hoadon.Last().Idhoadon + 1;
                hoadon = new Hoadon()
                {
                    Idhoadon = idToAdd,
                    IdNguoimua = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    Nguoinhan = model.tenNguoiNhan,
                    TongTien = model.toltal,
                    SoLuong = model.Quantity,
                    Ghichu = model.GhiChu,
                    Diachi = model.DiaChi,
                    NgayTao = DateTime.Now,
                    TinhTrang = "Đang Xử Lý",
                };
            }
            return hoadon;
        }

        [HttpPost, ActionName("CreateInvoce")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateInvoce(InvoceModel model)
        {
            // Tach tao hoa don ra rieng de nhin va de tranh loi null
            Hoadon HDtoADD = HoadonCreator(model);
            List<Chitiethd> chitiethds = new List<Chitiethd>();
            // Lay danh sach san pham trong cart
            var cartLst = await _context.Carts.Include(sp => sp.Sp).ToListAsync();
            foreach (Carts item in cartLst)
            {
                // Tao chi tiet hoa don vs san pham vaf thong tin trong cart
                Chitiethd chitiet = new Chitiethd()
                {
                    IdHd = HDtoADD.Idhoadon,
                    IdSp = item.SpId,
                    GiaSp = item.Sp.GiaSp,
                    Soluong = item.Quantity.Value.ToString(),
                };
                chitiethds.Add(chitiet);
            }
            _context.Carts.RemoveRange(cartLst);
            _context.Hoadon.Add(HDtoADD);
            _context.Chitiethd.AddRange(chitiethds);
            await _context.SaveChangesAsync();
            ViewData["Message"] = "Đã lưu thông tin Hóa Đơn, mã hóa đơn của bạn là " + HDtoADD.Idhoadon.ToString();
            return RedirectToAction("Index","HoaDon");
        }
    }
}