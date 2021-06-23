using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionFileDownloader
{
    public class FilesDBManager
    {

        public int FilesCountInSegment { get; set; }

        string sqlConnectionString;

        object getLockObject;
        object setLockObject;

        public FilesDBManager(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
            this.getLockObject = new object();
            this.setLockObject = new object();
            FilesCountInSegment = 5;
        }

        public List<FileObject> GetNextFilesList()
        {
            //string sqlConnectionString = "Data Source=localhost;Initial Catalog=BillionFileDownloader;Integrated Security=True";

            using (IDbConnection connection = new SqlConnection(sqlConnectionString))
            {
                //int 
                //IDbCommand command = new SqlCommand($"Select top {FilesCountInSegment} * from files where statusId = {(int)FileStates.ToDownload}");
                IDbCommand command = new SqlCommand($"getNextSegment");
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;

                //command.

                connection.Open();

                IDataReader reader = command.ExecuteReader();
                List<FileObject> filesProxies = new List<FileObject>();


                while (reader.Read())
                {
                    FileObject fileProxy = new FileObject();

                    string dbData = "";
                    for (int i = 0; i < reader.FieldCount; i++)
                        dbData += reader[i] + " | ";

                    fileProxy.Id = reader.GetInt32(0);
                    fileProxy.FilePath = reader.GetString(2);
                    fileProxy.Guid = reader.GetGuid(1);
                    fileProxy.StatusId = reader.GetInt32(3);
                    filesProxies.Add(fileProxy);


                    Console.WriteLine(dbData);
                }
                return filesProxies;
            }
        }

        public void SetFileObjectDownloaded(FileObject fileProxy)
        {
            using (IDbConnection connection = new SqlConnection(sqlConnectionString))
            {
                IDbCommand command = new SqlCommand($"Update files set statusId = {(int)FileStates.Downloaded} where Id="+fileProxy.Id);
                command.Connection = connection;
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
            }
        }

        public void SetFileObjectIsDownloading(FileObject fileProxy)
        {
            using (IDbConnection connection = new SqlConnection(sqlConnectionString))
            {
                IDbCommand command = new SqlCommand($"Update files set statusId = {(int)FileStates.IsDownloading} where Id=" + fileProxy.Id);
                command.Connection = connection;
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
            }
        }

        public void SetFileDownloadIsFaulted(FileObject fileProxy, string message)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                SqlCommand command = new SqlCommand($"Update files set statusId = {(int)FileStates.Faulted} where Id=" + fileProxy.Id);
                command.Connection = connection;
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                command.CommandText = $"insert into DownloadErrors (fileId,ErrorMessage) values({fileProxy.Id}, @ErrorMessage)";
                command.Parameters.AddWithValue("@ErrorMessage", fileProxy.ErrorMessage);
                command.ExecuteNonQuery();
            }
        }

        public void SetAllFilesToDownload()
        {
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                //int 
                SqlCommand command = new SqlCommand("setAllFilesToDownload");
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
            }
        }

        public void SetAllDownloadingFilesToDownload()
        {
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                //int 
                SqlCommand command = new SqlCommand("setDownloadingFilesToDownload");
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = txtFirstName.Text;
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
            }
        }
    }

    public enum FileStates
    {
        ToDownload = 1,
        IsDownloading = 2,
        Downloaded = 3,
        Faulted = 4
    }
}
