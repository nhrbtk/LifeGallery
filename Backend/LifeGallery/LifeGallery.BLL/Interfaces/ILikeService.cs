using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Interfaces
{
    public interface ILikeService : IDisposable
    {
        Task<OperationDetails> AddLike(LikeDTO likeDTO);
        Task<OperationDetails> RemoveLike(LikeDTO likeDTO);
    }
}
