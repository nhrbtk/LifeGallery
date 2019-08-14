using AutoMapper;
using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Interfaces;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        static ServiceCreator()
        {
            Mapper.Initialize
                (cfg =>
                {
                    cfg.CreateMap<UserProfile, UserDTO>();
                    cfg.CreateMap<UserDTO, UserProfile>();
                    cfg.CreateMap<PhotoDTO, Photo>();
                    cfg.CreateMap<Photo, PhotoDTO>();

                    cfg.CreateMap<Category, CategoryDTO>();
                    cfg.CreateMap<CategoryDTO, Category>();
                    cfg.CreateMap<Like, LikeDTO>();
                    cfg.CreateMap<LikeDTO, Like>();
                    cfg.CreateMap<Comment, CommentDTO>();
                    cfg.CreateMap<CommentDTO, Comment>();
                });
        }
        public IPhotoService CreatePhotoService(string connection)
        {
            return new PhotoService(new LGUnitOfWork(connection));
        }

        public IUserService CreateUserService(string connection)
        {
            return new UserService(new LGUnitOfWork(connection));
        }
    }
}
