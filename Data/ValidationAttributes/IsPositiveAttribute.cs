using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MotoShop.Data.ValidationAttributes
{
    public class IsPositiveAttribute : ValidationAttribute
    {
        public IsPositiveAttribute() : base("This field can't contain negative numbers!") { }

        public override bool IsValid(object? value)
        {
            return double.TryParse(value?.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result) && result >= 0;
        }
    }
}
