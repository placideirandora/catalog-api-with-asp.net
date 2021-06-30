using CatalogApi.Entities;
using CatalogApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly InMemoryItemsRepository repository;

        public ItemsController()
        {
            repository = new InMemoryItemsRepository();
        }

        [HttpGet]
        public IEnumerable<Item> GetItems()
        {
            var items = repository.GetItems();

            return items;
        }
    }
}
