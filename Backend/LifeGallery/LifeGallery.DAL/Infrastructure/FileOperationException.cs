using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.Infrastructure
{
    public class FileOperationException:Exception
    {
        public FileOperationException() { }

        public FileOperationException(string message) : base(message) { }

        public FileOperationException(string message, Exception inner) : base(message, inner) { }
    }
}
