using Contracts.V1;
using Contracts.V1.Requests;
using Financial_Market_API.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class StockControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutAnyStocks_ReturnsEmptyResponse()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Stock.GetAll);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<Stock>>()).Should().NotBeEmpty();
        }

        [Fact]
        public async Task Get_ReturnsStock_WhenStockExistsInTheDB()
        {
            // Arrange
            await AuthenticateAsync();
            var createdStock = await CreateStockAsync(new CreateStockRequest
            {
                Name = "Test Stock1",
                Company = "Test Company1"
            });

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Stock.Get.Replace("{stockId}", createdStock.Id.ToString()));
            var returnedStock = await response.Content.ReadAsAsync<Stock>();

            // Assert
            // should return 200 upon stock created
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            returnedStock.Id.Should().Be(createdStock.Id);
            returnedStock.Name.Should().Be("Test Stock1");
            returnedStock.Company.Should().Be("Test Company1");

        }

    }
}
