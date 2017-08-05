using LibraryData.Models;
using System.Collections.Generic;

namespace LibraryData
{
    public interface IVideo
    {
        IEnumerable<Video> GetAll();
        IEnumerable<Video> GetByDirector(string author);
        Video Get(int id);
        void Add(Video newVideo);
    }
}
