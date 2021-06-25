using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    class Program
    {
        static void Main(string[] args)
        {

            FilesDBManager filesDBManager = new FilesDBManager("Data Source=localhost;Initial Catalog=BillionFileDownloader;Integrated Security=True");

            filesDBManager.SetAllDownloadingFilesToDownload();
            filesDBManager.SetAllFilesToDownload();

            WebClientDownloader webClientDownloader = new WebClientDownloader();
            // Logger initialization
            Logger.Source = new ConsoleLogger();

            string filePath = Directory.GetCurrentDirectory() + "\\FileList.txt";
            string savePathDirectory = Directory.GetCurrentDirectory() + "\\Downloads";

            bool isFileExist = File.Exists(filePath);

            if (isFileExist)
            {




                IFileDownloadManager fileDownloadManager;
                //Console.WriteLine("Start consistently downloading\n");

                //
                //fileDownloadManager = new SimpleFileDownloader(filesDBManager, webClientDownloader, savePathDirectory);
                //fileDownloadManager.Start();

                //Console.WriteLine("Downloading is finished\n");

                Console.WriteLine("Start multi thread downloading\n");
                fileDownloadManager = new MultiThreadFileDownloader(filesDBManager, webClientDownloader, savePathDirectory, 4);
                fileDownloadManager.Start();
                Console.WriteLine("Downloading is finished\n");

                filesDBManager.SetAllFilesToDownload();


                DBEnum files = new DBEnum();

                Console.WriteLine("Start parralel downloading\n");
                //fileDownloadManager = new ParallelDownloadManager(filesDBManager, webClientDownloader, savePathDirectory);
                fileDownloadManager = new ParallelDownloadManager(filesDBManager, webClientDownloader, savePathDirectory, files.ToList());
                fileDownloadManager.Start();
                Console.WriteLine("Downloading is finished\n");



            }

            Console.ReadKey();

        }

        
    }
}
