using System;
using CatalogApi.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CatalogApi.Repositories.Interfaces
{
    public interface IInMemoryItemsRepository
    {
        Task<Item> GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task CreateItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(Guid id);
    }
}
