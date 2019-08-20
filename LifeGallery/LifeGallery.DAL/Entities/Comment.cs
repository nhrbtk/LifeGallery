using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public string Text { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }
    }
}
