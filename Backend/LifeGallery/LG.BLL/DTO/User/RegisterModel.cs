using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LG.BLL.DTO.User
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(30)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Username length must be from 6 up to 20 characters.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Use only english characters and numbers.")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,16}$", ErrorMessage = "Password must contain at least one upper and lower case letter, one number and length between 8 and 16")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
