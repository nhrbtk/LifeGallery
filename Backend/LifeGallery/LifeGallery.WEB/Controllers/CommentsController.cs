using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LifeGallery.WEB.Controllers
{
    [Authorize]
    public class CommentsController : ApiController
    {
        private ICommentService CommentService=>HttpContext.Current.GetOwinContext().Get<ICommentService>();

        [HttpGet]
        public IHttpActionResult GetComment(int id)
        {
            var commentDTO = CommentService.Read(id);
            if (commentDTO != null)
                return Ok(commentDTO);
            else
                return NotFound();
        }

        [HttpPost]
        public IHttpActionResult AddComment([FromBody] CommentDTO commentDTO)
        {
            if (ModelState.IsValid)
            {
                var result = CommentService.Create(commentDTO);
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
                return BadRequest(ModelState);
        }

        [HttpDelete]
        public IHttpActionResult DeleteComment(int id)
        {
            var result = CommentService.Delete(id);
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
