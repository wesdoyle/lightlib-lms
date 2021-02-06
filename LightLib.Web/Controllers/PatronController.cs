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
    /// <summary>
    /// Handles web-layer requests for Library Patrons
    /// </summary>
    public class PatronController : LibraryController {
        private readonly IPatronService _patronService;

        public PatronController(IPatronService patronService) {
            _patronService = patronService;
        }

        /// <summary>
        /// Returns a paginated collection of all Library Patrons
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index([FromQuery] int page, [FromQuery] int perPage) {
            var serviceResult = await _patronService.GetAll(page, perPage);

            if (serviceResult.Error != null) {
                return HandleServerError(serviceResult.Error);
            }

            if (serviceResult.Error != null) {
                return HandleServerError(serviceResult.Error);
            }

            if (serviceResult.Data != null && serviceResult.Data.Results.Any()) {
                var allAssets = serviceResult.Data.Results.ToList();
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
            
            if (serviceResult.Error != null) {
                return HandleServerError(serviceResult.Error);
            }
            
            throw new NotImplementedException();
        }
    }
}