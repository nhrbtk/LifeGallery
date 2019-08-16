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
                    cfg.CreateMap<UserDTO, UserProfile>();
                    cfg.CreateMap<UserProfile, UserDTO>()
                    .ForMember("PhotosIds", x => x.MapFrom(u => u.Photos.Select(p => p.Id)))
                    .ForMember("LikedIds", x => x.MapFrom(u => u.Liked.Select(l => l.Id)))
                    .ForMember("CommentsIds", x => x.MapFrom(u => u.Comments.Select(c => c.Id)));

                    cfg.CreateMap<PhotoDTO, Photo>();
                    cfg.CreateMap<Photo, PhotoDTO>()
                    .ForMember("UserId", x => x.MapFrom(p => p.UserProfile.Id))
                    .ForMember("LikesIds", x => x.MapFrom(p => p.Likes.Select(l => l.Id)))
                    .ForMember("CommentsIds", x => x.MapFrom(p => p.Comments.Select(c => c.Id)))
                    .ForMember("CategoriesIds", x => x.MapFrom(p => p.Categories.Select(c => c.Id)));

                    cfg.CreateMap<CategoryDTO, Category>();
                    cfg.CreateMap<Category, CategoryDTO>()
                    .ForMember("PhotosIds", x => x.MapFrom(c => c.Photos.Select(p => p.Id)));

                    cfg.CreateMap<LikeDTO, Like>();
                    cfg.CreateMap<Like, LikeDTO>()
                    .ForMember("PhotoId", x => x.MapFrom(l => l.Photo.Id))
                    .ForMember("UserId", x => x.MapFrom(l => l.UserProfile.Id));

                    cfg.CreateMap<CommentDTO, Comment>();
                    cfg.CreateMap<Comment, CommentDTO>()
                    .ForMember("PhotoId", x => x.MapFrom(c => c.Photo.Id))
                    .ForMember("UserId", x => x.MapFrom(c => c.UserProfile.Id));
                });
        }

        public ICategoryService CreateCategoryService(string connection)
        {
            return new CategoryService(new LGUnitOfWork(connection));
        }

        public ICommentService CreateCommentService(string connection)
        {
            return new CommentService(new LGUnitOfWork(connection));
        }

        public ILikeService CreateLikeService(string connection)
        {
            return new LikeService(new LGUnitOfWork(connection));
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
