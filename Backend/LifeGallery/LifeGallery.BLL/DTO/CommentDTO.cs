using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public PhotoDTO Photo { get; set; }
        public UserDTO User { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
