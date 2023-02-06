namespace Webshop.Models
{
    public class Product
    {
        public int Id 
        { 
            get 
            {
                return 1;
            }
        }
        public string Name 
        { 
            get
            {
                return "Bijzettafel Kyan - zwart/naturel - 60x40x25 cm";
            }
        }
        public string Description
        {
            get
            {
                return "Bijzettafel Kyan is een stoer tafeltje met een industriële en moderne look. Dit leuke tafeltje is gemaakt van mangohout en het is afgewerkt met zwart metaal. Dit geeft Kyan zijn industriële look en feel! Het tafeltje heeft een afmeting van 60x40x25 cm. Een echte eyecatcher in jouw interieur.";
            }
        }
        public decimal Price
        {
            get
            {
                return 79.99m;
            }
        }
        public int Quantity
        {
            get
            {
                return 3;
            }
        }
    }
}
