using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    class SimpleFileDownloader : IFileDownloader
    {

        public IRepository Repository { get; }

        public string FileSavePath { get; }

        public SimpleFileDownloader(IRepository repository, string fileSavePath)
        {
            Repository = repository;
            FileSavePath = fileSavePath;
        }

        public void Download()
        {
            Directory.CreateDirectory(FileSavePath);

            foreach(var repositoryObject in Repository.GetRepositoryObjects())
            {
                repositoryObject.Download(FileSavePath);
            }

        }
    }
}
