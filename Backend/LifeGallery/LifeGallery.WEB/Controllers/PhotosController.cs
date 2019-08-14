using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LifeGallery.WEB.Controllers
{
    public class PhotosController : ApiController
    {
        private IUserService UserService => HttpContext.Current.GetOwinContext().GetUserManager<IUserService>();

        private IPhotoService PhotoService => HttpContext.Current.GetOwinContext().Get<IPhotoService>();

        private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            return Ok(PhotoService.GetFeed());
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void SavePhoto([FromBody]PhotoDTO photoDTO)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles/";
            string filename = User.Identity.Name + 5;
            photoDTO.Path = Path.Combine(path, filename);
            photoDTO.PublishingDate = DateTime.Now;
            photoDTO.User = UserService.ReadByUserName(User.Identity.Name);
            PhotoService.Create(photoDTO);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}