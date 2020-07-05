using System.Collections.Generic;
using Library.Data.Models;

namespace Library.Data
{
    public interface IStatusService
    {
        IEnumerable<Status> GetAll();
        Status Get(int id);
        void Add(Status newStatus);
    }
}