using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    public class EnumDownloadManager : IFileDownloadManager
    {
        FilesDBManager dBManager;
        IEnumerable<FileObject> filesToDownloadBuffer;
        IFileDownloader fileDownloader;

        public EnumDownloadManager(FilesDBManager dBManager, IFileDownloader fileDownloader, string fileSavePath, IEnumerable<FileObject> filesToDownload)
        {
            this.dBManager = dBManager;
            this.fileDownloader = fileDownloader;
            FileSavePath = fileSavePath;
            filesToDownloadBuffer = filesToDownload;
        }

        public string FileSavePath { get; }

        public FileObject GetNextFileObject()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {

            //filesToDownloadBuffer.AsParallel().ForAll(DownloadingProcess);

            var thread = new Thread(DownloadingProcessOneThread);
            thread.Start();
            thread.Join();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        void DownloadingProcess(FileObject fileObject)
        {
            fileDownloader.Download(fileObject, FileSavePath);

            if (fileObject.Id == 3) return;

            if (fileObject.IsProcessed)
                dBManager.SetFileObjectDownloaded(fileObject);
            if (fileObject.IsFaulted)
                dBManager.SetFileDownloadIsFaulted(fileObject, fileObject.ErrorMessage);
        }


        void DownloadingProcessOneThread()
        {

            foreach(var file in filesToDownloadBuffer)
            {
                if (file.Id == 3) return;
                fileDownloader.Download(file, FileSavePath);

                if (file.IsProcessed)
                    dBManager.SetFileObjectDownloaded(file);
                if (file.IsFaulted)
                    dBManager.SetFileDownloadIsFaulted(file, file.ErrorMessage);
            }

        }

    }
}
