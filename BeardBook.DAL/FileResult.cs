using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class FileResult
    {
        public FileResult(File file, File thumbnail)
        {
            File = file;
            Thumbnail = thumbnail;
        }
        public File File { get; private set; }
        public File Thumbnail { get; private set; }
    }
}