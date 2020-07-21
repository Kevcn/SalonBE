using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using SalonAPI.Configuration;
using SalonAPI.Domain;

namespace SalonAPI.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly MySqlConfig _mySqlConfig;

        public ContactRepository(IOptions<MySqlConfig> mySqlConfig)
        {
            _mySqlConfig = mySqlConfig.Value;
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
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.Local);
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
                // TODO: log expection
                Console.WriteLine(exception);
                throw;
            }
            catch(InvalidOperationException exception)
            {
                // TODO: log expection
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}