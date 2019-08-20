using LifeGallery.BLL.Interfaces;
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
