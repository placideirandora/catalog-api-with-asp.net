using Moq;
using Xunit;
using System;
using FluentAssertions;
using Catalog.Api.Entities;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using static Catalog.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Catalog.Api.Repositories.Interfaces;

namespace Catalog.UnitTests
{
    public class ItemsControllerTests
    {
        private readonly Random rand = new();
        private readonly Mock<ILogger<ItemsController>> loggerStub = new();
        private readonly Mock<IMongoDbItemsRepository> repositoryStub = new();


        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync((Item)null);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
        {
            // Arrange
            var expectedItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(expectedItem);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(expectedItem);
        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
        {
            // Arrange
            var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };

            repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(expectedItems);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var actualItems = await controller.GetItemsAsync();

            // Assert
            actualItems.Should().BeEquivalentTo(expectedItems);
        }

        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            // Arrange
            var itemToCreate = new CreateItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), rand.Next(1000));

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.CreateItemAsync(itemToCreate);

            // Assert
            var createdItem = (result.Result as CreatedAtActionResult).Value as GetItemDto;

            itemToCreate.Should().BeEquivalentTo(createdItem,
                 options => options.ComparingByMembers<GetItemDto>().ExcludingMissingMembers());

            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        }

        [Fact]
        public async Task UpdateItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync((Item)null);

            var itemToUpdate = new UpdateItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), rand.Next(1000));

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.UpdateItemAsync(Guid.NewGuid(), itemToUpdate);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            var existingItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(existingItem);

            var itemId = existingItem.Id;
            var itemToUpdate = new UpdateItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), existingItem.Price + 3);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync((Item)null);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.DeleteItemAsync(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            var existingItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(existingItem);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.DeleteItemAsync(existingItem.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems()
        {
            // Arrange
            var allItems = new[]
            {
                new Item() { Name = "Potion" },
                new Item() { Name = "Antidote" },
                new Item() { Name = "Hi-Potion" },

             };

            var nameToMatch = "Potion";

            repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(allItems);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            IEnumerable<GetItemDto> foundItems = await controller.GetItemsAsync(nameToMatch);

            // Assert
            foundItems.Should().OnlyContain(item => item.Name == allItems[0].Name || item.Name == allItems[2].Name);
        }


        private Item CreateRandomItem()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
