using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LG.BLL.DTO
{
    public class LikeDTO
    {
        public int Id { get; set; }
        [Required]
        public int PhotoId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
