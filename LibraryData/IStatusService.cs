using System.Collections.Generic;
using LibraryData.Models;

namespace LibraryData
{
    public interface IStatusService
    {
        IEnumerable<Status> GetAll();
        Status Get(int id);
        void Add(Status newStatus);
    }
}