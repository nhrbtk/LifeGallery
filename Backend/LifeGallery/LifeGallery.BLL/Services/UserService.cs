using AutoMapper;
using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Infrastructure;
using LifeGallery.BLL.Interfaces;
using LifeGallery.DAL.Entities;
using LifeGallery.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database;

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDTO)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            ApplicationUser user = await Database.UserManager.FindAsync(userDTO.UserName, userDTO.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public async Task<OperationDetails> ChangePassword(string id, string currentPassword, string newPassword)
        {
            var result = await Database.UserManager.ChangePasswordAsync(id, currentPassword, newPassword);
            if (result.Errors.Count() > 0)
                return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
            else
                return new OperationDetails(true, "Password successfully changed.", "");
        }

        public async Task<OperationDetails> Create(UserDTO userDTO)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDTO.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDTO.Email, UserName = userDTO.UserName ?? userDTO.Email };
                var result = await Database.UserManager.CreateAsync(user, userDTO.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDTO.Role);
                // создаем профиль клиента
                UserProfile clientProfile = new UserProfile { Id = user.Id, Name = userDTO.Name };
                Database.ProfileManager.Create(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<OperationDetails> Delete(string id)
        {
            ApplicationUser user = await Database.UserManager.FindByIdAsync(id);
            if (user != null)
            {
                await Database.UserManager.DeleteAsync(user);
                await Database.SaveAsync();
                return new OperationDetails(true, "User deleted.", user.Id);
            }
            else
            {
                return new OperationDetails(false, "User not found.", user.Id);
            }
        }

        public IEnumerable<UserDTO> GetAll()
        {
            var userDTOs = Mapper.Map<IEnumerable<UserDTO>>(Database.ProfileManager.GetAll());
            foreach (var item in userDTOs)
            {
                var appUser = Database.UserManager.FindById(item.Id);
                item.Email = appUser.Email;
                item.UserName = appUser.UserName;
            }

            return userDTOs;

        }

        public UserDTO Read(string id)
        {
            var userDTO = Mapper.Map<UserDTO>(Database.ProfileManager.Read(id));
            if (userDTO != null)
            {
                var appUser = Database.UserManager.FindById(userDTO.Id);
                userDTO.Email = appUser.Email;
                userDTO.UserName = appUser.UserName;
            }
            return userDTO;
        }

        public UserDTO ReadByUserName(string username)
        {
            var appUser = Database.UserManager.FindByName(username);
            if (appUser == null)
                return null;
            var userDTO = Mapper.Map<UserDTO>(Database.ProfileManager.Read(appUser.Id));
            if (userDTO != null)
            {
                userDTO.Email = appUser.Email;
                userDTO.UserName = appUser.UserName;
            }
            return userDTO;
        }

        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
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
            await Create(adminDto);
        }

        public async Task<OperationDetails> UpdateProfile(UserDTO userDTO)
        {
            var appUser = Database.UserManager.FindByName(userDTO.UserName);
            var user = Mapper.Map<UserProfile>(userDTO);
            user.Id = appUser.Id;
            user.ApplicationUser = appUser;
            Database.ProfileManager.Update(user);
            await Database.SaveAsync();
            return new OperationDetails(true, "Profile updated", "");
        }

        public async Task<OperationDetails> ChangeUserName(string id, string username)
        {
            var user = await Database.UserManager.FindByIdAsync(id);
            if (user == null)
                return new OperationDetails(false, "User not found.", id);
            if (await Database.UserManager.FindByNameAsync(username) == null)
            {                
                user.UserName = username;
                await Database.SaveAsync();
                return new OperationDetails(true, "UserName is available.", username);
            }
            else
            {
                return new OperationDetails(false, "UserName is taked.", username);
            }
        }

        public async Task<OperationDetails> CheckUserName(string username)
        {
            if (await Database.UserManager.FindByNameAsync(username) == null)
            {
                return new OperationDetails(true, "UserName is available.", username);
            }
            else
            {
                return new OperationDetails(false, "UserName is taked.", username);
            }
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
