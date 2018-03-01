using System.Collections.Generic;
using LibraryData.Models;

namespace LibraryData
{
    public interface IAssetType
    {
        IEnumerable<AssetType> GetAll();
        AssetType Get(int id);
        void Add(AssetType newType);
    }
}