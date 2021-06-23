using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    public class MultiThreadFileDownloader : IFileDownloadManager
    {

        object lockObject;

        FilesDBManager dBManager;
        List<FileObject> filesToDownloadBuffer;
        IFileDownloader fileDownloader;

        List<Thread> threads;

        int numberOfTasks = 4;

        public string FileSavePath { get; }

        public MultiThreadFileDownloader(FilesDBManager dBManager, IFileDownloader fileDownloader, string fileSavePath, int numberOfTasks)
        {
            this.dBManager = dBManager;
            FileSavePath = fileSavePath;
            this.fileDownloader = fileDownloader;
            filesToDownloadBuffer = dBManager.GetNextFilesList();
            this.numberOfTasks = numberOfTasks;

            lockObject = new object();
        }

        public void Start()
        {
            threads = new List<Thread>();

            for (int i = 0; i < numberOfTasks; i++)
            {
                var thread = new Thread(DownloadingProcess);
                thread.Start();
                threads.Add(thread);
            }

            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
            }
        }


        void DownloadingProcess()
        {
            FileObject fileObject;
            fileObject = GetNextFileObject();

            while (fileObject != null)
            {

                fileDownloader.Download(fileObject, FileSavePath);

                lock (lockObject)
                {
                    if (fileObject.IsProcessed)
                        dBManager.SetFileObjectDownloaded(fileObject);
                    if (fileObject.IsFaulted)
                        dBManager.SetFileDownloadIsFaulted(fileObject, fileObject.ErrorMessage);
                }

                fileObject = GetNextFileObject();

                Console.WriteLine("Thread #" + Thread.CurrentThread.ManagedThreadId);
            }


        }

        public FileObject GetNextFileObject()
        {
            lock (lockObject)
            {
                var remainingFiles = filesToDownloadBuffer.Where(s => s.IsNeedToDownload());

                if (remainingFiles.Count() == 0)
                    filesToDownloadBuffer = dBManager.GetNextFilesList();

                remainingFiles = filesToDownloadBuffer.Where(s => s.IsNeedToDownload());

                if (remainingFiles.Count() == 0)
                    return null;

                var fileobject = filesToDownloadBuffer.Where(s => s.IsNeedToDownload()).First();

                fileobject.SetInDownloading();

                return fileobject;
            }

        }


        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
