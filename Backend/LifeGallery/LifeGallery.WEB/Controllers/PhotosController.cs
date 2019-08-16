using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Interfaces;
using LifeGallery.BLL.Services;
using LifeGallery.WEB.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LifeGallery.WEB.Controllers
{
    public class PhotosController : ApiController
    {
        private IUserService UserService => HttpContext.Current.GetOwinContext().GetUserManager<IUserService>();

        private IPhotoService PhotoService => HttpContext.Current.GetOwinContext().Get<IPhotoService>();
        private ILikeService LikeService => HttpContext.Current.GetOwinContext().Get<ILikeService>();
        private ICommentService CommentService => HttpContext.Current.GetOwinContext().Get<ICommentService>();

        private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            return Ok(PhotoService.GetFeed());
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            PhotoDTO photoDTO = PhotoService.GetPhoto(id);
            MemoryStream ms = new MemoryStream(photoDTO.File);
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            responseMessage.Content = new StreamContent(ms);
            responseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(photoDTO.Type);
            responseMessage.Headers.Add("Description", photoDTO.Description);
            responseMessage.Headers.Add("PublishingDate", photoDTO.PublishingDate.ToString());
            responseMessage.Headers.Add("IsPrivate", photoDTO.IsPrivate.ToString());
            responseMessage.Headers.Add("Name", photoDTO.Name);
            responseMessage.Headers.Add("UserId", photoDTO.UserId);
            responseMessage.Headers.Add("Description", photoDTO.Description);
            responseMessage.StatusCode = HttpStatusCode.OK;
            return responseMessage;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IHttpActionResult> SavePhoto()
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["Image"];

            if (file != null && file.ContentLength > 0)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles/";
                string filename = User.Identity.Name + '_' + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(file.FileName);

                PhotoDTO photoDTO = new PhotoDTO();
                photoDTO.Path = Path.Combine(path, filename);
                photoDTO.Type = file.ContentType;
                photoDTO.PublishingDate = DateTime.Now;
                photoDTO.File = new byte[file.InputStream.Length];
                await file.InputStream.ReadAsync(photoDTO.File, 0, photoDTO.File.Length);
                photoDTO.Description = request["Description"];
                photoDTO.UserId = UserService.ReadByUserName(User.Identity.Name).Id;
                var result = await PhotoService.Create(photoDTO);
                if (!result.Succedeed)
                    return BadRequest();
                else
                    return Ok();
            }
            else
                return BadRequest();

        }

        // PUT api/<controller>/5
        public async Task<IHttpActionResult> Put(int id, [FromBody]PhotoDTO photoDTO)
        {
            if (id != photoDTO.Id)
            {
                return BadRequest();
            }
            else
            {
                var result = await PhotoService.UpdateInfo(photoDTO);
                if (result.Succedeed)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/{controler}/{id}/like")]
        public IHttpActionResult Like(int id)
        {
            var result = LikeService.Create(new LikeDTO { UserId = UserService.ReadByUserName(User.Identity.Name).Id, PhotoId = id });
            if (!result.Succedeed)
                return BadRequest(result.Message);
            else
                return Ok(result.Message);
        }

        [HttpDelete]
        [Authorize]
        [Route("api/{controler}/{id}/like")]
        public IHttpActionResult Unlike(int id)
        {
            var result = LikeService.Delete(new LikeDTO { UserId = UserService.ReadByUserName(User.Identity.Name).Id, PhotoId = id });
            if (!result.Succedeed)
                return BadRequest(result.Message);
            else
                return Ok(result.Message);
        }

        [HttpPost]
        [Authorize]
        [Route("api/{controler}/{id}/comment")]
        public IHttpActionResult AddComment(int id, [FromBody]CommentDTO commentDTO)
        {
            if (commentDTO == null)
                return BadRequest("Object is null");
            commentDTO.Date = DateTime.Now;
            commentDTO.PhotoId = id;
            commentDTO.UserId = UserService.ReadByUserName(User.Identity.Name).Id;
            var result = CommentService.Create(commentDTO);
            if (!result.Succedeed)
                return BadRequest(result.Message);
            else
                return Ok(result.Message);
        }

        [HttpDelete]
        [Authorize]
        [Route("api/{controler}/{id}/comment")]
        public IHttpActionResult DeleteComment(int id)
        {
            throw new NotImplementedException();
        }


        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}