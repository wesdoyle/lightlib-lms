using System.Collections.Generic;
using Library.Data.Models;

namespace Library.Data
{
    public interface IVideoService
    {
        IEnumerable<Video> GetAll();
        IEnumerable<Video> GetByDirector(string author);
        Video Get(int id);
        void Add(Video newVideo);
    }
}