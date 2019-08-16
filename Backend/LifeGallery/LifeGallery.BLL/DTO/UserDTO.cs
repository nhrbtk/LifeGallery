using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [StringLength(16, MinimumLength = 4, ErrorMessage = "UserName must contain from 4 to 16 symbols.")]
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime? Birthdate { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public ICollection<int> PhotosIds { get; set; }
        public ICollection<int> LikedIds { get; set; }
        public ICollection<int> CommentsIds { get; set; }
    }
}
