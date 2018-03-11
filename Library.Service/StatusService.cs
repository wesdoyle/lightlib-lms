using System.Collections.Generic;
using System.Linq;
using Library.Data;
using Library.Data.Models;

namespace Library.Service
{
    public class StatusService : IStatusService
    {
        private readonly LibraryDbContext _context;

        public StatusService(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(Status newStatus)
        {
            _context.Add(newStatus);
            _context.SaveChanges();
        }

        public Status Get(int id)
        {
            return _context.Statuses.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Status> GetAll()
        {
            return _context.Statuses;
        }
    }
}