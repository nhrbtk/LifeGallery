using LifeGallery.BLL.DTO.Photo;
using LifeGallery.BLL.DTO.User;
using LifeGallery.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> CreateAsync(RegisterModel registerModel);
        UserInfo Read(string id);
        IEnumerable<UserInfo> GetAll();
        IEnumerable<UserInfo> SearchByUsername(string username);
        Task<OperationDetails> DeleteAsync(string id);
        OperationDetails UpdateProfile(UserUpdateModel updateModel);
        IEnumerable<PhotoInfoModel> GetUserPhotosInfo(string userId);
        Task<ClaimsIdentity> AuthenticateAsync(LoginModel loginModel);
        Task<OperationDetails> AddUserToRoleAsync(string id, string role);
        Task<bool> IsInRole(string id, string role);
        Task SetInitialData(RegisterModel adminModel, List<string> roles);
    }
}
