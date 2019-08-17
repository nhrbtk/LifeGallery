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
    public class LikesController : ApiController
    {
        private ILikeService LikeService => HttpContext.Current.GetOwinContext().Get<ILikeService>();

        [HttpPost]
        public IHttpActionResult SetLike([FromBody]LikeDTO likeDTO)
        {
            if (ModelState.IsValid)
            {
                var result = LikeService.Create(likeDTO);
                if (result.Succedeed)
                    return Ok(result.Message);
                else
                    return BadRequest(result.Message);
            }
            else
                return BadRequest(ModelState);
        }

        [HttpDelete]
        public IHttpActionResult RemoveLike([FromBody]LikeDTO likeDTO)
        {
            if (ModelState.IsValid)
            {
                var result = LikeService.Delete(likeDTO);
                if (result.Succedeed)
                    return Ok(result.Message);
                else
                    return BadRequest(result.Message);
            }
            else
                return BadRequest(ModelState);
        }
    }
}
