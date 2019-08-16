using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
        public ICollection<int> LikesIds { get; set; }
        public ICollection<int> CommentsIds { get; set; }
        public ICollection<int> CategoriesIds { get; set; }
        public string UserId { get; set; }
    }
}
