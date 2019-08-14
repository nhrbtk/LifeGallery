﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public Photo Photo { get; set; }
        public UserProfile UserProfile { get; set; }
        public string Text { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }
    }
}