using System.Globalization;

namespace Webshop.Models
{
    public class ProductViewModel
    {
        private Product ProductItem { get; set; } = new();
        public string Name { get; set; }
        public string Description { get; set; }

        public ProductViewModel()
        {
            Name = ProductItem.Name;
            Description = ProductItem.Description;
        }

        public string FormattedPrice()
        {
            var culture = new CultureInfo("nl-NL");
            culture.NumberFormat.CurrencyPositivePattern = 0;
            culture.NumberFormat.CurrencyNegativePattern = 2;
            culture.NumberFormat.CurrencyDecimalSeparator = CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator;

            return ProductItem.Price.ToString("C", culture);
        }
    }
}