using LifeGallery.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Interfaces
{
    public interface IProfileManager : IDisposable
    {
        void Create(UserProfile profile);
        UserProfile Read(string id);
        IEnumerable<UserProfile> GetAll();
        void Update(UserProfile profile);
        void Delete(string id);
    }
}
