using System.ComponentModel.DataAnnotations;

namespace Invent_io.ViewModels.AccountVMs
{
    public class LoginVM
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
