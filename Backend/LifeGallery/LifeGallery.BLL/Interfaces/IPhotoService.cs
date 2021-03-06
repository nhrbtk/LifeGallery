﻿using LifeGallery.BLL.DTO;
using LifeGallery.BLL.DTO.Photo;
using LifeGallery.BLL.DTO.User;
using LifeGallery.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.BLL.Interfaces
{
    public interface IPhotoService : IDisposable
    {
        Task<OperationDetails> Create(PhotoCreateModel createModel);
        Task<OperationDetails> UpdateInfo(PhotoUpdateModel updateModel);
        IEnumerable<CommentDTO> GetPhotoComments(int id);
        IEnumerable<UserInfo> GetPhotoLikers(int id);
        IEnumerable<CategoryDTO> GetPhotoCategories(int id);
        PhotoInfoModel GetPhotoInfo(int id);
        byte[] GetImage(int id);
        IEnumerable<PhotoInfoModel> GetAllPhotosInfo();
        string GetOwnerId(int photoId);
        Task<OperationDetails> DeleteAsync(int id);
    }
}
