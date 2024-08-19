using System.ComponentModel.DataAnnotations;

namespace _21_MVC_API.DTO
{
    public class UserForAuthenticationDto
    {
        
            [Required(ErrorMessage = "Username is required")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Password required")]
            public string Password { get; set; }

        

    }
}
