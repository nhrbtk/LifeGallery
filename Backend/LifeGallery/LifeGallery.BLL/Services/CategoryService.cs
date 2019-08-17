using AutoMapper;
using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Infrastructure;
using LifeGallery.BLL.Interfaces;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork Database;

        public CategoryService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public OperationDetails Create(CategoryDTO categoryDTO)
        {
            try
            {
                Database.CategoryManager.Create(Mapper.Map<Category>(categoryDTO));
                Database.Save();
            }
            catch(Exception ex)
            {
                return new OperationDetails(false, ex.Message, ex.StackTrace);
            }
            return new OperationDetails(true, "Category created", "");
        }

        public OperationDetails Delete(int id)
        {
            try
            {
                Database.CategoryManager.Delete(id);
                Database.Save();
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, ex.StackTrace);
            }
            return new OperationDetails(true, "Category deleted", "");
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            return Mapper.Map<IEnumerable<CategoryDTO>>(Database.CategoryManager.GetAll());
        }

        public CategoryDTO GetCategory(int id)
        {
            return Mapper.Map<CategoryDTO>(Database.CategoryManager.Read(id));         
        }

        public IEnumerable<PhotoDTO> GetCategoryPhotos(int id)
        {
            Category category = Database.CategoryManager.Read(id);
            if(category==null || category.Photos == null)
            {
                return null;
            }

            var photoDTOs = Mapper.Map<IEnumerable<PhotoDTO>>(category.Photos);
            foreach (var photoDTO in photoDTOs)
            {
                photoDTO.File = Database.PhotoManager.GetImage(photoDTO.Path);
            }
            return photoDTOs;
        }     

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Database.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CategoryService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
