using MotoShop.Repository;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Data.ValidationAttributes
{
    public class UniqueVIN : ValidationAttribute
    {
        public UniqueVIN() : base("Such VIN already exists! Please, try another one.") { }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var repository = (MotorcycleRepository)validationContext.GetService(typeof(MotorcycleRepository));
            string param = value as string ?? "";

            if ((param != "") && !repository.IsVinExist(param))
                return ValidationResult.Success;
            else
                return new ValidationResult("Such VIN already exists! Please, try another one.");
        }
    }
}
