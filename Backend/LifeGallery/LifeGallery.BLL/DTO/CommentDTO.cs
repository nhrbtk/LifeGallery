﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        [Required]
        public int PhotoId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "Comment length must be up to 500 characters.")]
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
