using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ChiNhanh_Z9TheCoffee.Models
{
    public class listNhanVien
    {
        public DateTime NgayVaoLam { get; set; }
        public String TenChiNhanh { get; set; }
        public int MaChiNhanh { get; set; }
        public int MaNV { get; set; }
        public String TenNhanVien { get; set; }
        public String NamSinh { get; set; }
        public String SDT { get; set; }
        public Boolean DaNghi { get; set; }
    }
}