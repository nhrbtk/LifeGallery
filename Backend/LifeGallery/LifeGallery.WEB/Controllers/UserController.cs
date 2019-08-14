﻿using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Infrastructure;
using LifeGallery.BLL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace LifeGallery.WEB.Controllers
{
    public class UsersController:ApiController
    {
        private IUserService UserService => HttpContext.Current.GetOwinContext().GetUserManager<IUserService>();

        private IPhotoService PhotoService => HttpContext.Current.GetOwinContext().Get<IPhotoService>();

        private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;

        
        [HttpPost]
        [Route("api/login")]
        public async Task<IHttpActionResult> LoginAsync([FromBody]UserDTO userDTO)
        {
            //SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                ClaimsIdentity claim = await UserService.Authenticate(userDTO);
                if (claim == null)
                {
                    return BadRequest("User with such login and password not found");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return Ok("Authorized.");
                }
            }
            else
                return BadRequest(ModelState);
        }

        [Authorize]
        [HttpGet]
        [Route("api/logout")]
        public void Logout()
        {
            AuthenticationManager.SignOut();
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<IHttpActionResult> Register([FromBody]UserDTO userDTO)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                userDTO.Role = "user";
                OperationDetails operationDetails = await UserService.Create(userDTO);
                if (operationDetails.Succedeed)
                    return Ok();
                else
                {
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                    return BadRequest(ModelState);
                }
            }
            else
                return BadRequest(ModelState);
        }

        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(new UserDTO
            {
                Email = "somemail@mail.ru",
                UserName = "somemail",
                Password = "ad46D_ewr3",
                Name = "Семен Семенович Горбунков",
                Role = "admin",
            }, new List<string> { "user", "admin" });
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/profile")]
        public IHttpActionResult GetCurrentProfile()
        {
            var id = User.Identity.GetUserId();
            var user = UserService.Read(id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/account/{username}")]
        public IHttpActionResult GetUserProfile(string username)
        {
            var user = UserService.ReadByUserName(username);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/account/{username}")]
        [Authorize(Roles = "admin")]
        public IHttpActionResult DeleteUser(string username)
        {
            var user = UserService.ReadByUserName(username);
            if (user != null)
            {
                UserService.Delete(user.Id);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/account/{username}")]
        [Authorize]
        public IHttpActionResult UpdateUser(string username, [FromBody]UserDTO userDTO)
        {
            
            if (userDTO != null)
            {
                userDTO.UserName = username;
                UserService.UpdateProfile(userDTO);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}