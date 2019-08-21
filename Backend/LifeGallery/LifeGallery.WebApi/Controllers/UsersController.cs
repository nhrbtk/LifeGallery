using LifeGallery.BLL.DTO.User;
using LifeGallery.BLL.Infrastructure;
using LifeGallery.BLL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LifeGallery.WebApi.Controllers
{
    public class UsersController : ApiController
    {
        private IUserService UserService => HttpContext.Current.GetOwinContext().GetUserManager<IUserService>();
        private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;

        [HttpPost]
        [Route("api/login")]
        public async Task<IHttpActionResult> LoginAsync([FromBody]LoginModel loginModel)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                ClaimsIdentity claim = await UserService.AuthenticateAsync(loginModel);
                if (claim == null)
                {
                    return BadRequest("User with such login and password not found");
                }
                else
                {
                    AuthenticationManager.SignOut(User.Identity.AuthenticationType);
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddDays(2)
                    }, claim);
                    return Ok("Authorized.");
                }
            }
            else
                return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost]
        [Route("api/logout")]
        public void Logout()
        {
            AuthenticationManager.SignOut(User.Identity.AuthenticationType);
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<IHttpActionResult> RegisterAsync([FromBody]RegisterModel registerModel)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                registerModel.Role = "user";
                OperationDetails operationDetails = await UserService.CreateAsync(registerModel);
                if (operationDetails.Succedeed)
                    return Created(Url.Link("DefaultApi", new { controller = "users", id = operationDetails.Property }), 0);
                else
                {
                    return BadRequest(operationDetails.Message);
                }
            }
            else
                return BadRequest(ModelState);
        }

        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(new RegisterModel
            {
                Email = "admin@admin.com",
                UserName = "admin",
                Password = "adminpassword",
                Role = "admin",
            }, new List<string> { "user", "admin", "moderator" });
        }

        [Authorize]
        [HttpGet]
        [Route("api/profile")]
        public IHttpActionResult GetCurrentProfile()
        {
            var user = UserService.Read(User.Identity.GetUserId());
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            var result = UserService.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult SearchUsers([FromUri]string username)
        {
            if (username == null)
                return BadRequest();
            var result = UserService.SearchByUsername(username);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult GetUserProfile(string id)
        {
            if (id == null)
                return BadRequest("No id.");
            var user = UserService.Read(id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/users/{id}/role")]
        public async Task<IHttpActionResult> SetRole(string id, [FromUri]string role)
        {
            if (role == null)
                return BadRequest("Role is null.");

            var result = await UserService.AddUserToRoleAsync(id, role);
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
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if ((User.IsInRole("admin") && (await UserService.IsInRole(id, "moderator") || await UserService.IsInRole(id, "user")))
                || (User.IsInRole("moderator") && await UserService.IsInRole(id, "user"))
                || User.Identity.GetUserId() == id)
            {
                var result = await UserService.DeleteAsync(id);
                if (result.Succedeed)
                    return Ok();
                else
                    return BadRequest(result.Message);
            }
            else
                return Unauthorized();
        }

        [HttpGet]
        [Route("api/users/{id}/photos")]
        public IHttpActionResult GetUserPhotos(string id)
        {
            if (id == null)
                return BadRequest("Id is null.");

            var result = UserService.GetUserPhotosInfo(id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [Authorize]
        [HttpPut]
        public IHttpActionResult UpdateUserProfile(string id, [FromBody]UserUpdateModel updateModel)
        {
            if (id == null)
                return BadRequest("Id is null.");

            if (User.Identity.GetUserId() != id)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                var result = UserService.UpdateProfile(updateModel);
                if (result.Succedeed)
                    return Ok();
                else
                    return BadRequest(result.Message);
            }
            else
                return BadRequest(ModelState);
        }


    }
}