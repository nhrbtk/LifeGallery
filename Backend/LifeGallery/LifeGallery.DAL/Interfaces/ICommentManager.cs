using LifeGallery.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Interfaces
{
    public interface ICommentManager : IDisposable
    {
        void Create(Comment comment);
        Comment Read(int id);
        IEnumerable<Comment> GetAll();
        void Update(Comment comment);
        void Delete(Comment comment);
    }
}
