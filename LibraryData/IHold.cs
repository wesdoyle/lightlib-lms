using LibraryData.Models;
using System.Collections.Generic;

namespace LibraryData
{
    public interface IHold
    {
        IEnumerable<AssetType> GetAll();
        AssetType Get(int id);
        void Add(AssetType newType);
    }
}
