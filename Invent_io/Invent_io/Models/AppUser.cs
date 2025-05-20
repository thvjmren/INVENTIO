using Microsoft.AspNetCore.Identity;

namespace Invent_io.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
