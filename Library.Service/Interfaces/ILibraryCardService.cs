using System.Collections.Generic;
using Library.Data.Models;

namespace Library.Service.Interfaces
{
    public interface ILibraryCardService
    {
        IEnumerable<LibraryCard> GetAll();
        LibraryCard Get(int id);
        void Add(LibraryCard newCard);
    }
}