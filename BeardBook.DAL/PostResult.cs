using System.Collections.Generic;
using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class PostResult
    {
        public PostResult(Post post, ICollection<FileResult> fileResults)
        {
            Post = post;
            FileResults = fileResults;
        }

        public Post Post { get; private set; }
        public ICollection<FileResult> FileResults { get; private set; }
    }
}