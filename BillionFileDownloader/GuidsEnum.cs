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
    class GuidsEnum : IEnumerable
    {
        SqlConnection connection;
        string sqlConnectionString = "Data Source=localhost;Initial Catalog=BillionFileDownloader;Integrated Security=True";
        IDataReader reader;

        public GuidsEnum()
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

        public IEnumerator<Guid> GetEnumerator()
        {
            using (connection = new SqlConnection(sqlConnectionString))
            {
                while (reader.Read())
                {
                    yield return reader.GetGuid(1);
                }
                yield break;
            }


        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<Guid> getIDs()
        {
            return (IEnumerable<Guid>)this;
        }
    }
}
