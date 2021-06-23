using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    public interface IFileDownloader
    {
        public void Download(FileObject fileObject, string savePath);
    }
}
