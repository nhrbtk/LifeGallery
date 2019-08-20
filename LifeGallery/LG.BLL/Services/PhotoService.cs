using LG.BLL.DTO;
using LG.BLL.DTO.Photo;
using LG.BLL.Infrastructure;
using LG.BLL.Interfaces;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Interfaces;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Collections;
using Microsoft.AspNet.Identity;
using LG.BLL.DTO.User;

namespace LG.BLL.Services
{
    public class PhotoService : IPhotoService
    {
        private IUnitOfWork Database;
        public PhotoService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public async Task<OperationDetails> Create(PhotoCreateModel createModel)
        {
            if (createModel == null
                || createModel.File == null
                || createModel.File.Length==0
                || createModel.FileName == null
                || createModel.StoragePath == null
                || createModel.UserId == null
                || createModel.CategoriesIds == null)
                return new OperationDetails(false, "Model is not valid.", "");

            UserProfile user = Database.ProfileManager.Read(createModel.UserId);
            if (user == null)
                return new OperationDetails(false, "User not found", "");

            string ext = "";
            List<string> allowedExtentions = new List<string>() { ".png", ".jpeg", ".jpg" };
            try
            {
                using(MemoryStream ms=new MemoryStream(createModel.File))
                {
                    Image.FromStream(ms);
                }
                ext = Path.GetExtension(createModel.FileName);
            }
            catch (ArgumentException ex)
            {
                return new OperationDetails(false, "Image or filename is not valid.", ex.Message);
            }
            if(!allowedExtentions.Contains(ext))
                return new OperationDetails(false, "Valid formats are png and jpeg.", "");

            byte[] img = ResizeImage(createModel.File, 1000);
            if (img == null)
                return new OperationDetails(false, "Image cannot be saved.", "");

            List<Category> categories = new List<Category>();
            foreach (var catId in createModel.CategoriesIds)
            {
                Category category = Database.CategoryManager.Read(catId);
                if (category == null)
                    return new OperationDetails(false, "Category not found.", catId.ToString());
                categories.Add(category);
            }

            string filename = user.ApplicationUser.UserName + '_' + DateTime.UtcNow.ToString("yyyyMMddHHmmssff") + ext;
            string path = Path.Combine(createModel.StoragePath, filename);

            Photo photo = new Photo()
            {
                Description = createModel.Description,
                IsPrivate = createModel.IsPrivate,
                Path = path,
                Type = ext == ".png" ? "image/png" : "image/jpeg",
                PublishingDate = DateTime.UtcNow,
                Categories = categories,
                UserProfile = user
            };            
            try
            {
                Database.PhotoManager.Create(photo, img);
            }
            catch (IOException)
            {
                return new OperationDetails(false, "Cannot get access to storage.", "");
            }
            await Database.SaveAsync();
            return new OperationDetails(true, "Photo saved.", photo.Id.ToString());
        }

        private byte[] ResizeImage(byte[] image, int maxSide)
        {
            byte[] result = null;
            if (image == null || maxSide < 0 || image.Length == 0)
            {
                return null;
            }
            using (MemoryStream ms = new MemoryStream(image))
            {
                double rate;
                Image img = Image.FromStream(ms);
                var isJpeg = ImageFormat.Jpeg.Equals(img.RawFormat);

                if (img.Width > maxSide || img.Height > maxSide)
                {
                    rate = (double)maxSide / Math.Max(img.Width, img.Height);
                    Bitmap bitmap = new Bitmap(img, Convert.ToInt32(img.Width * rate), Convert.ToInt32(img.Height * rate));
                    img = (Image)bitmap;
                    MemoryStream newStream = new MemoryStream();
                    img.Save(newStream, isJpeg ? ImageFormat.Jpeg : ImageFormat.Png);
                    result = newStream.ToArray();
                    newStream.Close();
                }
                else
                {
                    result = image;
                }
            }
            return result;
        }

        public async Task<OperationDetails> DeleteAsync(int id)
        {
            Photo photo = Database.PhotoManager.GetInfo(id);
            if (photo == null)
                return new OperationDetails(false, "Photo not found.", "");
            try
            {
                Database.PhotoManager.Delete(photo);
            }
            catch (IOException)
            {
                return new OperationDetails(false, "Cannot get access to storage.", "");
            }
            await Database.SaveAsync();
            return new OperationDetails(true, "Photo deleted.", "");
        }

