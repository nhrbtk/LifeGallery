using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Interfaces
{
    public interface ICommentService : IDisposable
    {
        Task<OperationDetails> Create(CommentDTO commentDTO);
        Task<OperationDetails> Delete(int id);
        string GetOwnerId(int id);
    }
}
