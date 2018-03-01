using System.Collections.Generic;
using LibraryData.Models;

namespace LibraryData
{
    public interface ILibraryCard
    {
        IEnumerable<LibraryCard> GetAll();
        LibraryCard Get(int id);
        void Add(LibraryCard newCard);
    }
}