using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.DTO.Photo
{
    public class PhotoUpdateModel
    {
        public PhotoUpdateModel()
        {
            CategoriesIds = new List<int>();
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public ICollection<int> CategoriesIds { get; set; }
    }
}
