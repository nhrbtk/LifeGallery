using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LG.BLL.DTO.Photo
{
    public class PhotoInfoModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public string Type { get; set; }
        public DateTime PublishingDate { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public ICollection<int> CategoriesIds { get; set; }
        public string UserId { get; set; }
    }
}
