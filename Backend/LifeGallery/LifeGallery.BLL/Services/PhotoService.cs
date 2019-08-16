using AutoMapper;
using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Infrastructure;
using LifeGallery.BLL.Interfaces;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Interfaces;
using LifeGallery.DAL.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LifeGallery.BLL.Services
{
    public class PhotoService : IPhotoService
    {
        private IUnitOfWork Database;

        public PhotoService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(PhotoDTO photoDto)
        {
            try
            {
                Photo photo = Mapper.Map<Photo>(photoDto);
                photo.UserProfile = Database.ProfileManager.Read(photoDto.UserId);
                Database.PhotoManager.Create(photo, photoDto.File);
                await Database.SaveAsync();
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
            return new OperationDetails(true, "Photo added", "");
        }

        public OperationDetails Delete(int id)
        {
            try
            {
                Database.PhotoManager.Delete(id);
            }
            catch(Exception ex)
            {
                return new OperationDetails(false, ex.Message, ex.StackTrace);
            }
            return new OperationDetails(true, "Photo deleted", id.ToString());
        }

        public PhotoDTO GetPhoto(int id)
        {
            var p = Database.PhotoManager.GetInfo(id);
            var photo = Mapper.Map<PhotoDTO>(p);
            photo.File = Database.PhotoManager.GetImage(photo.Path);
            return photo;
        }

        public IEnumerable<PhotoDTO> GetFeed()
        {
            var photoDTOs = Mapper.Map<IEnumerable<PhotoDTO>>(Database.PhotoManager.GetInfoAll());
            foreach (var item in photoDTOs)
            {
                item.File = Database.PhotoManager.GetImage(item.Path);
            }
            return photoDTOs;
        }

        public async Task<OperationDetails> UpdateInfo(PhotoDTO photoDto)
        {
            try
            {
                Photo oldPhoto = Database.PhotoManager.GetInfo(photoDto.Id);
                oldPhoto.PublishingDate = photoDto.PublishingDate;
                oldPhoto.Type = photoDto.Type;
                oldPhoto.Path = photoDto.Path;
                oldPhoto.Description = photoDto.Description;
                Database.PhotoManager.Update(oldPhoto);
                await Database.SaveAsync();
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "");
            }
            return new OperationDetails(true, "Photo updated.", "");
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
        // ~PhotoService()
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
