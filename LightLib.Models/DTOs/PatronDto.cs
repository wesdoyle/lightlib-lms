using System;

namespace LightLib.Models.DTOs {
    public class PatronDto {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Telephone { get; set; }
        public string Gender { get; set; }
        public LibraryCardDto LibraryCard { get; set; }
        public LibraryBranchDto HomeLibraryBranch { get; set; }
        public int LibraryCardId { get; set;  }
        public decimal OverdueFees { get; set;  }
        public string HomeLibrary { get; set;  }
    }
}