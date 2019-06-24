using DOTNET_CuoiKy.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Threading;

namespace DOTNET_CuoiKy.Models.PartialviewLoader
{
    public class SanphamHotLoader : ViewComponent
    {
        static Random rnd = new Random();
        private readonly comdatabaseContext _context;
        List<Sanpham> spLst;
        public SanphamHotLoader(comdatabaseContext context)
        {
            _context = context;
        }
        private List<Sanpham> GetSanphams()
        {
            bool success = false;
            int retryCount = 0;
            MySqlException exception = null ;
            while (!success&& retryCount<=15)
            {
                try
                {
                    spLst = _context.Sanpham.ToList();
                    success = true;
                    //retryCount = 15;
                    //exception = null;
                }
                catch (MySqlException e)
                {
                    exception = e;
                    success = false;
                    retryCount++;
                    Debug.WriteLine(retryCount, "Reset time");
                    Thread.Sleep(800);
                }
            }
            if(retryCount>=15)
            {
                throw exception;
            }
            return spLst;
        }
        public IViewComponentResult Invoke()
        {
            //Select random san pham here
            int i = 4;
            //List<Sanpham> splist = _context.Sanpham.ToList();
            List<Sanpham> splist = GetSanphams();
            List <Sanpham> dmLst = new List<Sanpham>();
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
