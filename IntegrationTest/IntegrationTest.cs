using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Financial_Market_API.Data;
using Financial_Market_API;

namespace IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        //public IntegrationTest()
        //{
        //    var appFactory = new WebApplicationFactory<Startup>()
        //        .WithWebHostBuilder(builder =>
        //        {
        //            // Empty this builder to test on real DB
        //            builder.ConfigureServices(services =>
        //            {
        //                // All datacontext is using the real SQL server
        //                services.RemoveAll(typeof(ApplicationDbContext));
        //                // ### Create a in memory database for testing
        //                services.AddDbContext<ApplicationDbContext>(options =>
        //                {
        //                    options.UseInMemoryDatabase("InMemoryTestDb");
        //                });
        //            });
        //        });
        //    // Create virtual client for testing
        //    TestClient = appFactory.CreateClient();
        //}

        //protected async Task AuthenticateAsync()
        //{
        //    TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJWTAsync());
        //}

        //private async Task<string> GetJWTAsync()
        //{
        //    var response = await TestClient.PostAsync
        //}


    }
}
