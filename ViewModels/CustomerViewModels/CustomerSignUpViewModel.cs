using MotoShop.Data.ValidationAttributes;
using MotoShop.Constants;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.ViewModels.CustomerViewModels
{
    public class CustomerSignUpViewModel
    {
        [Key]
        [UniqueLogin]
        [Required(ErrorMessage = "{0} can't be empty!")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "{0} can't be empty!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "{0} can't be empty!")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} can't be empty!")]
        [DataType(DataType.Text)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "{0} can't be empty!")]
        [DataType(DataType.Text)]
        public string? LastName { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        [MinLength(12, ErrorMessage = "Phone number must contain 12 characters!")]
        public string? Phone { get; set; }
    }
}
