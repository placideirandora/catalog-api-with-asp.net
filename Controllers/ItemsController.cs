using System;
using System.Linq;
using CatalogApi.Dtos;
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
        public ActionResult<IEnumerable<GetItemDto>> GetItems()
        {
            var items = _repository.GetItems().Select(item => item.AsDto());

            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<GetItemDto> GetItem(Guid id)
        {

            var item = _repository.GetItem(id);

            if (item is null)
            {
                return NotFound();
            }

            return Ok(item.AsDto());
        }
    }
}
