using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using ChiNhanh_Z9TheCoffee.Models;
using System.Collections;
using System.Web.Helpers;

namespace ChiNhanh_Z9TheCoffee.Controllers
{
    public class ChiNhanh_Z9TheCoffeeController : Controller
    {
        // GET: NguoiDung    
        DataClasses1DataContext data = new DataClasses1DataContext();
        public ActionResult DangNhap(FormCollection collection)
        {
            var sdt = collection["SDT"];
            if (String.IsNullOrEmpty(sdt))
            {
                ViewData["Loi1"] = "* Bắt buộc";
            }
            else
            {
                QuanLy NV = data.QuanLies.SingleOrDefault(n => n.SDT == sdt);
                if (NV != null)
                {
                    Session["SDT"] = NV.SDT;
                    Session["MaNV"] = NV.MaNV;
                    Session["TenNhanVien"] = NV.TenNhanVien;
                    Session["MaChiNhanh"] = NV.MaChiNhanh;
                    var machinhanh = NV.MaChiNhanh;
                    ChiNhanh CN = data.ChiNhanhs.SingleOrDefault(n => n.MaChiNhanh == machinhanh);
                    Session["TenChiNhanh"] = CN.TenChiNhanh;
                    Session["NamSinh"] = NV.NamSinh;
                    return RedirectToAction("Index", "ChiNhanh_Z9TheCoffee");
                }else{ViewBag.ThongBao = "Số Điện Thoại không tồn tại";}
            }
            return View();
        }
        public ActionResult Index()
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            else  {   return View(); }
        }
        private decimal TongTien(int iddonhang)
        {
            decimal TongTien = 0;
            List<listChiTietDonDatHang> CTDDH = listCTDDH(iddonhang);
            if (CTDDH != null)
            {
                TongTien = CTDDH.Sum(n => n.ThanhTien);
            }
            return TongTien;
        }
        private int TongSoLuong(int iddonhang)
        {
            int TongSoLuong = 0;
            List<listChiTietDonDatHang> CTDDH = listCTDDH(iddonhang);
            if (CTDDH != null)
            {
                TongSoLuong = CTDDH.Sum(n => n.SlMua);
            }
            return TongSoLuong;
        }
        // đăng xuất
        public ActionResult DangXuat()
        {
            Session["SDT"] = null;
            return RedirectToAction("Index", "ChiNhanh_Z9TheCoffee");
        }
        public List<listDonDatHang> listDDH(int id)
        {
            var listDDH = from ddh in data.DonDatHangs
                          from kh in data.KhachHangs
                          from qg in data.QuanGiaos
                          from cn in data.ChiNhanhs
                          from nv in data.QuanLies
                          where ddh.MaKH == kh.MaKH && qg.MaQuan == ddh.MaQuan && cn.MaChiNhanh == qg.MaChiNhanh && ddh.DaThanhToan == false && cn.MaChiNhanh == id
                          group new { ddh, kh, qg, nv } by new
                          {
                              ddh.DaThanhToan,
                              ddh.MaDonHang,
                              ddh.NgayDat,
                              kh.TenKH,
                              qg.TenQuan,
                              ddh.DiaChiGiao
                          }
                          into a
                          select new listDonDatHang()
                          {
                              DaThanhToan = Convert.ToBoolean(a.Key.DaThanhToan),                           
                              MaDonHang = a.Key.MaDonHang,
                              NgayDat = Convert.ToDateTime(a.Key.NgayDat),
                              TenKhachHang = a.Key.TenKH,
                              TenQuan = a.Key.TenQuan,
                              DiaChiGiao = a.Key.DiaChiGiao
                          };
            return listDDH.OrderBy(a => a.NgayDat).ToList();
        }
        public ActionResult HienThiDonDatHang(int id, int? page)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            var layddh = listDDH(id);
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            return View(layddh.ToPagedList(pageNumber, pageSize));
        }
        public List<listChiTietDonDatHang> listCTDDH(int iddonhang)
        {
            var listCTDDH = from ddh in data.DonDatHangs
                            from kh in data.KhachHangs
                            from qg in data.QuanGiaos
                            from cn in data.ChiNhanhs
                            from ctdh in data.ChiTietDonHangs
                            from ctdu in data.ChiTietDoUongs
                            where ddh.MaKH == kh.MaKH && qg.MaQuan == ddh.MaQuan && cn.MaChiNhanh == qg.MaChiNhanh && ddh.MaDonHang == ctdh.MaDonHang && ctdu.MaDoUong == ctdh.MaDoUong && ddh.MaDonHang == iddonhang
                            group new { ddh, kh, qg, cn, ctdh, ctdu } by new
                            {
                                ddh.MaDonHang,
                                ddh.NgayDat,
                                ddh.GioDat,
                                kh.TenKH,
                                qg.TenQuan,
                                ddh.DiaChiGiao,
                                ddh.DienThoai_KH,
                                ctdu.TenDoUong,
                                ctdh.SlMua,
                                ctdh.DonGia
                            }
                          into a
                            select new listChiTietDonDatHang()
                            {
                                MaDonHang = a.Key.MaDonHang,
                                NgayDat = Convert.ToDateTime(a.Key.NgayDat),
                                GioDat = Convert.ToDateTime(a.Key.GioDat),
                                TenKhachHang = a.Key.TenKH,
                                TenQuan = a.Key.TenQuan,
                                DiaChiGiao = a.Key.DiaChiGiao,
                                DienThoai_KH = a.Key.DienThoai_KH,
                                TenDoUong = a.Key.TenDoUong,
                                SlMua = Convert.ToInt32(a.Key.SlMua),
                                DonGia = Convert.ToDecimal(a.Key.DonGia)
                            };
            return listCTDDH.OrderBy(a => a.NgayDat).ToList();
        }
        public ActionResult HienThiChiTietDonDatHang(int iddonhang, int? page)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            ViewBag.TongTien = TongTien(iddonhang);
            ViewBag.TongSoLuongMua = TongSoLuong(iddonhang);
            var layctddh = listCTDDH(iddonhang);
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            return View(layctddh.ToPagedList(pageNumber, pageSize));
        }
        // list cập nhật
        public List<listCapNhatDonDatHang> listCNDDH(int id)
        {
            var listCNDDH = from ddh in data.DonDatHangs
                            from kh in data.KhachHangs
                            from qg in data.QuanGiaos
                            from cn in data.ChiNhanhs
                            from nv in data.QuanLies
                            where ddh.MaKH == kh.MaKH && qg.MaQuan == ddh.MaQuan && cn.MaChiNhanh == qg.MaChiNhanh && ddh.MaDonHang == id
                            group new { ddh, kh, qg, nv } by new
                            {
                                ddh.DaThanhToan,                               
                                ddh.MaDonHang,
                                ddh.NgayDat,
                                kh.TenKH,
                                qg.TenQuan,
                                ddh.DiaChiGiao
                            }
                          into a
                            select new listCapNhatDonDatHang()
                            {
                                DaThanhToan = Convert.ToBoolean(a.Key.DaThanhToan),                               
                                MaDonHang = a.Key.MaDonHang,
                                NgayDat = Convert.ToDateTime(a.Key.NgayDat),
                                TenKhachHang = a.Key.TenKH,
                                TenQuan = a.Key.TenQuan,
                                DiaChiGiao = a.Key.DiaChiGiao
                            };
            return listCNDDH.ToList();
        }
        //Cập nhật đơn hàng
        [HttpGet]
        public ActionResult CapNhatDonDatHang(int id)
        {
            int machinhanh = Convert.ToInt32(Session["MaChiNhanh"]);
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            else
            { //Lay ra doi tuong sach theo ma                                
                var cnddh = listCNDDH(id);
                ViewBag.TenNhanVien = new SelectList(data.QuanLies.Where(a => a.MaChiNhanh == machinhanh).Where(a => a.DaNghi == false).ToList(), "MaNV", "TenNhanVien");
                return View(cnddh);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CapNhatDonDatHang(int id, FormCollection collection, DonDatHang ddh)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            else
            {
                //Them vao CSDL               
                if (ModelState.IsValid)
                {                    
                    try
                    {
                        ddh = data.DonDatHangs.SingleOrDefault(n => n.MaDonHang == id);
                        ddh.MaNV = Convert.ToInt32(collection["MaNV"]);
                        ddh.DaThanhToan = Convert.ToBoolean(collection["DaThanhToan"]);
                        //Luu vao CSDL     
                        data.SubmitChanges();
                    }
                    catch { ViewBag.Loi = "Vui lòng kiểm tra đầy đủ thông tin"; }
                }
                return RedirectToAction("HienThiDonDatHang", "ChiNhanh_Z9TheCoffee", new { id = Session["MaChiNhanh"] });
            }
        }
        public List<listDonDatHang_DaGiao> listDDH_DaGiao(int id)
        {
            var listDDH_DaGiao = from ddh in data.DonDatHangs
                                 join ql in data.QuanLies
                                 on ddh.MaNV equals ql.MaNV into a
                                 join kh in data.KhachHangs
                                 on ddh.MaKH equals kh.MaKH into b
                                 join qg in data.QuanGiaos
                                 on ddh.MaQuan equals qg.MaQuan into c
                                 join cn in data.ChiNhanhs
                                 on ddh.QuanGiao.MaChiNhanh equals cn.MaChiNhanh into d
                                 from cn in d.DefaultIfEmpty()
                                 from qg in c.DefaultIfEmpty()
                                 from kh in b.DefaultIfEmpty()
                                 from ql in a.DefaultIfEmpty()
                                 where ddh.DaThanhToan == true && cn.MaChiNhanh == id
                                 select new listDonDatHang_DaGiao()
                                 {
                                     MaDonHang = ddh.MaDonHang,
                                     NgayDat = Convert.ToDateTime(ddh.NgayDat),
                                     GioDat = Convert.ToDateTime(ddh.GioDat),
                                     TenKhachHang = kh.TenKH,
                                     TenQuan = qg.TenQuan,
                                     DiaChiGiao = ddh.DiaChiGiao,
                                     DienThoai_KH = ddh.DienThoai_KH,
                                     TenNhanVienGiao = ql.TenNhanVien,
                                     DaThanhToan = Convert.ToBoolean(ddh.DaThanhToan)
                                 };
            return listDDH_DaGiao.OrderByDescending(a => a.NgayDat).ToList();
        }
        public ActionResult HienThiDonDatHang_DaGiao(int id, int? page)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            var layddh_dagiao = listDDH_DaGiao(id);
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            return View(layddh_dagiao.ToPagedList(pageNumber, pageSize));
        }
        public List<listDatBan> listDB(int id)
        {
            var listDB = from db in data.DatBans
                         from cn in data.ChiNhanhs
                         where db.MaChiNhanh == cn.MaChiNhanh && db.MaChiNhanh == id && db.DaXong == false
                         group new { db, cn } by new
                         {
                             db.MaDatBan,
                             cn.TenChiNhanh,
                             db.NgayGui,
                             db.NgayDatBan,
                             db.GioDatBan,
                             db.SoLuongNguoi,
                             db.TenKhachDatBan,
                             db.DienThoaiDatBan,
                         }
                          into d
                         select new listDatBan()
                         {
                             MaDatBan = d.Key.MaDatBan,
                             TenChiNhanh = d.Key.TenChiNhanh,
                             NgayGui = Convert.ToDateTime(d.Key.NgayGui),
                             NgayDatBan = Convert.ToDateTime(d.Key.NgayDatBan),
                             GioDatBan = d.Key.GioDatBan,
                             SoLuongNguoi = Convert.ToInt32(d.Key.SoLuongNguoi),
                             TenKhachDatBan = d.Key.TenKhachDatBan,
                             DienThoaiDatBan = d.Key.DienThoaiDatBan,
                         };
            return listDB.OrderBy(a => a.NgayDatBan).ToList();
        }
        public ActionResult HienThiDonDatBan(int id, int? page)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            var laydb = listDB(id);
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            return View(laydb.ToPagedList(pageNumber, pageSize));
        }
        public List<listCapNhatDonDatBan> listCNDDB(int id)
        {
            var listCNDDB = from db in data.DatBans
                            from cn in data.ChiNhanhs
                            where cn.MaChiNhanh == db.MaChiNhanh && db.MaDatBan == id
                            group new { db, cn } by new
                            {
                                db.MaDatBan,
                                cn.TenChiNhanh,
                                db.NgayGui,
                                db.NgayDatBan,
                                db.GioDatBan,
                                db.SoLuongNguoi,
                                db.TenKhachDatBan,
                                db.DienThoaiDatBan,
                                db.DaXong
                            }
                          into d
                            select new listCapNhatDonDatBan()
                            {
                                MaDatBan = d.Key.MaDatBan,
                                TenChiNhanh = d.Key.TenChiNhanh,
                                NgayGui = Convert.ToDateTime(d.Key.NgayGui),
                                NgayDatBan = Convert.ToDateTime(d.Key.NgayDatBan),
                                GioDatBan = d.Key.GioDatBan,
                                SoLuongNguoi = Convert.ToInt32(d.Key.SoLuongNguoi),
                                TenKhachDatBan = d.Key.TenKhachDatBan,
                                DienThoaiDatBan = d.Key.DienThoaiDatBan,
                                DaXong = Convert.ToBoolean(d.Key.DaXong)
                            };
            return listCNDDB.ToList();
        }
        [HttpGet]
        public ActionResult CapNhatDonDatBan(int id)
        {
            int machinhanh = Convert.ToInt32(Session["MaChiNhanh"]);
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            else
            { //Lay ra doi tuong sach theo ma                                              
                var cnddb = listCNDDB(id);
                return View(cnddb);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CapNhatDonDatBan(int id, FormCollection collection, DatBan db)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            else
            {
                //Them vao CSDL               
                if (ModelState.IsValid)
                {                    
                    try
                    {
                        db = data.DatBans.SingleOrDefault(n => n.MaDatBan == id);
                        db.DaXong = Convert.ToBoolean(collection["DaXong"]);
                        //Luu vao CSDL     
                        data.SubmitChanges();
                    }
                    catch { ViewBag.Loi = "Vui lòng kiểm tra đầy đủ thông tin"; }
                }
                return RedirectToAction("HienThiDonDatBan", "ChiNhanh_Z9TheCoffee", new { id = Session["MaChiNhanh"] });
            }
        }
        public List<listDatBan> listDB_DaXong(int id)
        {
            var listDB_DaXong = from db in data.DatBans
                                from cn in data.ChiNhanhs
                                where db.MaChiNhanh == cn.MaChiNhanh && db.MaChiNhanh == id && db.DaXong == true
                                group new { db, cn } by new
                                {
                                    db.MaDatBan,
                                    cn.TenChiNhanh,
                                    db.NgayGui,
                                    db.NgayDatBan,
                                    db.GioDatBan,
                                    db.SoLuongNguoi,
                                    db.TenKhachDatBan,
                                    db.DienThoaiDatBan,
                                    db.DaXong
                                }
                             into d
                                select new listDatBan()
                                {
                                    MaDatBan = d.Key.MaDatBan,
                                    TenChiNhanh = d.Key.TenChiNhanh,
                                    NgayGui = Convert.ToDateTime(d.Key.NgayGui),
                                    NgayDatBan = Convert.ToDateTime(d.Key.NgayDatBan),
                                    GioDatBan = d.Key.GioDatBan,
                                    SoLuongNguoi = Convert.ToInt32(d.Key.SoLuongNguoi),
                                    TenKhachDatBan = d.Key.TenKhachDatBan,
                                    DienThoaiDatBan = d.Key.DienThoaiDatBan,
                                    DaXong = Convert.ToBoolean(d.Key.DaXong)
                                };
            return listDB_DaXong.OrderBy(a => a.NgayDatBan).ToList();
        }
        public ActionResult HienThiDonDatBan_DaXong(int id, int? page)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            var laydb_daxong = listDB_DaXong(id);
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            return View(laydb_daxong.ToPagedList(pageNumber, pageSize));
        }
        public List<listNhanVien> listQuanLy_NhanVien(int id)
        {
            var listQuanLy_NhanVien = from ql in data.QuanLies
                                      from cn in data.ChiNhanhs
                                      where ql.MaChiNhanh == cn.MaChiNhanh && ql.MaChiNhanh == id
                                      group new { ql, cn } by new
                                      {
                                          ql.NgayVaoLam,
                                          ql.MaNV,
                                          cn.TenChiNhanh,
                                          ql.TenNhanVien,
                                          ql.NamSinh,
                                          ql.SDT,
                                          ql.DaNghi
                                      }
                                        into d
                                      select new listNhanVien()
                                      {
                                          NgayVaoLam = Convert.ToDateTime(d.Key.NgayVaoLam),
                                          MaNV = d.Key.MaNV,
                                          TenChiNhanh = d.Key.TenChiNhanh,
                                          TenNhanVien = d.Key.TenNhanVien,
                                          NamSinh = d.Key.NamSinh,
                                          SDT = d.Key.SDT,
                                          DaNghi = Convert.ToBoolean(d.Key.DaNghi)
                                      };
            return listQuanLy_NhanVien.OrderBy(a => a.NgayVaoLam).ToList();
        }
        public ActionResult NhanVien(int id, int? page)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            Session["MaNV"] = id;
            var laydanhsach = listQuanLy_NhanVien(id);
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(laydanhsach.ToPagedList(pageNumber, pageSize));
        }
        public List<listNhanVien> listCapNhat_NhanVien(int id)
        {
            var listCapNhat_NhanVien = from ql in data.QuanLies
                                       from cn in data.ChiNhanhs
                                       where ql.MaChiNhanh == cn.MaChiNhanh && ql.MaNV == id
                                       group new { ql, cn } by new
                                       {
                                           ql.NgayVaoLam,
                                           ql.MaNV,
                                           cn.TenChiNhanh,
                                           ql.TenNhanVien,
                                           ql.NamSinh,
                                           ql.SDT,
                                           ql.DaNghi
                                       }
                                        into d
                                       select new listNhanVien()
                                       {
                                           NgayVaoLam = Convert.ToDateTime(d.Key.NgayVaoLam),
                                           MaNV = d.Key.MaNV,
                                           TenChiNhanh = d.Key.TenChiNhanh,
                                           TenNhanVien = d.Key.TenNhanVien,
                                           NamSinh = d.Key.NamSinh,
                                           SDT = d.Key.SDT,
                                           DaNghi = Convert.ToBoolean(d.Key.DaNghi)
                                       };
            return listCapNhat_NhanVien.OrderBy(a => a.NgayVaoLam).ToList();
        }
        public ActionResult CapNhatThongTinNhanVien(int id)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            else
            { //Lay ra doi tuong sach theo ma       
                QuanLy ql = data.QuanLies.SingleOrDefault(n => n.MaNV == id);
                return View(ql);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CapNhatThongTinNhanVien(int id, FormCollection collection, QuanLy ql)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            else
            {
                //Them vao CSDL               
                if (ModelState.IsValid)
                {
                    try
                    {
                        ql = data.QuanLies.SingleOrDefault(n => n.MaNV == id);
                        ql.NgayVaoLam = Convert.ToDateTime(collection["NgayVaoLam"]);
                        ql.TenNhanVien = collection["TenNhanVien"];
                        ql.NamSinh = collection["NamSinh"];
                        ql.SDT = collection["SDT"];
                        ql.DaNghi = Convert.ToBoolean(collection["DaNghi"]);
                        //Luu vao CSDL                             
                        data.SubmitChanges();
                    }
                    catch { ViewBag.Loi = "Vui lòng kiểm tra đầy đủ thông tin"; }
                }
                return RedirectToAction("NhanVien", "ChiNhanh_Z9TheCoffee", new { id = Session["MaChiNhanh"] });
            }
        }
        public ActionResult ThemNhanVien()
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemNhanVien(FormCollection collection, QuanLy ql)
        {
            try
            {
                ql.NgayVaoLam = Convert.ToDateTime(collection["NgayVaoLam"]);
                ql.MaChiNhanh = Convert.ToInt32(Session["MaChiNhanh"]);
                ql.TenNhanVien = collection["TenNhanVien"];
                ql.NamSinh = collection["NamSinh"];
                ql.SDT = collection["SDT"];
                ql.DaNghi = false;
                data.QuanLies.InsertOnSubmit(ql);
                data.SubmitChanges();
                return RedirectToAction("NhanVien", "ChiNhanh_Z9TheCoffee", new { id = Session["MaChiNhanh"] });
            }
            catch { return RedirectToAction("NhanVien", "ChiNhanh_Z9TheCoffee", new { id = Session["MaChiNhanh"] }); }
        }
        public List<listDonDatHang> listLocDDH(int id,DateTime TuNgay, DateTime DenNgay)
        {
            var listLocDDH = from ddh in data.DonDatHangs
                          from kh in data.KhachHangs
                          from qg in data.QuanGiaos
                          from cn in data.ChiNhanhs
                          from nv in data.QuanLies
                          where ddh.MaKH == kh.MaKH && qg.MaQuan == ddh.MaQuan && cn.MaChiNhanh == qg.MaChiNhanh && ddh.DaThanhToan == false 
                          && cn.MaChiNhanh == id && ddh.NgayDat >= TuNgay && ddh.NgayDat <= DenNgay
                             group new { ddh, kh, qg, nv } by new
                          {
                              ddh.DaThanhToan,
                              ddh.MaDonHang,
                              ddh.NgayDat,
                              kh.TenKH,
                              qg.TenQuan,
                              ddh.DiaChiGiao
                          }
                          into a
                          select new listDonDatHang()
                          {
                              DaThanhToan = Convert.ToBoolean(a.Key.DaThanhToan),                             
                              MaDonHang = a.Key.MaDonHang,
                              NgayDat = Convert.ToDateTime(a.Key.NgayDat),
                              TenKhachHang = a.Key.TenKH,
                              TenQuan = a.Key.TenQuan,
                              DiaChiGiao = a.Key.DiaChiGiao
                          };
          
            return listLocDDH.OrderBy(a => a.NgayDat).ToList();
        }
        public ActionResult LocDonDatHang(int id, int? page, FormCollection collection)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            id = Convert.ToInt32(Session["MaChiNhanh"]);
            DateTime TuNgay = Convert.ToDateTime(collection["TuNgay"]);
            DateTime DenNgay = Convert.ToDateTime(collection["DenNgay"]).AddDays(1);
            //TuNgay = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            //DenNgay = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;
            var locddh = listLocDDH(id, TuNgay, DenNgay);          
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            return View(locddh.ToPagedList(pageNumber, pageSize));
        }
        public List<listDemDonDatHang> listThongKe_DatHang_DaXong(int id)
        {
            var listThongKe_DatHang_DaXong = from dh in data.DonDatHangs
                                             from cn in data.ChiNhanhs
                                             from qg in data.QuanGiaos
                                             where dh.MaQuan == qg.MaQuan && qg.MaChiNhanh == cn.MaChiNhanh
                                             && qg.MaChiNhanh == id && dh.DaThanhToan == true  
                                             group new { dh, cn, qg } by new
                                             {
                                                 layThang = dh.NgayDat.Value.Month,                                              
                                             }
                             into d
                                             select new listDemDonDatHang()
                                             {
                                                 Thang = d.Key.layThang.ToString(),                                               
                                                 DemSoLuong = d.Count(),
                                             };

            return listThongKe_DatHang_DaXong.ToList();
        }
        public ActionResult HienThiThongKeDatHang(int id)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            var laythongkedathang = listThongKe_DatHang_DaXong(id);          
            return View(laythongkedathang);
        }
        public List<listDemDonDatHang> listLoc_ThongKe_DatHang_DaXong(int id, int Nam)
        {
            var listLoc_ThongKe_DatHang_DaXong = from dh in data.DonDatHangs
                                             from cn in data.ChiNhanhs
                                             from qg in data.QuanGiaos
                                             where dh.MaQuan == qg.MaQuan && qg.MaChiNhanh == cn.MaChiNhanh
                                             && qg.MaChiNhanh == id && dh.DaThanhToan == true && dh.NgayDat.Value.Year == Nam
                                             group new { dh, cn, qg } by new
                                             {
                                                 layThang = dh.NgayDat.Value.Month,
                                             }
                             into d
                                             select new listDemDonDatHang()
                                             {
                                                 Thang = d.Key.layThang.ToString(),
                                                 DemSoLuong = d.Count(),
                                             };

            return listLoc_ThongKe_DatHang_DaXong.ToList();
        }
        public ActionResult LocThongKeDatHang(int id, int Nam)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            var laythongkedathang = listLoc_ThongKe_DatHang_DaXong(id, Nam);
            return View(laythongkedathang);
        }
        public List<listDemDonDatHang> listThongKe_DatBan_DaXong(int id)
        {
            var listThongKe_DatBan_DaXong = from db in data.DatBans
                                             from cn in data.ChiNhanhs                                           
                                             where  db.MaChiNhanh == cn.MaChiNhanh
                                             && db.MaChiNhanh == id && db.DaXong == true
                                             group new { db, cn } by new
                                             {
                                                 layThang = db.NgayDatBan.Value.Month,
                                             }
                             into d
                                             select new listDemDonDatHang()
                                             {
                                                 Thang = d.Key.layThang.ToString(),
                                                 DemSoLuong = d.Count(),
                                             };

            return listThongKe_DatBan_DaXong.ToList();
        }
        public ActionResult HienThiThongKeDatBan(int id)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            var laythongkedatban = listThongKe_DatBan_DaXong(id);
            return View(laythongkedatban);
        }
        public List<listDemDonDatHang> listLoc_ThongKe_DatBan_DaXong(int id, int Nam)
        {
            var listLoc_ThongKe_DatBan_DaXong = from db in data.DatBans
                                            from cn in data.ChiNhanhs
                                            where db.MaChiNhanh == cn.MaChiNhanh
                                            && db.MaChiNhanh == id && db.DaXong == true && db.NgayDatBan.Value.Year == Nam
                                            group new { db, cn } by new
                                            {
                                                layThang = db.NgayDatBan.Value.Month,
                                            }
                             into d
                                            select new listDemDonDatHang()
                                            {
                                                Thang = d.Key.layThang.ToString(),
                                                DemSoLuong = d.Count(),
                                            };

            return listLoc_ThongKe_DatBan_DaXong.ToList();
        }
        public ActionResult LocThongKeDatBan(int id, int Nam)
        {
            if (Session["SDT"] == null || Session["SDT"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "ChiNhanh_Z9TheCoffee");
            }
            var locthongkedatban = listLoc_ThongKe_DatBan_DaXong(id, Nam);
            return View(locthongkedatban);
        }
    }
}