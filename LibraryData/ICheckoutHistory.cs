using LibraryData.Models;
using System.Collections.Generic;

namespace LibraryData
{
    public interface ICheckoutHistory
    {
        IEnumerable<CheckoutHistory> GetAll();
        IEnumerable<CheckoutHistory> GetForAsset(LibraryAsset asset);
        IEnumerable<CheckoutHistory> GetForCard(LibraryCard card);
        CheckoutHistory Get(int id);
        void Add(CheckoutHistory newCheckoutHistory);
    }
}
