using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    public interface IFileDownloadManager
    {

        public FileObject GetNextFileObject();
        public void Start();
        public void Stop();

    }
}
