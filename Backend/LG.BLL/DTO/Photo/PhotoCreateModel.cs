using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LG.BLL.DTO.Photo
{
    public class PhotoCreateModel
    {
        public PhotoCreateModel()
        {
            CategoriesIds = new List<int>();
        }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
        public string StoragePath { get; set; }
        public ICollection<int> CategoriesIds { get; set; }
        public string UserId { get; set; }
    }
}
