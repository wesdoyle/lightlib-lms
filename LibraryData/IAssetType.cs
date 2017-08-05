using LibraryData.Models;
using System;
using System.Collections.Generic;

namespace LibraryData
{
    public interface IAssetType
    {
        IEnumerable<AssetType> GetAll();
        AssetType Get(int id);
        void Add(AssetType newType);
    }
}
