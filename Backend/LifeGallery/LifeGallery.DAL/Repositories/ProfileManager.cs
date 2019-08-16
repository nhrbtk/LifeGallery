using LifeGallery.DAL.EF;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Repositories
{
    public class ProfileManager : IProfileManager
    {
        public LifeGalleryContext db;

        public ProfileManager(LifeGalleryContext context)
        {
            db = context ?? throw new NullReferenceException("Context is null.");
        }

        public void Create(UserProfile profile)
        {
            db.UserProfiles.Add(profile ?? throw new NullReferenceException("Profile is null."));
        }

        public void Delete(string id)
        {
            UserProfile profile = db.UserProfiles.Find(id ?? throw new NullReferenceException("Id is null."));
            if (profile != null)
            {
                db.UserProfiles.Remove(profile);
            }
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return db.UserProfiles;
        }

        public UserProfile Read(string id)
        {
            return db.UserProfiles.Find(id);
        }

        public void Update(UserProfile profile)
        {
            //db.Entry(profile ?? throw new NullReferenceException("Profile is null.")).State = EntityState.Modified;
            var oldUser = db.UserProfiles.Find(profile.Id);
            oldUser.Bio = profile.Bio;
            oldUser.Birthdate = profile.Birthdate;
            oldUser.Name = profile.Name;
            oldUser.ProfilePhoto = profile.ProfilePhoto;
            oldUser.Photos = profile.Photos;
            oldUser.Liked = profile.Liked;
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
        // ~ProfileManager()
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
