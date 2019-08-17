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
using System.Web.UI.WebControls;

namespace LifeGallery.WEB.Controllers
{
    public class CategoriesController : ApiController
    {
        private ICategoryService CategoryService => HttpContext.Current.GetOwinContext().Get<ICategoryService>();

        [HttpPost]
        [Authorize(Roles = "moderator")]
        public IHttpActionResult CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                var result = CategoryService.Create(categoryDTO);
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

        [HttpDelete]
        [Authorize(Roles = "moderator")]
        public IHttpActionResult DeleteCategory(int id)
        {
            var result = CategoryService.Delete(id);
            if (result.Succedeed)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Message);
            }

        }

        [HttpGet]
        [Route("api/{controller}/{id}/photos")]
        public IHttpActionResult GetPhotos(int id)
        {
            var photoDTOs = CategoryService.GetCategoryPhotos(id);
            if (photoDTOs != null)
            {
                return Ok(photoDTOs);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
