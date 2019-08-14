using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Entities
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Birthdate { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Like> Liked { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
