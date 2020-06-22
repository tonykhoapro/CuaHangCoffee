using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z9TheCoffee.Models
{
    public class listMenu
    {
        public DateTime NgayCapNhat { get; set; }
        public int MaLoai { get; set; }
        public String TenLoai { get; set; }
        public int MaDoUong { get; set; }
        public String TenDoUong { get; set; }
        public decimal GiaBan { get; set; }
        public String AnhBia { get; set; }
        public Boolean TrangThai { get; set; }
    }
}