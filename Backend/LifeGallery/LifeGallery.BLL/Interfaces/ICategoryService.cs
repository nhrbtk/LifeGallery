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
    public interface ICategoryService:IDisposable
    {
        OperationDetails Create(CategoryDTO categoryDTO);
        IEnumerable<CategoryDTO> GetAll();
        CategoryDTO GetCategory(int id);
        IEnumerable<PhotoDTO> GetCategoryPhotos(int id);
        OperationDetails Delete(int id);
    }
}
