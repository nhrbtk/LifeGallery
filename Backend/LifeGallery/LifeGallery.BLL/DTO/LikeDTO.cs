using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.DTO
{
    public class LikeDTO
    {
        public int Id { get; set; }
        public int PhotoId { get; set; }
        public string UserId { get; set; }
    }
}
