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
    public class CommentService : ICommentService
    {
        private IUnitOfWork Database;
        public CommentService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public async Task<OperationDetails> Create(CommentDTO commentDTO)
        {
            if (commentDTO == null)
                return new OperationDetails(false, "CommentDTO is null.", "");
            if (commentDTO.UserId == null)
                return new OperationDetails(false, "UserId is null.", "");
            if (commentDTO.Text == null)
                return new OperationDetails(false, "Text is null.", "");

            Photo photo = Database.PhotoManager.GetInfo(commentDTO.PhotoId);
            if (photo == null)
                return new OperationDetails(false, "Photo not found.", "");

            UserProfile user = Database.ProfileManager.Read(commentDTO.UserId);
            if (user == null)
                return new OperationDetails(false, "User not found.", "");

            Comment comment = new Comment
            {
                Photo = photo,
                UserProfile = user,
                Text = commentDTO.Text,
                Date = DateTime.UtcNow
            };
            Database.CommentManager.Create(comment);
            await Database.SaveAsync();
            return new OperationDetails(true, "Comment added.", comment.Id.ToString());
        }

        public async Task<OperationDetails> Delete(int id)
        {
            Comment comment = Database.CommentManager.Read(id);
            if (comment == null)
                return new OperationDetails(false, "Comment not found.", "");

            Database.CommentManager.Delete(comment);
            await Database.SaveAsync();
            return new OperationDetails(true, "Comment deleted.", "");
        }

        public string GetOwnerId(int id)
        {
            Comment comment = Database.CommentManager.Read(id);
            if (comment != null && comment.UserProfile != null)
            {
                return comment.UserProfile.Id;
            }
            else
            {
                return null;
            }
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
        // ~CommentService()
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
