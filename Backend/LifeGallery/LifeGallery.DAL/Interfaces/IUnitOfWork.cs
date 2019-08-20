using LifeGallery.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IProfileManager ProfileManager { get; }
        IPhotoManager PhotoManager { get; }
        ICommentManager CommentManager { get; }
        ICategoryManager CategoryManager { get; }
        ILikeManager LikeManager { get; }
        void Save();
        Task SaveAsync();
    }
}
