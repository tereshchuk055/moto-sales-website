using System.ComponentModel.DataAnnotations;

namespace MotoShop.ViewModels.CustomerViewModels
{
    public class CustomerSignInViewModel
    {
        [Key]
        [Required(ErrorMessage = "{0} can't be empty!")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "{0} can't be empty!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
