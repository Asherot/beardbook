using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class GetFileQuery : IQuery<File>
    {
        public GetFileQuery(int fileId, int userId)
        {
            FileId = fileId;
            UserId = userId;
        }

        public int FileId { get; private set; }
        public int UserId { get; private set; }
    }
}
