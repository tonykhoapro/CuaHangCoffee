using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ChiNhanh_Z9TheCoffee.Models
{
    public class listDonDatHang
    {
        public int MaDonHang { get; set; }
        public int MaNV { get; set; }  
        public DateTime NgayDat { get; set; }
        public String TenKhachHang { get; set; }
        public String TenQuan { get; set; }
        public String DiaChiGiao { get; set; }
        public Boolean DaThanhToan { get; set; }
    }
}