using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Infrastructure;
using LifeGallery.DAL.Entities;
using System;
using System.Collections;
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
        IEnumerable<CommentDTO> GetPhotoComments(int id);
        IEnumerable<LikeDTO> GetPhotoLikes(int id);
        IEnumerable<CategoryDTO> GetPhotoCategories(int id);
        OperationDetails AddCategory(int photoId, int categoryId);
        PhotoDTO GetPhoto(int id);
        IEnumerable<PhotoDTO> GetFeed();
        OperationDetails Delete(int id);
    }
}
