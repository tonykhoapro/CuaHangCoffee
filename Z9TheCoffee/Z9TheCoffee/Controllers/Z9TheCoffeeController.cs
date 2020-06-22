using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z9TheCoffee.Models;

namespace Z9TheCoffee.Controllers
{
    public class Z9TheCoffeeController : Controller
    {
        // GET: TrangChu
        DataClasses1DataContext data = new DataClasses1DataContext();
        private List<ChiTietDoUong> Laydouongmoi(int count)
        {
            // sắp xếp giảm dần theo ngày cập nhật, lấy count dòng đầu
            var Laydouongmoi = from ctdu in data.ChiTietDoUongs where ctdu.TrangThai == true select ctdu;
            return Laydouongmoi.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
            //return data.ChiTietDoUongs.Where(a=> a.TrangThai()).OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        public ActionResult Index()
        {           
            var douongmoi = Laydouongmoi(100);           
            return View(douongmoi);
        }
        public ActionResult CaPheViet()
        {
            var capheviet = from cfviet in data.ChiTietDoUongs where cfviet.MaLoai == 1 && cfviet.TrangThai == true select cfviet;
            return PartialView(capheviet);
        }
        public ActionResult CaPheY()
        {
            var caphey = from cfy in data.ChiTietDoUongs where cfy.MaLoai == 2 && cfy.TrangThai == true select cfy;
            return PartialView(caphey);
        }
        public ActionResult Tra()
        {
            var tra = from t in data.ChiTietDoUongs where t.MaLoai == 3 && t.TrangThai == true select t;
            return PartialView(tra);
        }

    }
}