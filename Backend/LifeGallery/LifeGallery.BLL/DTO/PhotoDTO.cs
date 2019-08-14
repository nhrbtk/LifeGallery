using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.DTO
{
    public class PhotoDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public string Name { get; set; }
        public byte[] File { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public DateTime PublishingDate { get; set; }
        public virtual ICollection<LikeDTO> Likes { get; set; }
        public virtual ICollection<CommentDTO> Comments { get; set; }
        public virtual ICollection<CategoryDTO> Tags { get; set; }
        public virtual UserDTO User { get; set; }
    }
}
