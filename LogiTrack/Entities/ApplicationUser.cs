using Microsoft.AspNetCore.Identity;

namespace LogiTrack.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}