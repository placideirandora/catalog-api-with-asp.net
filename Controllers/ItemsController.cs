using System;
using CatalogApi.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CatalogApi.Repositories.Interfaces;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IInMemoryItemsRepository _repository;

        public ItemsController(IInMemoryItemsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Item>> GetItems()
        {
            var items = _repository.GetItems();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(Guid id)
        {

            var item = _repository.GetItem(id);

            if (item is null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}
