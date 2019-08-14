using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime PublishingDate { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Category> Tags { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
