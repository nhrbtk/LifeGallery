using LifeGallery.DAL.EF;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Infrastructure;
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
            db = context;
        }

        public void Create(Photo photo, byte[] image)
        {            
            File.WriteAllBytes(photo.Path, image);
            db.Photos.Add(photo);
        }

        public void Delete(Photo photo)
        {            
            string photoPath = photo.Path;
            if (File.Exists(photoPath))
            {
                File.Delete(photoPath);
            }
            db.Photos.Remove(photo);
        }

        public byte[] GetImage(string path)
        {
            return File.Exists(path) ? File.ReadAllBytes(path) : throw new FileNotFoundException("Directory or file not found.");
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
            db.Entry(photo).State = EntityState.Modified;            
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
