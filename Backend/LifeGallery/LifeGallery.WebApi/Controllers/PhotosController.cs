using LifeGallery.BLL.DTO;
using LifeGallery.BLL.DTO.Photo;
using LifeGallery.BLL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace LifeGallery.WebApi.Controllers
{
    public class PhotosController : ApiController
    {
        private IPhotoService PhotoService => HttpContext.Current.GetOwinContext().Get<IPhotoService>();
        private ILikeService LikeService => HttpContext.Current.GetOwinContext().Get<ILikeService>();

        [HttpGet]
        [Route("api/photos")]
        public IHttpActionResult GetAllPhotosInfo()
        {
            return Ok(PhotoService.GetAllPhotosInfo());
        }

        [HttpGet]
        [Route("api/photos/{id}")]
        public IHttpActionResult GetPhotoInfo(int id)
        {
            var photoInfo = PhotoService.GetPhotoInfo(id);
            if (photoInfo == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(photoInfo);
            }
        }

        [HttpGet]
        [Route("api/photos/{id}/image")]
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public HttpResponseMessage GetImage(int id)
        {
            byte[] arr = PhotoService.GetImage(id);
            PhotoInfoModel photoInfo = PhotoService.GetPhotoInfo(id);
            if (arr != null && photoInfo != null)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(arr)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(photoInfo.Type);
                return response;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> SavePhoto()
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["Image"];
            List<string> allowedTypes = new List<string>()
                {
                    "image/jpeg",
                    "image/png"
                };
            if (file == null && file.ContentLength == 0)
            {
                return BadRequest("No file or file is empty.");
            }
            if (!allowedTypes.Contains(file.ContentType))
            {
                return BadRequest("Type is not allowed.");
            }

            string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles/";
            string categories = request["CategoriesIds"];
            List<int> categoriesIds = new List<int>();
            if (categories != null)
            {
                var c = categories.Split(new char[] { ' ', ',', '/' });
                foreach (var item in c)
                {
                    if (int.TryParse(item, out _)) return BadRequest("Categories Id cannot be parsed. Separate them by ' ', ',' or '/'.");
                }
                categoriesIds = c.Select(x => Convert.ToInt32(x)).ToList();
            }
            PhotoCreateModel photoCreate = new PhotoCreateModel()
            {
                StoragePath = path,
                FileName = file.FileName,
                File = new byte[file.InputStream.Length],
                Description = request["Description"],
                CategoriesIds = categoriesIds,
                IsPrivate = request["IsPrivate"] == "true" ? true : false,
                UserId = User.Identity.GetUserId()
            };
            await file.InputStream.ReadAsync(photoCreate.File, 0, photoCreate.File.Length);

            var result = await PhotoService.Create(photoCreate);
            if (!result.Succedeed)
                return BadRequest(result.Message);
            else
                return Ok();
        }

        [Authorize]
        [HttpPut]
        [Route("api/photos/{id}")]
        public async Task<IHttpActionResult> UpdatePhoto(int id, [FromBody]PhotoUpdateModel photoUpdate)
        {
            if (ModelState.IsValid)
            {
                if (id != photoUpdate.Id)
                    return BadRequest("Ids doesn't match.");
                if (PhotoService.GetOwnerId(id) != User.Identity.GetUserId())
                    return Unauthorized();
                var result = await PhotoService.UpdateInfo(photoUpdate);
                if (result.Succedeed)
                    return Ok();
                else
                    return BadRequest(result.Message);
            }
            else
                return BadRequest(ModelState);
        }

        [Authorize]
        [HttpDelete]
        [Route("api/photos/{id}")]
        public async Task<IHttpActionResult> DeletePhoto(int id)
        {
            string owner = PhotoService.GetOwnerId(id);
            if (owner == null)
                return NotFound();

            if (User.IsInRole("admin") || User.IsInRole("moderator") || User.Identity.GetUserId() == owner)
            {
                var result = await PhotoService.DeleteAsync(id);
                if (result.Succedeed)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("api/photos/{id}/categories")]
        public IHttpActionResult GetCategories(int id)
        {
            var categories = PhotoService.GetPhotoCategories(id);
            if (categories != null)
            {
                return Ok(categories);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("api/photos/{id}/comments")]
        public IHttpActionResult GetComments(int id)
        {
            var comments = PhotoService.GetPhotoComments(id);
            if (comments != null)
            {
                return Ok(comments);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/photos/{id}/likedby")]
        public IHttpActionResult GetLikers(int id)
        {
            var likers = PhotoService.GetPhotoLikers(id);
            if (likers != null)
            {
                return Ok(likers);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/photos/{id}/like")]
        public async Task<IHttpActionResult> SetLike(int id)
        {
            LikeDTO likeDTO = new LikeDTO
            {
                PhotoId = id,
                UserId = User.Identity.GetUserId()
            };
            var result = await LikeService.AddLike(likeDTO);
            if (result.Succedeed)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("api/photos/{id}/like")]
        public async Task<IHttpActionResult> RemoveLike(int id)
        {
            LikeDTO likeDTO = new LikeDTO
            {
                PhotoId = id,
                UserId = User.Identity.GetUserId()
            };
            var result = await LikeService.RemoveLike(likeDTO);
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
}