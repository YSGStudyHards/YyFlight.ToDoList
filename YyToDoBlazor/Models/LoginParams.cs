using System.ComponentModel.DataAnnotations;

namespace YyToDoBlazor.Models
{
    public class LoginParams
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
