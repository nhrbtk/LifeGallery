using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Interfaces
{
    public interface IPhotoService:IDisposable
    {
        Task<OperationDetails> Create(PhotoDTO photoDto);
        Task<OperationDetails> UpdateInfo(PhotoDTO photoDto);
        PhotoDTO GetPhoto(int id);
        IEnumerable<PhotoDTO> GetFeed();
        OperationDetails Delete(int id);
    }
}
