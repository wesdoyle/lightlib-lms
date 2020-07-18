using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Data;
using Library.Data.Models;
using Library.Models;
using Library.Models.DTOs;
using Library.Service.Interfaces;
using Library.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Service {
    public class LibraryAssetService : ILibraryAssetService {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPaginator<LibraryAsset> _paginator;

        public LibraryAssetService(
            LibraryDbContext context, 
            IMapper mapper, 
            IPaginator<LibraryAsset> paginator) {
            _context = context;
            _mapper = mapper;
            _paginator = paginator;
        }

        public async Task<ServiceResult<int>> Add(LibraryAssetDto assetDto) {
            var newAsset = _mapper.Map<LibraryAsset>(assetDto);
            await _context.AddAsync(newAsset);
            await _context.SaveChangesAsync();
            return new ServiceResult<int> {
                Data = newAsset.Id,
                Error = null
            };
        }

        public async Task<ServiceResult<LibraryAssetDto>> Get(int id) {
            var asset = await _context.LibraryAssets
                .Include(a => a.Status)
                .Include(a => a.Location)
                .FirstOrDefaultAsync(a => a.Id == id);
            var assetDto = _mapper.Map<LibraryAssetDto>(asset);
            return new ServiceResult<LibraryAssetDto> {
                Data = assetDto,
                Error = null
            };
        }

        public Task<ServiceResult<int>> Add(AssetTypeDto newType) {
            throw new NotImplementedException();
        }

        public async Task<PagedServiceResult<LibraryAssetDto>> GetAll(int page, int perPage) {
            var assets = _context.LibraryAssets
                .Include(a => a.Status)
                .Include(a => a.Location);

            var pageOfAssets = await _paginator
                .BuildPageResult(assets, page, perPage, a => a.Title)
                .ToListAsync();

            var pageOfAssetDtos = _mapper
                .Map<List<LibraryAssetDto>>(pageOfAssets);
            
            return new PagedServiceResult<LibraryAssetDto> {
                Data = new PaginationResult<LibraryAssetDto> {
                    PageNumber = page,
                    PerPage = perPage,
                    Results = pageOfAssetDtos 
                }
            };
        }

        public async Task<ServiceResult<string>> GetAuthorOrDirector(int id) {
            var isBook = await _context.LibraryAssets
                .OfType<Book>()
                .AnyAsync(a => a.Id == id);

            return isBook
                ? GetAuthor(id)
                : GetDirector(id);
        }

        public async Task<ServiceResult<LibraryBranchDto>> GetCurrentLocation(int assetId) {
            var asset = await _context
                .LibraryAssets
                .FirstOrDefaultAsync(a => a.Id == assetId);
            
            var location = asset.Location;
            var locationDto = _mapper.Map<LibraryBranchDto>(location);
            
            return new ServiceResult<LibraryBranchDto> {
                Data = locationDto,
                Error = null
            };
        }

        public async Task<ServiceResult<string>> GetDeweyIndex(int id) {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<string>> GetType(int id) {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<string>> GetTitle(int id) {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<string>> GetIsbn(int id) {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<LibraryCardDto>> GetLibraryCardByAssetId(int id) {
            throw new NotImplementedException();
        }
    }
}