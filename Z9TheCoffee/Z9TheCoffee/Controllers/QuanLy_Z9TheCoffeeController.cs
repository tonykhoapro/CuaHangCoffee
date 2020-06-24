using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z9TheCoffee.Models;

namespace Z9TheCoffee.Controllers
{
    public class QuanLy_Z9TheCoffeeController : Controller
    {
        // GET: NguoiDung    
        DataClasses1DataContext data = new DataClasses1DataContext();

        public ActionResult DangNhap(FormCollection collection)
        {
            var taikhoan = collection["TaiKhoan"];
            var matkhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(taikhoan) || String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi1"] = "* Bắt buộc";
            }
            else
            {
                MayChu ad = data.MayChus.SingleOrDefault(n => n.TaiKhoan_admin == taikhoan && n.MatKhau_admin == matkhau);
                if (ad != null)
                {
                    Session["TenAdmin"] = ad.TaiKhoan_admin;
                    return RedirectToAction("Index", "QuanLy_Z9TheCoffee");
                }
                else
                {
                    ViewBag.ThongBao = "* Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        public ActionResult Index()
        {
            if (Session["TenAdmin"] == null || Session["TenAdmin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "QuanLy_Z9TheCoffee");
            }
            else
            {
                return View();
            }
        }
        // đăng xuất
        public ActionResult DangXuat()
        {
            Session["TenAdmin"] = null;
            return RedirectToAction("Index", "QuanLy_Z9TheCoffee");
        }
        public List<listMenu> listQuanLy_Menu()
        {
            var listQuanLy_Menu = from du in data.ChiTietDoUongs
                                  from tl in data.LoaiDoUongs
                                  where du.MaLoai == tl.MaLoai
                                  select new listMenu()
                                  {
                                      NgayCapNhat = Convert.ToDateTime(du.NgayCapNhat),
                                      TenLoai = tl.TenLoai,
                                      MaDoUong = du.MaDoUong,
                                      TenDoUong = du.TenDoUong,
                                      GiaBan = Convert.ToDecimal(du.GiaBan),
                                      AnhBia = du.AnhBia,
                                      TrangThai = Convert.ToBoolean(du.TrangThai)
                                  };
            return listQuanLy_Menu.OrderBy(a => a.NgayCapNhat).ToList();
        }
        public ActionResult Menu(int? page)
        {
            if (Session["TenAdmin"] == null || Session["TenAdmin"].ToString() == "")
            {
                return RedirectToAction("Index", "QuanLy_Z9TheCoffee");
            }
            var laydanhsach = listQuanLy_Menu();
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            return View(laydanhsach.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DanhSachChiNhanh()
        {
            if (Session["TenAdmin"] == null || Session["TenAdmin"].ToString() == "")
            {
                return RedirectToAction("Index", "QuanLy_Z9TheCoffee");
            }
            List<ChiNhanh> chiNhanhs = data.ChiNhanhs.Where(v => v.TrangThai != false).ToList();
            return View(chiNhanhs);
        }
        public ActionResult CapNhatMenu(int id)
        {
            if (Session["TenAdmin"] == null || Session["TenAdmin"].ToString() == "")
            {
                return RedirectToAction("Index", "QuanLy_Z9TheCoffee");
            }
            else
            { //Lay ra doi tuong sach theo ma                                              
                ViewBag.MaLoai = new SelectList(data.LoaiDoUongs.Where(a => a.TrangThai == true).ToList(), "MaLoai", "TenLoai");
                ChiTietDoUong du = data.ChiTietDoUongs.SingleOrDefault(n => n.MaDoUong == id);
                return View(du);
            }
        }
        public ActionResult SuaThongTinChiNhanh(int id)
        {
            if (Session["TenAdmin"] == null || Session["TenAdmin"].ToString() == "")
            {
                return RedirectToAction("Index", "QuanLy_Z9TheCoffee");
            }
            else
            {
                ViewBag.MaLoai = new SelectList(data.ChiNhanhs.Where(a => a.TrangThai == true)).ToList();
                ChiNhanh chiNhanh = data.ChiNhanhs.SingleOrDefault(n => n.MaChiNhanh == id);
                return View(chiNhanh);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaThongTinChiNhanh(FormCollection collection)
        {
            if (Session["TenAdmin"] == null || Session["TenAdmin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "QuanLy_Z9TheCoffee");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        ChiNhanh chiNhanh = data.ChiNhanhs.Where(a => a.MaChiNhanh == Convert.ToInt32(collection["MaChiNhanh"])).FirstOrDefault();
                        chiNhanh.TenChiNhanh = Convert.ToString(collection["TenChiNhanh"]);
                        chiNhanh.TrangThai = Convert.ToBoolean(collection["TrangThai"]);
                        //data.ChiNhanhs.
                        data.SubmitChanges();
                    }

                    catch { ViewBag.Loi = "Vui lòng kiểm tra đầy đủ thông tin"; }


                }
            }
            return RedirectToAction("Menu", "QuanLy_Z9TheCoffee");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CapNhatMenu(int id, FormCollection collection, ChiTietDoUong du, HttpPostedFileBase fileUpload)
        {
            if (Session["TenAdmin"] == null || Session["TenAdmin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "QuanLy_Z9TheCoffee");
            }            
            else
            {               
                //Them vao CSDL               
                if (ModelState.IsValid)
                {
                    try
                    {
                        du = data.ChiTietDoUongs.SingleOrDefault(n => n.MaDoUong == id);
                        du.NgayCapNhat = Convert.ToDateTime(collection["NgayCapNhat"]);
                        du.MaLoai = Convert.ToInt32(collection["MaLoai"]);
                        du.TenDoUong = collection["TenDoUong"];
                        du.GiaBan = Convert.ToDecimal(collection["GiaBan"]);
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/HinhSanPham"), fileName);                                                
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                        du.AnhBia = fileName;
                        du.TrangThai = Convert.ToBoolean(collection["TrangThai"]);
                        //Luu vao CSDL                             
                        data.SubmitChanges();
                    }
                    catch { ViewBag.Loi = "Vui lòng kiểm tra đầy đủ thông tin"; }
                }
                return RedirectToAction("Menu", "QuanLy_Z9TheCoffee");
            }
        }
        public ActionResult ThemMoiMenu()
        {
            if (Session["TenAdmin"] == null || Session["TenAdmin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "QuanLy_Z9TheCoffee");
            }
            ViewBag.MaLoai = new SelectList(data.LoaiDoUongs.Where(a => a.TrangThai == true).ToList(), "MaLoai", "TenLoai");
            return View();
        }
        public ActionResult ThemMoiChiNhanh()
        {
            if (Session["TenAdmin"] == null || Session["TenAdmin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "QuanLy_Z9TheCoffee");
            }
            ViewBag.MaLoai = new SelectList(data.LoaiDoUongs.Where(a => a.TrangThai == true).ToList(), "MaLoai", "TenLoai");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiChiNhanh(FormCollection collection,ChiNhanh chiNhanh)
        {
            if (Session["TenAdmin"] == null || Session["TenAdmin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "QuanLy_Z9TheCoffee");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    chiNhanh.TenChiNhanh = Convert.ToString(collection["TenChiNhanh"]);
                    chiNhanh.TrangThai = Convert.ToBoolean(collection["TrangThai"]);
                    data.ChiNhanhs.InsertOnSubmit(chiNhanh);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("Menu", "QuanLy_Z9TheCoffee");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiMenu(FormCollection collection, ChiTietDoUong du, HttpPostedFileBase fileUpload)
        {

            if (Session["TenAdmin"] == null || Session["TenAdmin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "QuanLy_Z9TheCoffee");
            }
            else
            {
                ViewBag.MaLoai = new SelectList(data.LoaiDoUongs.Where(a => a.TrangThai == true).ToList(), "MaLoai", "TenLoai");
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        du.NgayCapNhat = Convert.ToDateTime(collection["NgayCapNhat"]);
                        du.MaLoai = Convert.ToInt32(collection["MaLoai"]);
                        du.TenDoUong = collection["TenDoUong"];
                        du.GiaBan = Convert.ToDecimal(collection["GiaBan"]);
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/HinhSanPham"), fileName);
                        //Kiem tra hình anh ton tai chua?                                              
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                        du.AnhBia = fileName;
                        //du.AnhBia = fileName;
                        du.TrangThai = Convert.ToBoolean(collection["TrangThai"]);
                        //Luu vao CSDL
                        data.ChiTietDoUongs.InsertOnSubmit(du);
                        data.SubmitChanges();
                    }
                    return RedirectToAction("Menu", "QuanLy_Z9TheCoffee");
                }
            }
        }
    }
}