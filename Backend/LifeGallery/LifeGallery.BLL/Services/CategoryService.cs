using LifeGallery.BLL.DTO;
using LifeGallery.BLL.DTO.Photo;
using LifeGallery.BLL.Infrastructure;
using LifeGallery.BLL.Interfaces;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork Database;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public async Task<OperationDetails> Create(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
                return new OperationDetails(false, "CategoryDTO is null", "");
            if (categoryDTO.Name == null || categoryDTO.Name.Length == 0)
                return new OperationDetails(false, "Invalid name.", "");

            Category category = new Category
            {
                Name = categoryDTO.Name
            };

            Database.CategoryManager.Create(category);
            await Database.SaveAsync();
            return new OperationDetails(true, "Category created.", category.Id.ToString());
        }

        public async Task<OperationDetails> Delete(int id)
        {
            Category category = Database.CategoryManager.Read(id);
            if (category == null)
                return new OperationDetails(false, "Category not found.", "");

            Database.CategoryManager.Delete(category);
            await Database.SaveAsync();
            return new OperationDetails(true, "Category deleted.", "");
        }

        public IEnumerable<PhotoInfoModel> GetPhotosByCategory(int id)
        {
            Category category = Database.CategoryManager.Read(id);
            if (category == null || category.Photos == null)
                return null;
            List<PhotoInfoModel> photoInfos = new List<PhotoInfoModel>();
            foreach (var photo in category.Photos)
            {
                PhotoInfoModel photoInfo = new PhotoInfoModel()
                {
                    Id = photo.Id,
                    Description = photo.Description,
                    IsPrivate = photo.IsPrivate,
                    Type = photo.Type,
                    PublishingDate = photo.PublishingDate,
                    LikesCount = photo.Likes.Count(),
                    CommentsCount = photo.Comments.Count(),
                    CategoriesIds = photo.Categories.Select(c => c.Id).ToList(),
                    UserId = photo.UserProfile.Id
                };
                photoInfos.Add(photoInfo);
            }
            return photoInfos;
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            var categories = Database.CategoryManager.GetAll();
            if (categories == null)
                return null;

            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            foreach (var cat in categories)
            {
                CategoryDTO categoryDTO = new CategoryDTO()
                {
                    Id = cat.Id,
                    Name = cat.Name
                };
                categoryDTOs.Add(categoryDTO);
            }
            return categoryDTOs;
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
