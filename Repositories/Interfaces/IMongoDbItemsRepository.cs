using System;
using CatalogApi.Entities;
using System.Collections.Generic;

namespace CatalogApi.Repositories.Interfaces
{
    public interface IMongoDbItemsRepository
    {
        Item GetItem(Guid id);
        IEnumerable<Item> GetItems();
        void CreateItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(Guid id);
    }
}