using LG.BLL.DTO;
using LG.BLL.Infrastructure;
using LG.BLL.Interfaces;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LG.BLL.Services
{
    public class LikeService:ILikeService
    {
        private IUnitOfWork Database;
        public LikeService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public async Task<OperationDetails> AddLike(LikeDTO likeDTO)
        {
            if (likeDTO.UserId == null)
                return new OperationDetails(false, "UserId is null.", "");

            Photo photo = Database.PhotoManager.GetInfo(likeDTO.PhotoId);
            if (photo == null)
                return new OperationDetails(false, "Photo not found.", "");
            UserProfile user = Database.ProfileManager.Read(likeDTO.UserId);
            if (user == null)
                return new OperationDetails(false, "User not found.", "");

            if (Database.LikeManager.Find(l => l.Photo == photo && l.UserProfile == user).Count() > 0)
                return new OperationDetails(false, "User already liked this photo.", "");

            Database.LikeManager.Create(new Like
            {
                Photo = photo,
                UserProfile = user
            });
            await Database.SaveAsync();
            return new OperationDetails(true, "Photo liked.", "");
        }

        public async Task<OperationDetails> RemoveLike(LikeDTO likeDTO)
        {
            if (likeDTO.UserId == null)
                return new OperationDetails(false, "UserId is null.", "");

            Photo photo = Database.PhotoManager.GetInfo(likeDTO.PhotoId);
            if (photo == null)
                return new OperationDetails(false, "Photo not found.", "");
            UserProfile user = Database.ProfileManager.Read(likeDTO.UserId);
            if (user == null)
                return new OperationDetails(false, "User not found.", "");

            var likes = Database.LikeManager.Find(l => l.Photo == photo && l.UserProfile == user);
            if (likes.Count() == 0)
                return new OperationDetails(false, "User didn't liked this photo before.", "");

            Database.LikeManager.Delete(likes.First());
            await Database.SaveAsync();
            return new OperationDetails(true, "Photo unliked.", "");
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Database.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LikeService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
