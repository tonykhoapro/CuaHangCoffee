using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ChiNhanh_Z9TheCoffee.Models
{
    public class listDatBan
    {
        public int MaDatBan { get; set; }      
        public String TenChiNhanh { get; set; }
        public DateTime NgayGui { get; set; }
        public DateTime NgayDatBan { get; set; }
        public String GioDatBan { get; set; }
        public int SoLuongNguoi { get; set; }
        public String TenKhachDatBan { get; set; }
        public String DienThoaiDatBan { get; set; }
        public Boolean DaXong { get; set; }
    }
}