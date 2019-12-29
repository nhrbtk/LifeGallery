﻿using LifeGallery.DAL.EF;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Repositories
{
    public class LikeManager : ILikeManager
    {
        public LifeGalleryContext db;

        public LikeManager(LifeGalleryContext context)
        {
            db = context;
        }

        public void Create(Like like)
        {
            db.Likes.Add(like);
        }

        public IEnumerable<Like> GetAll()
        {
            return db.Likes;
        }

        public void Delete(Like like)
        {
            db.Likes.Remove(like);
        }

        public IEnumerable<Like> Find(Func<Like, bool> func)
        {
            return db.Likes.Where(func);
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
        // ~LikeManager()
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