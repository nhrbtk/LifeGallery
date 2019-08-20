using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Entities
{
    public class Like
    {
        public int Id { get; set; }
        public Photo Photo { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
