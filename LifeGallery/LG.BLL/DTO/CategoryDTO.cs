using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LG.BLL.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20,MinimumLength = 4, ErrorMessage ="Length must be between 4 and 20 characters.")]
        public string Name { get; set; }
    }
}
