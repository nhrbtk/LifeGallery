using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connection);
        IPhotoService CreatePhotoService(string connection);
        ICategoryService CreateCategoryService(string connection);
        ICommentService CreateCommentService(string connection);
        ILikeService CreateLikeService(string connection);
    }
}