        public IEnumerable<PhotoInfoModel> GetAllPhotosInfo()
        {
            var photos = Database.PhotoManager.GetInfoAll();
            if (photos == null)
                return null;
            List<PhotoInfoModel> photoInfoModels = new List<PhotoInfoModel>();
            foreach (var item in photos)
            {
                PhotoInfoModel photoInfo = new PhotoInfoModel()
                {
                    Id = item.Id,
                    Description = item.Description,
                    IsPrivate = item.IsPrivate,
                    Type = item.Type,
                    PublishingDate = item.PublishingDate,
                    LikesCount = item.Likes.Count(),
                    CommentsCount = item.Comments.Count(),
                    CategoriesIds = item.Categories.Select(c => c.Id).ToList(),
                    UserId = item.UserProfile.Id
                };
                photoInfoModels.Add(photoInfo);
            }
            return photoInfoModels;
        }

        public byte[] GetImage(int id)
        {
            Photo photo = Database.PhotoManager.GetInfo(id);
            if (photo == null || photo.Path == null)
                return null;
            byte[] img;
            try
            {
                img = Database.PhotoManager.GetImage(photo.Path);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch(IOException)
            {
                return null;
            }
            //var i = ImageFormat.Png.Equals(Image.FromStream(new MemoryStream(img)).RawFormat);
            return img;
        }

        public string GetOwnerId(int photoId)
        {
            Photo photo = Database.PhotoManager.GetInfo(photoId);
            if (photo == null || photo.UserProfile == null)
                return null;
            return photo.UserProfile.Id;
        }

        public IEnumerable<CategoryDTO> GetPhotoCategories(int id)
        {
            Photo photo = Database.PhotoManager.GetInfo(id);
            if (photo == null || photo.Categories == null)
                return null;
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            foreach (var cat in photo.Categories)
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

        public IEnumerable<CommentDTO> GetPhotoComments(int id)
        {
            Photo photo = Database.PhotoManager.GetInfo(id);
            if (photo == null || photo.Comments == null)
                return null;
            List<CommentDTO> commentDTOs = new List<CommentDTO>();
            var comments = photo.Comments;
            foreach (var com in comments)
            {
                CommentDTO commentDTO = new CommentDTO()
                {
                    Id = com.Id,
                    Date = com.Date,
                    Text = com.Text,
                    PhotoId = com.Photo.Id,
                    UserId = com.UserProfile.Id
                };
                commentDTOs.Add(commentDTO);
            }
            return commentDTOs;
        }

        public PhotoInfoModel GetPhotoInfo(int id)
        {
            Photo photo = Database.PhotoManager.GetInfo(id);
            if (photo == null)
                return null;
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
            return photoInfo;
        }

        public IEnumerable<UserInfo> GetPhotoLikers(int id)
        {
             Photo photo = Database.PhotoManager.GetInfo(id);
            if (photo == null || photo.Likes == null)
                return null;
            List<UserInfo> userInfos = new List<UserInfo>();
            foreach (var like in photo.Likes)
            {
                if (like.UserProfile == null || like.UserProfile.Id == null)
                    return null;
                userInfos.Add(new UserInfo()
                {
                    Id = like.UserProfile.Id,
                    Email = like.UserProfile.ApplicationUser.Email,
                    UserName = like.UserProfile.ApplicationUser.UserName,
                    //Role=like.UserProfile.ApplicationUser.Roles.
                    Name = like.UserProfile.Name,
                    Bio = like.UserProfile.Bio,
                    Birthdate = like.UserProfile.Birthdate,
                    ProfilePhoto = like.UserProfile.ProfilePhoto,
                    PhotosCount = like.UserProfile.Photos.Count()
                });
            }
            return userInfos;
        }

        public async Task<OperationDetails> UpdateInfo(PhotoUpdateModel updateModel)
        {
            if (updateModel == null || updateModel.CategoriesIds == null)
                return new OperationDetails(false, "Input is not valid.", "");
            Photo photo = Database.PhotoManager.GetInfo(updateModel.Id);
            if(photo==null)
                return new OperationDetails(false, "Photo not found.", "");

            List<Category> categories = new List<Category>();
            foreach (var catId in updateModel.CategoriesIds)
            {
                Category category = Database.CategoryManager.Read(catId);
                if (category == null)
                    return new OperationDetails(false, "Category not found.", catId.ToString());
                categories.Add(category);
            }

            photo.Description = updateModel.Description;
            photo.IsPrivate = updateModel.IsPrivate;
            photo.Categories = categories;
            Database.PhotoManager.Update(photo);
            await Database.SaveAsync();
            return new OperationDetails(true, "Photo updated.", "");
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
        // ~PhotoService()
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
