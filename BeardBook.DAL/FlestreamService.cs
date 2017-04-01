using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using File = BeardBook.Entities.File;

namespace BeardBook.DAL
{
    public class FilestreamService
    {
        private readonly BeardBookDbContext _context;
        private const string BaseSelectStatement =
            @"SELECT Id, Data.PathName() AS 'Path', GET_FILESTREAM_TRANSACTION_CONTEXT() AS 'Transaction' FROM {0} WHERE Id = @id";

        public FilestreamService(BeardBookDbContext context)
        {
            _context = context;
        }

        public byte[] GetFileData(int fileId)
        {
            byte[] bytes;

            var selectStatement = string.Format(BaseSelectStatement, "[dbo].[Files]");

            var rowData = _context.Database
                .SqlQuery<RowData>(selectStatement, new SqlParameter("id", fileId))
                .First();

            using (var source = new SqlFileStream(rowData.Path, rowData.Transaction, FileAccess.Read))
            {
                var buffer = new byte[16 * 1024];
                using (var destination = new MemoryStream())
                {
                    int bytesRead;
                    while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        destination.Write(buffer, 0, bytesRead);
                    }
                    bytes = destination.ToArray();
                }
            }
            return bytes;
        }

        public void SaveFileData(File file)
        {
            if (file == null) return;

            var selectStatement = string.Format(BaseSelectStatement, "[dbo].[Files]");

            var rowData = _context.Database
                .SqlQuery<RowData>(selectStatement, new SqlParameter("id", file.Id))
                .First();

            using (var destination = new SqlFileStream(rowData.Path, rowData.Transaction, FileAccess.Write))
            {
                var buffer = new byte[16 * 1024];
                using (var source = new MemoryStream(file.Data))
                {
                    int bytesRead;
                    while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        destination.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }
    }

    public class RowData
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public byte[] Transaction { get; set; }
    }
}
