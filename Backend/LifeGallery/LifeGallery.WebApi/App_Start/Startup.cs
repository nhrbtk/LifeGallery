using LifeGallery.BLL.Interfaces;
using LifeGallery.BLL.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(LifeGallery.WebApi.App_Start.Startup))]

namespace LifeGallery.WebApi.App_Start
{
    public class Startup
    {
        IServiceCreator serviceCreator = new ServiceCreator();

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.CreatePerOwinContext<IPhotoService>(CreatePhotoService);
            app.CreatePerOwinContext<ILikeService>(CreateLikeService);
            app.CreatePerOwinContext<ICategoryService>(CreateCategoryService);
            app.CreatePerOwinContext<ICommentService>(CreateCommentService);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/api/login"),
            });
        }

        private IUserService CreateUserService()
        {
            return serviceCreator.CreateUserService("DefaultConnection");
        }

        private IPhotoService CreatePhotoService()
        {
            return serviceCreator.CreatePhotoService("DefaultConnection");
        }

        private ILikeService CreateLikeService()
        {
            return serviceCreator.CreateLikeService("DefaultConnection");
        }

        private ICategoryService CreateCategoryService()
        {
            return serviceCreator.CreateCategoryService("DefaultConnection");
        }

        private ICommentService CreateCommentService()
        {
            return serviceCreator.CreateCommentService("DefaultConnection");
        }
    }
}