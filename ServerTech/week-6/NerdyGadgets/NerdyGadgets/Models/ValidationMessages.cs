using System.ComponentModel.DataAnnotations;

namespace NerdyGadgets.Models
{
    public class ValidationMessages
    {
        //[Required(ErrorMessage = "{0} is verplicht!")]
        //[MaxLength(100, ErrorMessage = "{0} heeft een maximaal karakter lengte van 100!")]

        //[Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        //[MaxLength(100, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public const string RequiredMessage = "{0} is verplicht!";
        public const string MaxLenghtMessage = "{0} heeft een maximaal karakter lengte van 100!";
    }
}