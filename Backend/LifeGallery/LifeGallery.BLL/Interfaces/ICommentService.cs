using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Interfaces
{
    public interface ICommentService:IDisposable
    {
        OperationDetails Create(CommentDTO commentDTO);
        OperationDetails Delete(int id);
        CommentDTO Read(int id);
    }
}
