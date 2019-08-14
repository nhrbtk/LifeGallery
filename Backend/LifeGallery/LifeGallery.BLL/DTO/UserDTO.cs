using System;
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
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime? Birthdate { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public virtual ICollection<PhotoDTO> Photos { get; set; }
        public virtual ICollection<LikeDTO> Liked { get; set; }
        public virtual ICollection<CommentDTO> Comments { get; set; }
    }
}
