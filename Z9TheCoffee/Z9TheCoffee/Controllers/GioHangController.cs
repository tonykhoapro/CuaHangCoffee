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
    public class GioHangController : Controller
    {
        // GET: GioHang
        DataClasses1DataContext data = new DataClasses1DataContext();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> listGiohang = Session["Giohang"] as List<GioHang>;
            if (listGiohang == null)
            {
                // nếu giỏ hàng chưa tồn tại thì khởi tạo listGioHang
                listGiohang = new List<GioHang>();
                Session["Giohang"] = listGiohang;
            }
            return listGiohang;
        }
        // thêm vào giỏ hàng
        public ActionResult ThemGioHang(int iMaDoUong, string strURL)
        {
            // lấy ra session giohang
            List<GioHang> listGiohang = LayGioHang();
            // kiểm tra sách này tồn tại trong Session["GioHang"] chưa
            GioHang sanpham = listGiohang.Find(n => n.iMaDoUong == iMaDoUong);
            if (sanpham == null)
            {
                sanpham = new GioHang(iMaDoUong);
                listGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSlMua++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> listGiohang = Session["Giohang"] as List<GioHang>;
            if (listGiohang != null)
            {
                iTongSoLuong = listGiohang.Sum(n => n.iSlMua);
            }
            return iTongSoLuong;
        }
        // tính tổng tiền
        private decimal TongTien()
        {
            decimal iTongTien = 0;
            List<GioHang> listGiohang = Session["Giohang"] as List<GioHang>;
            if (listGiohang != null)
            {
                iTongTien = listGiohang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }
        // xây dựng trang giỏ hàng
        public ActionResult GioHang()
        {
            List<GioHang> listGiohang = LayGioHang();
            if (listGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Z9TheCoffee");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGiohang);
        }
        // tạo partipal view de hien thi lên màn hình chính
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        //xóa Giohang
        public ActionResult XoaGioHang(int iMaDoUong)
        { //Lay gio hang tu Session
            List<GioHang> lstGiohang = LayGioHang();
            //kiem tra sach da co trong Session[ Giohang"
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iMaDoUong == iMaDoUong);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMaDoUong == iMaDoUong);
                return RedirectToAction("GioHang");
            }

            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Z9TheCoffee");
            }
            return RedirectToAction("GioHang");
        }
        //Cap nhat Giỏ hàng
        public ActionResult CapNhatGioHang(int iMaDoUong, FormCollection f)
        {
            //Lay gio hang tu Session
            List<GioHang> lstGiohang = LayGioHang();
            //kiem tra sach da co trong Session["Giohang"]
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iMaDoUong == iMaDoUong);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                sanpham.iSlMua = int.Parse(f["txtSlMua"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            // lấy giỏ hàng từ session
            List<GioHang> lstGiohang = LayGioHang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "Z9TheCoffee");
        }

        public ActionResult DatHang()
        {
            if (Session["Email"] == null || Session["Email"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Z9TheCoffee");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.MaQuan = new SelectList(data.QuanGiaos.ToList(), "MaQuan", "TenQuan");
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            try
            {
                DonDatHang ddh = new DonDatHang();
                KhachHang kh = (KhachHang)Session["Email"];
                List<GioHang> giohang = LayGioHang();
                int quangiao = Convert.ToInt32(collection["MaQuan"]);
                var diachigiao = collection["DiaChiGiao"];
                var dienthoaikh = collection["DienThoai_KH"];
                decimal tongtien = 0;
                int tongsoluong = 0;
                ddh.NgayDat = DateTime.Now;
                ddh.GioDat = DateTime.Now;
                ddh.MaKH = kh.MaKH;
                ddh.MaQuan = quangiao;
                ddh.DiaChiGiao = diachigiao;
                ddh.DienThoai_KH = dienthoaikh;
                ddh.DaThanhToan = false;
                data.DonDatHangs.InsertOnSubmit(ddh);
                data.SubmitChanges();
                foreach (var item in giohang)
                {
                    ChiTietDonHang ctdh = new ChiTietDonHang();
                    ctdh.MaDonHang = ddh.MaDonHang;
                    ctdh.MaDoUong = item.iMaDoUong;
                    ctdh.SlMua = item.iSlMua;
                    ctdh.DonGia = (decimal)item.dGiaBan;
                    data.ChiTietDonHangs.InsertOnSubmit(ctdh);
                    tongtien += (item.iSlMua * item.dGiaBan);
                    tongsoluong += item.iSlMua;
                }
                data.SubmitChanges();
                Session["Giohang"] = null;

                string content = System.IO.File.ReadAllText(Server.MapPath("~/SendMail/noidung.html"));
                content = content.Replace("{{TenKH}}", kh.TenKH);
                content = content.Replace("{{DienThoai_KH}}", ddh.DienThoai_KH);
                content = content.Replace("{{Email}}", kh.Email);
                content = content.Replace("{{QuanGiao}}", ddh.QuanGiao.TenQuan);
                content = content.Replace("{{DiaChiGiao}}", ddh.DiaChiGiao);
                content = content.Replace("{{TongSoLuong}}", Convert.ToString(tongsoluong));
                content = content.Replace("{{TongTien}}", String.Format("{0:0,0}", tongtien));
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new MailHelper().SendMail(kh.Email, "Xác Nhận Đơn Hàng từ Z9TheCoffee", content);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Z9TheCoffee", content);
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            catch
            {
                return RedirectToAction("Loi", "GioHang");
            }


        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        public ActionResult ThanhToan_ThanhCong()
        {
            return View();
        }
        public ActionResult Loi()
        {
            return View();
        }
    }
}