using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightLib.Models;
using LightLib.Models.DTOs;
using LightLib.Service.Interfaces;
using LightLib.Web.Models.Patron;
using Microsoft.AspNetCore.Mvc;

namespace LightLib.Web.Controllers {
    
    public class PatronController : LibraryController {
        private readonly IPatronService _patronService;

        public PatronController(IPatronService patronService) {
            _patronService = patronService;
        }

        public async Task<IActionResult> Index([FromQuery] int page, [FromQuery] int perPage) {
            var serviceResult = await _patronService.GetPaginated(page, perPage);

            if (serviceResult != null && serviceResult.Results.Any()) {
                var allAssets = serviceResult.Results.ToList();
                var viewModel = new PatronIndexModel {
                    PageOfPatrons = new PaginationResult<PatronDto> {
                        Results = allAssets 
                    }
                };

                return View(viewModel);
            }
            
            var emptyModel = new PatronIndexModel {
                PageOfPatrons = new PaginationResult<PatronDto> {
                    Results = new List<PatronDto>(),
                    PerPage = perPage,
                    PageNumber = page
                }
            };
            
            return View(emptyModel);
        }

        public async Task<IActionResult> Detail(int id) {
            var serviceResult = await _patronService.Get(id);
            throw new NotImplementedException();
        }
    }
}