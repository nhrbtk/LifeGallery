using LifeGallery.BLL.DTO;
using LifeGallery.BLL.DTO.Photo;
using LifeGallery.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Interfaces
{
    public interface ICategoryService : IDisposable
    {
        Task<OperationDetails> Create(CategoryDTO categoryDTO);
        Task<OperationDetails> Delete(int id);
        IEnumerable<PhotoInfoModel> GetPhotosByCategory(int id);
        IEnumerable<CategoryDTO> GetCategories();
        IEnumerable<CategoryDTO> SearchCategories(string name);
    }
}
