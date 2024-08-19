using Microsoft.AspNetCore.Identity;

namespace _21_MVC_API.Models
{
    public class User:IdentityUser
    {
        public string? FirstName {  get; set; }  
        public string? LastName { get; set; }
    }
}
