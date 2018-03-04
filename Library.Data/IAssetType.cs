using System.Collections.Generic;
using Library.Data.Models;

namespace Library.Data
{
    public interface IAssetType
    {
        IEnumerable<AssetType> GetAll();
        AssetType Get(int id);
        void Add(AssetType newType);
    }
}