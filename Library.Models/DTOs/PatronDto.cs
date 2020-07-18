using System;
using Library.Data.Models;

namespace Library.Models.DTOs {
    public class PatronDto {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Telephone { get; set; }
        public string Gender { get; set; }
        public LibraryCard LibraryCard { get; set; }
        public LibraryBranch HomeLibraryBranch { get; set; }
    }
}