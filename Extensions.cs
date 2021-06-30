using CatalogApi.Dtos;
using CatalogApi.Entities;

namespace CatalogApi
{
    public static class Extensions
    {
        public static GetItemDto AsDto(this Item item)
        {
            return new GetItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate
            };
        }
    }
}