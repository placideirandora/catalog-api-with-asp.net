using System;
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
        public ActionResult<IEnumerable<Item>> GetItems()
        {
            var items = repository.GetItems();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(Guid id)
        {

            Console.WriteLine("ID RECIEVED: ", id);

            var item = repository.GetItem(id);

            if (item is null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}
