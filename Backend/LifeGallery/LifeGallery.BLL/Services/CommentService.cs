using AutoMapper;
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

        public CommentService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public OperationDetails Create(CommentDTO commentDTO)
        {
            if (commentDTO.UserId == null)
            {
                return new OperationDetails(false, "User id is null", "");
            }
            if (commentDTO.Text == null)
            {
                return new OperationDetails(false, "Comment text is null", "");
            }
            Photo photo = Database.PhotoManager.GetInfo(commentDTO.PhotoId);
            if (photo == null)
            {
                return new OperationDetails(false, "Photo with such id is not found", commentDTO.PhotoId.ToString());
            }
            UserProfile user = Database.ProfileManager.Read(commentDTO.UserId);
            if (user == null)
            {
                return new OperationDetails(false, "User with such id is not found", commentDTO.UserId);
            }
            
            try
            {
                Comment comment = Mapper.Map<Comment>(commentDTO);
                comment.Photo = photo;
                comment.UserProfile = user;
                comment.Date = DateTime.UtcNow;
                Database.CommentManager.Create(comment);
                Database.Save();
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
            return new OperationDetails(true, "Comment added.", "");
        }

        public OperationDetails Delete(int id)
        {
            try
            {
                Database.CommentManager.Delete(id);
                return new OperationDetails(true, "Comment deleted", "");
            }
            catch(Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
        }

        public CommentDTO Read(int id)
        {
            var comment = Database.CommentManager.Read(id);
            if (comment == null)
            {
                return null;
            }
            else
            {
                CommentDTO commentDTO = Mapper.Map<CommentDTO>(comment);
                return commentDTO;
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
