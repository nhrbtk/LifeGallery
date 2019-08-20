using LG.BLL.DTO;
using LG.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LG.BLL.Interfaces
{
    public interface ICommentService:IDisposable
    {
        Task<OperationDetails> Create(CommentDTO commentDTO);
        Task<OperationDetails> Delete(int id);
        string GetOwnerId(int id);
    }
}
