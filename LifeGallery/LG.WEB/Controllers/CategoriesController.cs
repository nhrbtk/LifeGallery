using LG.BLL.DTO;
using LG.BLL.Infrastructure;
using LG.BLL.Interfaces;
using LG.BLL.Services;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace LG.WEB.Controllers
{
    public class CategoriesController : ApiController
    {
        private ICategoryService CategoryService => HttpContext.Current.GetOwinContext().Get<ICategoryService>();

        public IHttpActionResult GetCategories()
        {
            var categories = CategoryService.GetCategories();
            if (categories != null)
                return Ok(categories);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("api/categories/{id}/photos")]
        public IHttpActionResult GetCategoriesPhotos(int id)
        {
            var photos = CategoryService.GetPhotosByCategory(id);
            if (photos != null)
                return Ok(photos);
            else
                return NotFound();
        }

        [Authorize(Roles="admin, moderator")]
        [HttpPost]
        public async Task<IHttpActionResult> AddCategory([FromBody]CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await CategoryService.Create(categoryDTO);
                if(result.Succedeed)
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

        [Authorize(Roles = "admin, moderator")]
        [HttpDelete]
        [Route("api/categories/{id}")]
        public async Task<IHttpActionResult> AddCategory(int id)
        {
            var result = await CategoryService.Delete(id);
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
