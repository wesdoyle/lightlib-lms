using System.Threading.Tasks;
using Library.Data.Models;
using Library.Service.Models;

namespace Library.Service.Interfaces {
    public interface IHoldService {
        Task<PagedServiceResult<AssetType>> GetAll(int page, int perPage);
        Task<ServiceResult<AssetType>> Get(int id);
        Task<ServiceResult<int>> Add(AssetType newType);
    }
}