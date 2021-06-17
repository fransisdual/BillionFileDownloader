using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    public class SimpleWebRepository : IRepository
    {
        public string SourceLinksFilepath { get; }
        List<IRepositoryObject> repositoryObjects;


        public SimpleWebRepository(string sourceLinksFilepath)
        {
            SourceLinksFilepath = sourceLinksFilepath;
            repositoryObjects = new List<IRepositoryObject>();
            LoadObjectsFromFile();

        }

        public IList<IRepositoryObject> GetRepositoryObjects()
        {
            return repositoryObjects;
        }

        private void LoadObjectsFromFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(SourceLinksFilepath, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        repositoryObjects.Add(new WebRepositoryObject(line));
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.Source.LogError(ex.Message);
            }
        }

    }
}
