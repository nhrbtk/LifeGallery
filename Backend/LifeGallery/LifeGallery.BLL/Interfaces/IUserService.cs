using LifeGallery.BLL.DTO;
using LifeGallery.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Interfaces
{
    public interface IUserService:IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDTO);
        UserDTO Read(string id);
        UserDTO ReadByUserName(string username);
        IEnumerable<UserDTO> GetAll();
        OperationDetails Delete(string id);
        Task<OperationDetails> ChangePassword(string id, string currentPassword, string newPassword);
        Task<OperationDetails> ChangeUserName(string id, string username);
        Task<OperationDetails> CheckUserName(string username);
        Task<OperationDetails> UpdateProfile(UserDTO userDTO);
        IEnumerable<PhotoDTO> GetUserPhotos(string userId);
        Task<ClaimsIdentity> Authenticate(UserDTO userDTO);
        Task SetInitialData(UserDTO adminDto, List<string> roles);
    }
}
