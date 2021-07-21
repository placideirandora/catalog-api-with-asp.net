using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api
{
    public class Dtos
    {
        public record GetItemDto(Guid Id, string Name, string Description, Decimal Price, DateTimeOffset CreatedDate);
        public record CreateItemDto([Required] string Name, string Description, [Range(1, 1000)] Decimal Price);
        public record UpdateItemDto([Required] string Name, string Description, [Range(1, 1000)] Decimal Price);
    }
}