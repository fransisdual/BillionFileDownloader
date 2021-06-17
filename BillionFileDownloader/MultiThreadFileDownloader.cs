using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    class MultiThreadFileDownloader : IFileDownloader
    {
        int numberOfThreads = 4;

        public IRepository Repository { get; }

        List<IRepositoryObject> repositoryObjects;

        public string FileSavePath { get; }

        public MultiThreadFileDownloader(IRepository repository, string fileSavePath)
        {
            Repository = repository;
            FileSavePath = fileSavePath;

            repositoryObjects = repository.GetRepositoryObjects().ToList();
        }

        public void Download()
        {
            var tasks = new List<Task>();
            int filesCount = repositoryObjects.Count;

            Directory.CreateDirectory(FileSavePath);

            foreach (var repositoryObject in Repository.GetRepositoryObjects())
            {
                tasks.Add(Task.Run(() => repositoryObject.Download(FileSavePath)));

                //repositoryObject.Download(FileSavePath);
            }

            Task t = Task.WhenAll(tasks);
            
            try
            {
                t.Wait();
            }
            catch { }

            if (t.Status == TaskStatus.RanToCompletion)
                Logger.Source.Log("All files are downloaded.");
            else if (t.Status == TaskStatus.Faulted)
                Logger.Source.LogError("Some files downloading is failed");
        }

    }
}
