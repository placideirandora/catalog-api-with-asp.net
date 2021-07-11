using System;
using Catalog.Api.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Catalog.Api.Repositories.Interfaces
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
