using LibraryData.Models;
using System.Collections.Generic;

namespace LibraryData
{
    public interface ILibraryBranch 
    {
        IEnumerable<LibraryBranch> GetAll();
        IEnumerable<Patron> GetPatrons(int branchId);
        IEnumerable<LibraryAsset> GetAssets(int branchId);
        LibraryBranch Get(int branchId);
        void Add(LibraryBranch newBranch);
        IEnumerable<string> GetBranchHours(int branchId);
        bool IsBranchOpen(int branchId);
        int GetAssetCount(IEnumerable<LibraryAsset> libraryAssets);
        int GetPatronCount(IEnumerable<Patron> patrons);
        decimal GetAssetsValue(int id);
    }
}
