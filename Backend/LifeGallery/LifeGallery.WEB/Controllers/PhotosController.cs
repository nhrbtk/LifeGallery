using ImageResizer;
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

        [HttpGet]
        [Route("api/{controler}/{id}/likes")]
        public IHttpActionResult GetLikes(int id)
        {
            var result = PhotoService.GetPhotoLikes(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("api/{controler}/{id}/comments")]
        public IHttpActionResult GetComments(int id)
        {
            var result = PhotoService.GetPhotoComments(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("api/{controler}/{id}/categories")]
        public IHttpActionResult GetCategories(int id)
        {
            var result = PhotoService.GetPhotoCategories(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/{controller}/{photoId}/category/{categoryId}")]
        public IHttpActionResult AddCategoryToPhoto(int photoId, int categoryId)
        {
            var result = PhotoService.AddCategory(photoId, categoryId);
            if (result.Succedeed)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }


        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            var result = PhotoService.Delete(id);
            if (result.Succedeed)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}