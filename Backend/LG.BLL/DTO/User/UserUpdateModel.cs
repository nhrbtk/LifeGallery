using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LG.BLL.DTO.User
{
    public class UserUpdateModel
    {
        [Required]
        public string Id { get; set; }
        [MaxLength(50, ErrorMessage = "Name must be up to 50 characters")]
        public string Name { get; set; }
        [MaxLength(200, ErrorMessage = "Bio must be up to 200 characters")]
        public string Bio { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }
        public byte[] ProfilePhoto { get; set; }
    }
}
