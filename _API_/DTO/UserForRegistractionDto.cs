using System.ComponentModel.DataAnnotations;

namespace _21_MVC_API.DTO
{
    public class UserForRegistractionDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }


        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        //Role kontrolü gerçekleştirmk istersek kullanabilriiz.
        //pıblic string Roles{get;set;}
        //public IColection<string>?Roles{get;set;}
    }
}
