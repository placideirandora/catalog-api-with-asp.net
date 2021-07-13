using Catalog.Api.Entities;
using static Catalog.Api.Dtos;

namespace Catalog.Api
{
    public static class Extensions
    {
        public static GetItemDto AsDto(this Item item)
        {
            return new GetItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}