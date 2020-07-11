using System.Collections.Generic;
using Library.Data.Models;

namespace Library.Service.Interfaces {
    public interface IHoldService {
        IEnumerable<AssetType> GetAll();
        AssetType Get(int id);
        void Add(AssetType newType);
    }
}