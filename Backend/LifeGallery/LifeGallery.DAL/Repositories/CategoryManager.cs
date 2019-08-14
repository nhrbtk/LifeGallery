using LifeGallery.DAL.EF;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Repositories
{
    public class CategoryManager : ICategoryManager
    {
        public LifeGalleryContext db;

        public CategoryManager(LifeGalleryContext context)
        {
            db = context ?? throw new NullReferenceException("Context is null.");
        }

        public void Create(Category category)
        {
            db.Categories.Add(category ?? throw new NullReferenceException("Category is null."));
        }

        public void Delete(int id)
        {
            Category category = db.Categories.Find(id);
            if(category != null)
            {
                db.Categories.Remove(category);
            }
        }

        public IEnumerable<Category> GetAll()
        {
            return db.Categories;
        }

        public Category Read(int id)
        {
            return db.Categories.Find(id);
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
        // ~CategoryManager()
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
