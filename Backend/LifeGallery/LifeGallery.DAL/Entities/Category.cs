using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Entities
{
    public class Category
    {
        public Category()
        {
            Photos = new List<Photo>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
