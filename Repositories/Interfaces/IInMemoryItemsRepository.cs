using System;
using CatalogApi.Entities;
using System.Collections.Generic;

namespace CatalogApi.Repositories.Interfaces
{
    public interface IInMemoryItemsRepository
    {
        Item GetItem(Guid id);
        IEnumerable<Item> GetItems();
        void CreateItem(Item item);
    }
}
