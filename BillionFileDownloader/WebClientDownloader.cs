using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    class WebClientDownloader : IFileDownloader
    {


        public void Download(FileObject fileObject, string savePath)
        {
            try
            {
                Uri uri = new Uri(fileObject.FilePath);
                string fileSaveName = uri.Segments[uri.Segments.Length - 1];
                string fullFileSavePath = savePath + "\\" + fileSaveName;

                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(uri.ToString(), fullFileSavePath);
                    fileObject.SetIsDownloaded();

                    Logger.Source.Log($"File {fileSaveName} saved");
                }
            }
            catch (Exception ex)
            {
                fileObject.SetIsDownloadError(ex.InnerException.Message);

                Logger.Source.LogError(ex.Message);
                Logger.Source.LogError(ex.InnerException.Message);
            }
        }
    }
}
