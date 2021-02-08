using System;
using LightLib.Models;
using LightLib.Models.DTOs;

namespace LightLib.Web.Models.Patron {
    public class PatronDetailModel {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? LibraryCardId { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string HomeLibrary { get; set; }
        public decimal? OverdueFees { get; set; }
        public string HasBeenMemberFor { get; set; }
        public PaginationResult<CheckoutDto> AssetsCheckedOut { get; set; }
        public PaginationResult<CheckoutHistoryDto> CheckoutHistory { get; set; }
        public PaginationResult<HoldDto> Holds { get; set; }
    }
}