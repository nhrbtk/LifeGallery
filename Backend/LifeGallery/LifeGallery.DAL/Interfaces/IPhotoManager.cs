using LifeGallery.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Interfaces
{
    public interface IPhotoManager : IDisposable
    {
        void Create(Photo photo, byte[] image);
        IEnumerable<Photo> GetInfoAll();
        Photo GetInfo(int id);
        byte[] GetImage(string path);
        void Update(Photo photo);
        void Delete(Photo photo);
    }
}
