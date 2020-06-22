using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z9TheCoffee.Models
{
    public class GioHang
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        public int iMaDoUong { set; get; }
        public String sTenDoUong { set; get; }
        public String sQuanGiao { set; get; }
        public String sAnhBia { set; get; }
        public decimal dGiaBan { set; get; }
        public int iSlMua { set; get; }
        public decimal dThanhTien
        {
            get { return iSlMua * dGiaBan; }
        }
        // khỏi tạo giỏ hàng theo MAaDoUong được truyền vào với sl mặc định là 1
        public GioHang(int MaDoUong)
        {
            iMaDoUong = MaDoUong;
            ChiTietDoUong douong = data.ChiTietDoUongs.Single(n => n.MaDoUong == iMaDoUong);
            sTenDoUong = douong.TenDoUong;
            sAnhBia = douong.AnhBia;
            dGiaBan = decimal.Parse(douong.GiaBan.ToString());
            iSlMua = 1;
        }
    }
}