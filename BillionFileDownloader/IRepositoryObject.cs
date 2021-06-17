using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    public interface IRepositoryObject
    {

        public string getStringPath();

        public bool Download(string savePath);
    }
}
