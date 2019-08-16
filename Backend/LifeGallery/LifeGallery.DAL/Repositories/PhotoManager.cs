using LifeGallery.DAL.EF;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Repositories
{
    public class PhotoManager : IPhotoManager
    {
        public LifeGalleryContext db;

        public PhotoManager(LifeGalleryContext context)
        {
            db = context ?? throw new NullReferenceException("Context is null.");
        }

        public void Create(Photo photo, byte[] image)
        {
            if(File.Exists(photo.Path ?? throw new ArgumentNullException("Path is null.")))
            {
                throw new InvalidOperationException("File already exists");
            }
            db.Photos.Add(photo ?? throw new NullReferenceException("Photo is null."));
            File.WriteAllBytes(photo.Path, image ?? throw new ArgumentNullException("Image is null."));
        }

        public void Delete(int id)
        {
            Photo photo = db.Photos.Find(id);
            if (photo != null)
            {
                if (File.Exists(photo.Path))
                {
                    File.Delete(photo.Path);
                }
                db.Photos.Remove(photo);
            }
        }

        public byte[] GetImage(string path)
        {
            return File.Exists(path) ? File.ReadAllBytes(path) : throw new FileNotFoundException();
        }

        public Photo GetInfo(int id)
        {
            return db.Photos.Find(id);
        }

        public IEnumerable<Photo> GetInfoAll()
        {
            return db.Photos;
        }

        public void Update(Photo photo)
        {
            //var oldPhoto = db.Photos.Find(photo.Id);
            //oldPhoto.Description = photo.Description;
            //oldPhoto.IsPrivate = photo.IsPrivate;
            //oldPhoto.Name = photo.Name;
            //oldPhoto.Path = photo.Path;
            //oldPhoto.PublishingDate = photo.PublishingDate;
            //oldPhoto.Type = photo.Type;
            //oldPhoto.Comments = photo.Comments;
            //oldPhoto.Categories = photo.Categories;
            //oldPhoto.Likes = photo.Likes;
            //oldPhoto.UserProfile = photo.UserProfile;
            db.Entry(photo ?? throw new NullReferenceException("Photo is null.")).State = EntityState.Modified;            
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PhotoManager()
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
