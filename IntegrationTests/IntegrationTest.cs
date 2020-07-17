using Financial_Market_API;
using Financial_Market_API.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Contracts.V1;
using Contracts.V1.Requests;
using Contracts.V1.Responses;

namespace IntegrationTests
{
    public class IntegrationTest // Implement IDisposable to work in .NET Core 3.0 +
    {
        protected readonly HttpClient TestClient;
        private readonly IServiceProvider _serviceProvider;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    // Empty this builder to test on real DB
                    builder.ConfigureServices(services =>
                    {
                        // All datacontext is using the real SQL server
                        services.RemoveAll(typeof(ApplicationDbContext));
                        // ###Create a in memory database for testing
                        // TBC - Not using in memory DB
                        services.AddDbContext<ApplicationDbContext>(options => { options.UseInMemoryDatabase("InMemoryTestDb"); });
                    });
                });
            _serviceProvider = appFactory.Services;
            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        protected async Task<StockResponse> CreateStockAsync(CreateStockRequest request)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Stock.Create, request);
            return await response.Content.ReadAsAsync<StockResponse>();
        }


        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {
                Email = "test969@integration.com",
                Password = "SomePass12324!"
            });

            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }

        //public void Dispose()
        //{
        //    using var serviceScope = _serviceProvider.CreateScope();
        //    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        //    context.Database.EnsureDeleted();
        //}
    }
}
