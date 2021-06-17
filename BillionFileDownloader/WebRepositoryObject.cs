using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    class WebRepositoryObject : IRepositoryObject
    {
        string stringPath;
        

        public WebRepositoryObject(string stringPath)
        {
            this.stringPath = stringPath;
        }


        public bool Download(string savePath)
        {

            try
            {
                // Проблема каждый раз будет создаваться новый веб клиент

                Uri uri = new Uri(stringPath);
                string fileSaveName = uri.Segments[uri.Segments.Length - 1];
                
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(stringPath, savePath +"\\"+ fileSaveName);
                    Logger.Source.Log($"File {fileSaveName} saved");
                }

            }

            catch(Exception ex)
            {
                Logger.Source.LogError(ex.Message);
                Logger.Source.LogError(ex.InnerException.Message);
                return false;
            }

            return true;

        }

        public string getStringPath()
        {
            return stringPath;
        }
    }
}
