using System.Collections.Generic;
using Library.Data.Models;

namespace Library.Service.Interfaces
{
    public interface IStatusService
    {
        IEnumerable<Status> GetAll();
        Status Get(int id);
        void Add(Status newStatus);
    }
}