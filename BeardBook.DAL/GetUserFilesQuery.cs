using System.Collections.Generic;
using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class GetUserFilesQuery : IQuery<IEnumerable<FileResult>>
    {
        public GetUserFilesQuery(int userId, FileType fileType)
        {
            UserId = userId;
            FileType = fileType;
        }

        public int UserId { get; private set; }
        public FileType FileType { get; private set; }
    }
}
