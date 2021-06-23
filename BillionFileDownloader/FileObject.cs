using System;

namespace BillionFileDownloader
{
    public class FileObject
    {

        // Data
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string FilePath { get; set; }
        public int StatusId { get; set; }

        // Flag
        bool Downloading = false;
        bool isProcessed = false;

        // Error info
        bool isFaulted = false;
        public string ErrorMessage { get; set; }

        public bool IsFaulted { get => isFaulted; }
        public bool IsProcessed { get => isProcessed; }

        public void SetInDownloading()
        {
            Downloading = true;
        }

        public void SetIsDownloaded()
        {
            isProcessed = true;
        }

        public void SetIsDownloadError(string errorMessage)
        {
            isFaulted = true;

            ErrorMessage = errorMessage;
        }

        public bool IsNeedToDownload()
        {
            return !IsProcessed && !Downloading && !IsFaulted;
        }

    }




}