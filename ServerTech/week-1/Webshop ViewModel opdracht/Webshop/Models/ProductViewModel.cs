using System.Globalization;

namespace Webshop.Models
{
    public class ProductViewModel
    {
        public string FormatPrice(decimal Price)
        {
            var ri = new RegionInfo(System.Threading.Thread.CurrentThread.CurrentUICulture.LCID);
            return ri.ISOCurrencySymbol + Price;
        }
    }
}