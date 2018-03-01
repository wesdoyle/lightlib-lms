using System.Collections.Generic;
using LibraryData.Models;

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