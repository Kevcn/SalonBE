using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;
using SalonAPI.Domain;

namespace SalonAPI.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly IConfiguration _configuration;

        public ContactRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task<int> AddContact(Contact contact)
        {
            const string insertContactStatement = @"
            INSERT INTO contact (
                Name, 
                Phone, 
                Email, 
                CreatedDate) 
            VALUES (
                @Name,
                @Phone,
                @Email,
                @CreatedDate);
            SELECT LAST_INSERT_ID();";

            try
            {
                await using var _connection =
                    new MySqlConnection(connectionString: _configuration.GetConnectionString("LocalMySQL"));
                var contactID = await _connection.QueryAsync<int>(insertContactStatement,
                    new
                    {
                        Name = contact.Name,
                        Phone = contact.Phone,
                        Email = contact.Email,
                        CreatedDate = DateTime.Now.ToShortDateString()
                    });

                return contactID.Single();

            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            catch(InvalidOperationException exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}