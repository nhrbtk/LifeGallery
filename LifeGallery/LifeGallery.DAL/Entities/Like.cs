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
        public virtual Photo Photo { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
