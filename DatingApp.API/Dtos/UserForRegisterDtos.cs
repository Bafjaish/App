using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDtos
    {
         // shape the data the we rec and retrun from clients 
        // here we can vadliate our request to protect sedning blank 
        [Required]
        public string  UserName { get; set; }

         [Required]
         [StringLength(8,MinimumLength=4, ErrorMessage="You must specify password 4 and 8 ")]
        public  string Password { get; set; }
    }
}