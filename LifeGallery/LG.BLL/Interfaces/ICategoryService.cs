using LG.BLL.DTO;
using LG.BLL.DTO.Photo;
using LG.BLL.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LG.BLL.Interfaces
{
    public interface ICategoryService:IDisposable
    {
        Task<OperationDetails> Create(CategoryDTO categoryDTO);
        Task<OperationDetails> Delete(int id);
        IEnumerable<PhotoInfoModel> GetPhotosByCategory(int id);
        IEnumerable<CategoryDTO> GetCategories();
    }
}
