using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    class SimpleFileDownloader : IFileDownloadManager
    {
        bool cancelled = false;
        FilesDBManager dBManager;
        List<FileObject> filesToDownloadBuffer;
        IFileDownloader fileDownloader;

        public string FileSavePath { get; }

        public SimpleFileDownloader(FilesDBManager dBManager, IFileDownloader fileDownloader, string fileSavePath)
        {
            this.dBManager = dBManager;
            FileSavePath = fileSavePath;
            this.fileDownloader = fileDownloader;
            filesToDownloadBuffer = dBManager.GetNextFilesList();
        }

        public void Start()
        {
            Directory.CreateDirectory(FileSavePath);

            FileObject fileObject = GetNextFileObject();

            while (fileObject != null)
            {
                if (cancelled)
                    break;

                fileDownloader.Download(fileObject, FileSavePath);

                if (fileObject.IsProcessed)
                    dBManager.SetFileObjectDownloaded(fileObject);
                if (fileObject.IsFaulted)
                    dBManager.SetFileDownloadIsFaulted(fileObject, fileObject.ErrorMessage);

                fileObject = GetNextFileObject();
            }

        }

        public void Stop()
        {
            cancelled = true;
        }

        public FileObject GetNextFileObject()
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

        void SaveFileObjectToDatabase(FileObject fileObject)
        {
            dBManager.SetFileObjectDownloaded(fileObject);
        }

    }
}