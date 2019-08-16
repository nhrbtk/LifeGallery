using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Infrastructure;
using LifeGallery.BLL.Interfaces;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Services
{
    public class LikeService : ILikeService
    {
        private IUnitOfWork Database;

        public LikeService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public OperationDetails Create(LikeDTO likeDTO)
        {
            try
            {
                Database.LikeManager.Create(new Like
                {
                    Photo = Database.PhotoManager.GetInfo(likeDTO.PhotoId),
                    UserProfile = Database.ProfileManager.Read(likeDTO.UserId)
                });
                Database.SaveAsync();
            }
            catch(Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
            return new OperationDetails(true, "Liked", "");
        }

        public OperationDetails Delete(LikeDTO likeDTO)
        {
            var like = Database.LikeManager.GetAll().Where(l => l.Photo.Id == likeDTO.PhotoId && l.UserProfile.Id == likeDTO.UserId).SingleOrDefault();
            if (like != null)
            {
                Database.LikeManager.Delete(like.Id);
                Database.SaveAsync();
                return new OperationDetails(true, "Unliked", "");
            }
            return new OperationDetails(false, "Like not found", "");
            
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
