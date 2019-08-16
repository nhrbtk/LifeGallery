using LifeGallery.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LifeGallery.WEB.Models
{
    public class PhotoModel:PhotoDTO
    {
        public PhotoDTO PhotoDTO { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}