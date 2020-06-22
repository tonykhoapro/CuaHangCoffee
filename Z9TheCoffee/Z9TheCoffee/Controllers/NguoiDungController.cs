using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z9TheCoffee.Models;

namespace Z9TheCoffee.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung    
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DangKi()
        {
            return View();
        }
        //POST :Hàm DangKi nhận dữ liệu từ trang DangKi và tạo mới dữ liệu
        [HttpPost]
        public ActionResult DangKi(FormCollection collection, KhachHang KH)
        {
            // Gán các giá trị đã nhập
            var hoten = collection["TenKH"];
            var matkhau = collection["MatKhau"];
            //var nhaplaimatkhau = collection["NhapLaiMatKhau"];
            var email = collection["Email"];
            try
            {
                KH.TenKH = hoten;
                KH.Email = email;
                KH.MatKhau = matkhau;
                data.KhachHangs.InsertOnSubmit(KH);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            catch { ViewBag.ThongBao = "Email này đã được người khác sử dụng !"; ViewBag.Trung = "Vui lòng sử dụng Email khác"; }
            return this.DangKi();
        }
        public ActionResult DangNhap(FormCollection collection)
        {
            var email = collection["Email"];
            var matkhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi1"] = "* Bắt buộc";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "* Bắt buộc";
            }
            else
            {
                KhachHang KH = data.KhachHangs.SingleOrDefault(n => n.Email == email && n.MatKhau == matkhau);
                if (KH != null)
                {
                    Session["Email"] = KH;
                    Session["TenKH"] = KH.TenKH;
                    Session["MaKH"] = KH.MaKH;
                    return RedirectToAction("GioHang", "GioHang");
                }
                else
                {
                    ViewBag.ThongBao = "Tài Khoản hoặc Mật khẩu không đúng";
                }
            }
            return View();
        }
        // đăng xuất
        public ActionResult DangXuat()
        {
            Session["Email"] = null;
            return RedirectToAction("Index", "Z9TheCoffee");
        }
        // tạo partipal view de hien thi lên màn hình chính
        public ActionResult NguoiDungPartial(FormCollection collection)
        {
            return PartialView();
        }
        public List<LichSuMuaHang> listLichSuMuaHang(int id)
        {
            var listLichSuMuaHang = from ddh in data.DonDatHangs
                                    from ctdh in data.ChiTietDonHangs
                                    from kh in data.KhachHangs
                                    from ctd in data.ChiTietDoUongs
                                    where ddh.MaDonHang == ctdh.MaDonHang && kh.MaKH == ddh.MaKH && ctdh.MaDoUong == ctd.MaDoUong && kh.MaKH == id
                                    select new LichSuMuaHang()
                                    {
                                        NgayDat = ddh.NgayDat,
                                        DiaChiGiao = ddh.DiaChiGiao,
                                        TenDoUong = ctd.TenDoUong,
                                        SL_Mua = ctdh.SlMua,
                                        DonGia = ctdh.DonGia
                                    };
            return listLichSuMuaHang.OrderBy(a => a.NgayDat).ToList();
        }
        public ActionResult HienThiLichSuMuaHang(int id)
        {
            if (Session["TenKH"] == null || Session["TenKH"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            var laylichsumuahang = listLichSuMuaHang(id);
            return View(laylichsumuahang);
        }
        public ActionResult DatBan()
        {
            if (Session["Email"] == null || Session["Email"].ToString() == "")
            {                
                return RedirectToAction("DangNhap", "NguoiDung");                
            }           
            ViewBag.MaChiNhanh = new SelectList(data.ChiNhanhs.ToList(), "MaChiNhanh", "TenChiNhanh");
            return View();
        }
        //POST :Hàm DangKi nhận dữ liệu từ trang DangKi và tạo mới dữ liệu
        [HttpPost]
        public ActionResult DatBan(FormCollection collection, DatBan datban)
        {
            try
            {          
                KhachHang kh = (KhachHang)Session["Email"];
                datban.MaChiNhanh = Convert.ToInt32(collection["MaChiNhanh"]);
                datban.NgayGui = Convert.ToDateTime(DateTime.Now.ToString());
                datban.NgayDatBan = Convert.ToDateTime(collection["NgayDatBan"]);
                datban.GioDatBan = collection["GioDatBan"];
                datban.TenKhachDatBan = collection["TenKhachDatBan"];
                datban.SoLuongNguoi = Convert.ToInt32(collection["SoLuongNguoi"]);
                datban.DienThoaiDatBan = collection["DienThoaiDatBan"];
                datban.DaXong = false;
                data.DatBans.InsertOnSubmit(datban);
                data.SubmitChanges();
                //send mail 
                string content_datban = System.IO.File.ReadAllText(Server.MapPath("~/SendMail/xacnhandatban.html"));
                content_datban = content_datban.Replace("{{TenKH}}", datban.TenKhachDatBan);
                content_datban = content_datban.Replace("{{ChiNhanhDat}}", datban.ChiNhanh.TenChiNhanh);
                content_datban = content_datban.Replace("{{NgayGui}}", Convert.ToString(datban.NgayGui));
                content_datban = content_datban.Replace("{{NgayDatBan}}", String.Format("{0:dd/MM/yyyy}", datban.NgayDatBan));
                content_datban = content_datban.Replace("{{GioDatBan}}", datban.GioDatBan);
                content_datban = content_datban.Replace("{{SoLuongNguoi}}", Convert.ToString(datban.SoLuongNguoi));
                content_datban = content_datban.Replace("{{DienThoaiDatBan}}", datban.DienThoaiDatBan);
                var toEmail_datban = ConfigurationManager.AppSettings["ToEmailAddress_datban"].ToString();
                new MailHelper_datban().SendMail_datban(kh.Email, "Xác Nhận Đặt Bàn từ Z9TheCoffee", content_datban);
                new MailHelper_datban().SendMail_datban(toEmail_datban, "Đơn Đặt Bàn mới từ Z9TheCoffee", content_datban);
                //
                return RedirectToAction("XacNhanDatBan");
        }
            catch { return RedirectToAction("Loi", "GioHang");}          
    }
        public ActionResult XacNhanDatBan()
        {
            return View();
        }
    }
}