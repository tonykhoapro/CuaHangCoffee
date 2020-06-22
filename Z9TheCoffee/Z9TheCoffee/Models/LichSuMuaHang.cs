using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z9TheCoffee.Models
{
    public class LichSuMuaHang
    {
        public DateTime? NgayDat { get; set; }
        public String DiaChiGiao { get; set; }
        public string TenDoUong { get; set; }
        public int? SL_Mua { get; set; }
        public decimal? DonGia { get; set; }
    }
}