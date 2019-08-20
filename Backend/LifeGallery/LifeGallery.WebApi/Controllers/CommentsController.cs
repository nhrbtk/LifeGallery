using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LifeGallery.WebApi.Controllers
{
    public class CommentsController : ApiController
    {
        private ICommentService CommentService => HttpContext.Current.GetOwinContext().Get<ICommentService>();

        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> AddComment([FromBody]CommentDTO commentDTO)
        {
            if (ModelState.IsValid)
            {
                if (commentDTO.UserId != User.Identity.GetUserId())
                    return Unauthorized();
                var result = await CommentService.Create(commentDTO);
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
                return BadRequest(ModelState);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("api/comments/{id}")]
        public async Task<IHttpActionResult> RemoveComment(int id)
        {
            string owner = CommentService.GetOwnerId(id);
            if (owner == null)
                return NotFound();

            if (User.IsInRole("admin") || User.IsInRole("moderator") || User.Identity.GetUserId() == owner)
            {
                var result = await CommentService.Delete(id);
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
    }
}