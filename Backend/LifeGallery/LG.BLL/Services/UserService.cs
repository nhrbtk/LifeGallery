using AutoMapper;
using LG.BLL.DTO.Photo;
using LG.BLL.DTO.User;
using LG.BLL.Infrastructure;
using LG.BLL.Interfaces;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LG.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database;
        public UserService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public async Task<OperationDetails> AddUserToRoleAsync(string id, string role)
        {
            if (id == null || role == null)
                return new OperationDetails(false, "Input is null.", id == null ? "Id" : "Role");

            var result = await Database.UserManager.AddToRoleAsync(id, role);
            return new OperationDetails(result.Succeeded, result.Errors?.FirstOrDefault() ?? "Error.", "");
        }

        public async Task<ClaimsIdentity> AuthenticateAsync(LoginModel loginModel)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            ApplicationUser user = await Database.UserManager.FindAsync(loginModel.UserName, loginModel.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public async Task<OperationDetails> CreateAsync(RegisterModel registerModel)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(registerModel.UserName);
            if (user == null)
            {
                user = new ApplicationUser { Email = registerModel.Email, UserName = registerModel.UserName };
                var result = await Database.UserManager.CreateAsync(user, registerModel.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, registerModel.Role);
                // создаем профиль клиента
                UserProfile clientProfile = new UserProfile { Id = user.Id };
                Database.ProfileManager.Create(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Registered", user.Id);
            }
            else
            {
                return new OperationDetails(false, "User with such username already exists.", "UserName");
            }
        }

        public async Task<OperationDetails> DeleteAsync(string id)
        {
            if (id == null)
                return new OperationDetails(false, "Id is null.", "");
            ApplicationUser user = Database.UserManager.FindById(id);
            if (user != null)
            {
                var photos = user.UserProfile.Photos.ToList();
                foreach (var item in photos)
                {
                    Database.PhotoManager.Delete(item);
                }
                Database.UserManager.Delete(user);
                await Database.SaveAsync();
                return new OperationDetails(true, "User deleted.", user.Id);
            }
            else
            {
                return new OperationDetails(false, "User not found.", user.Id);
            }
        }

        public IEnumerable<UserInfo> GetAll()
        {
            var users = Database.ProfileManager.GetAll();
            List<UserInfo> usersInfo = new List<UserInfo>();
            if (users != null)
            {
                foreach (var u in users)
                {
                    UserInfo userInfo = new UserInfo()
                    {
                        Id = u.Id,
                        Email = u.ApplicationUser.Email,
                        UserName = u.ApplicationUser.UserName,
                        //Role=u.ApplicationUser.Roles.
                        Name = u.Name,
                        Bio = u.Bio,
                        Birthdate = u.Birthdate,
                        ProfilePhoto = u.ProfilePhoto,
                        PhotosCount = u.Photos.Count()
                    };
                    usersInfo.Add(userInfo);
                }
            }
            return usersInfo;
        }

        public IEnumerable<PhotoInfoModel> GetUserPhotosInfo(string userId)
        {            
            if (userId == null)
            {
                return null;
            }
            var user = Database.ProfileManager.Read(userId);
            if (user == null || user.Photos == null)
            {
                return null;
            }

            List<PhotoInfoModel> photoInfos = new List<PhotoInfoModel>();
            foreach (var photo in user.Photos)
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

        public UserInfo Read(string id)
        {
            if (id == null)
            {
                return null;
            }
            var user = Database.ProfileManager.Read(id);
            if (user == null)
            {
                return null;
            }

            UserInfo userInfo = new UserInfo()
            {
                Id = user.Id,
                Email = user.ApplicationUser.Email,
                UserName = user.ApplicationUser.UserName,
                //Role=user.ApplicationUser.Roles.
                Name = user.Name,
                Bio = user.Bio,
                Birthdate = user.Birthdate,
                ProfilePhoto = user.ProfilePhoto,
                PhotosCount = user.Photos.Count()
            };
            return userInfo;
        }

        public async Task SetInitialData(RegisterModel adminModel, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await CreateAsync(adminModel);
        }

        public OperationDetails UpdateProfile(UserUpdateModel updateModel)
        {
            if (updateModel == null || updateModel.Id==null)
                return new OperationDetails(false, "Model is not valid.", "");
            UserProfile userProfile = Database.ProfileManager.Read(updateModel.Id);
            if (userProfile == null)
                return new OperationDetails(false, "User not found.", "");
            byte[] profileImg = null;
            if (updateModel.ProfilePhoto != null)
            {
                Image img;
                try
                {
                    using (MemoryStream ms = new MemoryStream(updateModel.ProfilePhoto))
                    {
                        img = Image.FromStream(ms);
                    }
                }
                catch (ArgumentException)
                {
                    return new OperationDetails(false, "Profile image is not valid.", "");
                }
                if(!ImageFormat.Png.Equals(img.RawFormat))
                    return new OperationDetails(false, "Profile image must be PNG.", "");

                Bitmap bitmap = new Bitmap(img, 300, 300);
                ImageConverter converter = new ImageConverter();
                profileImg = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
            }

            userProfile.Bio = updateModel.Bio;
            userProfile.Name = updateModel.Name;
            userProfile.Birthdate = updateModel.Birthdate;
            userProfile.ProfilePhoto = profileImg;
            Database.ProfileManager.Update(userProfile);
            Database.Save();
            return new OperationDetails(true, "User updated", "");
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
        // ~UserService()
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
