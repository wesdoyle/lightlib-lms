using System;

namespace Library.Models.DTOs {
    public class PatronDto {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Telephone { get; set; }
        public string Gender { get; set; }
        public LibraryCardDto LibraryCard { get; set; }
        public LibraryBranchDto HomeLibraryBranch { get; set; }
    }
}