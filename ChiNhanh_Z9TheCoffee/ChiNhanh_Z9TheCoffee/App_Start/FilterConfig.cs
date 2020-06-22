using System.Web;
using System.Web.Mvc;

namespace ChiNhanh_Z9TheCoffee
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
