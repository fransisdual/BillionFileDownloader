using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    class DBEnum : IEnumerable<FileObject>, IDisposable
    {
        SqlConnection connection;
        string sqlConnectionString = "Data Source=localhost;Initial Catalog=BillionFileDownloader;Integrated Security=True";
        IDataReader reader;

        public DBEnum()
        {
            connection = new SqlConnection(sqlConnectionString);
            SqlCommand command = new SqlCommand($"Select * from files where statusId = 1");
            command.Connection = connection;
            connection.Open();
            reader = command.ExecuteReader();
        }

        public void Dispose()
        {
            connection.Close();
            reader.Close();
        }

        public IEnumerator<FileObject> GetEnumerator()
        {
            while (reader.Read())
            {
                yield return GetNextFileObject();
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        FileObject GetNextFileObject()
        {
            FileObject fileProxy = new FileObject();

            string dbData = "";
            for (int i = 0; i < reader.FieldCount; i++)
                dbData += reader[i] + " | ";

            fileProxy.Id = reader.GetInt32(0);
            fileProxy.FilePath = reader.GetString(2);
            fileProxy.Guid = reader.GetGuid(1);
            fileProxy.StatusId = reader.GetInt32(3);

            return fileProxy;
        }
    }
}
