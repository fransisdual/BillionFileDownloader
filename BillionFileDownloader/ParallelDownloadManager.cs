using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    public class ParallelDownloadManager : IFileDownloadManager
    {
        FilesDBManager dBManager;
        List<FileObject> filesToDownloadBuffer;
        IFileDownloader fileDownloader;

        public ParallelDownloadManager(FilesDBManager dBManager, IFileDownloader fileDownloader, string fileSavePath)
        {
            this.dBManager = dBManager;
            //this.filesToDownloadBuffer = filesToDownloadBuffer;
            this.fileDownloader = fileDownloader;
            FileSavePath = fileSavePath;
            filesToDownloadBuffer = dBManager.GetNextFilesList();
        }

        public string FileSavePath { get; }

        public FileObject GetNextFileObject()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            while (filesToDownloadBuffer.Count>0)
            {
                filesToDownloadBuffer.AsParallel().ForAll(DownloadingProcess);

                filesToDownloadBuffer = dBManager.GetNextFilesList();
            }

        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        void DownloadingProcess(FileObject fileObject)
        {
            fileDownloader.Download(fileObject, FileSavePath);

            if (fileObject.IsProcessed)
                dBManager.SetFileObjectDownloaded(fileObject);
            if (fileObject.IsFaulted)
                dBManager.SetFileDownloadIsFaulted(fileObject, fileObject.ErrorMessage);
        }

    }
}
