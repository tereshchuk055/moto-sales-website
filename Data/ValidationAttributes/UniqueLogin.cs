using MotoShop.Repository;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Data.ValidationAttributes
{
    public class UniqueLogin : ValidationAttribute
    {
        public UniqueLogin() : base("Such Login already exists! Please, try another one.") { }
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var repository = (CustomerRepository)validationContext.GetService(typeof(CustomerRepository));
            string param = value as string ?? "";

            if ((param != "") && !repository.IsLoginExist(param))
                return ValidationResult.Success;
            else
                return new ValidationResult("Such Login already exists! Please, try another one.");
        }
    }
}
