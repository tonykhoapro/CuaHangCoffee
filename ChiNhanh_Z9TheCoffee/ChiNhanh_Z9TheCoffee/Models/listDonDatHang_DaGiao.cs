using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ChiNhanh_Z9TheCoffee.Models
{
    public class listDonDatHang_DaGiao
    {
        public int MaDonHang { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime GioDat { get; set; }
        public int MaNV { get; set; }
        public String TenNhanVienGiao { get; set; }
        public String TenKhachHang { get; set; }
        public String TenQuan { get; set; }
        public String DiaChiGiao { get; set; }
        public String DienThoai_KH { get; set; }
        public int SlMua { get; set; }
        public decimal DonGia { set; get; }
        public decimal ThanhTien
        {
            get { return SlMua * DonGia; }
        }

        public Boolean DaThanhToan { get; set; }
    }
}