using System.Collections.Generic;
using LibraryData.Models;

namespace LibraryData
{
    public interface IHoldService
    {
        IEnumerable<AssetType> GetAll();
        AssetType Get(int id);
        void Add(AssetType newType);
    }
}