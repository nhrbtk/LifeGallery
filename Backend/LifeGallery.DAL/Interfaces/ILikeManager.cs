using LifeGallery.DAL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Interfaces
{
    public interface ILikeManager : IDisposable
    {
        void Create(Like like);        
        IEnumerable<Like> GetAll();
        IEnumerable<Like> Find(Func<Like, bool> func);
        void Delete(Like like);       

    }
}
