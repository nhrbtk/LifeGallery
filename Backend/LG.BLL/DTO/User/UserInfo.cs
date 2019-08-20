using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LG.BLL.DTO.User
{
    public class UserInfo
    {
        public UserInfo()
        {
            //Role = new List<string>();
        }
        public string Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
        //public ICollection<string> Role { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime? Birthdate { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public int PhotosCount { get; set; }
    }
}
