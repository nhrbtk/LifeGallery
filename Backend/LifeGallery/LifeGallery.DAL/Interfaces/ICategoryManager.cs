using LifeGallery.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Interfaces
{
    public interface ICategoryManager : IDisposable
    {
        void Create(Category category);
        Category Read(int id);
        IEnumerable<Category> GetAll();
        void Delete(Category category);
    }
}
