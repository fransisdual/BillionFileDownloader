using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    class Program
    {
        static void Main(string[] args)
        {

            Logger.Source = new ConsoleLogger();

            string filePath = Directory.GetCurrentDirectory() + "\\FileList.txt";
            string savePathDirectory = Directory.GetCurrentDirectory() + "\\Downloads";

            bool isFileExist = File.Exists(filePath);

            if (isFileExist)
            {
                SimpleWebRepository simpleRepository = new SimpleWebRepository(filePath);


                Console.WriteLine("Start consistently downloading\n");
                IFileDownloader fileDownloader = new SimpleFileDownloader(simpleRepository, savePathDirectory);
                fileDownloader.Download();
                Console.WriteLine("Downloading is finished\n");

                Console.WriteLine("Start parralel downloading\n");
                fileDownloader = new MultiThreadFileDownloader(simpleRepository, savePathDirectory);
                fileDownloader.Download();
                Console.WriteLine("Downloading is finished\n");

            }

            Console.ReadKey();

        }


    }
}
