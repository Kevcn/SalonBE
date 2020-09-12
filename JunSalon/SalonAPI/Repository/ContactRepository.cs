using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
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

        public async Task<int> GetContactID(Contact contact)
        {
            const string getContactID = @"
            SELECT
                ID
            FROM contact
            WHERE Name = @Name
                AND Phone = @Phone";

            try
            {
                await using var _connection = new MySqlConnection(_mySqlConfig.ConnectionString);
                var contactID = await _connection.QueryAsync<int>(getContactID, new
                {
                    contact.Name, contact.Phone
                });

                var contactId = contactID.ToList();
                return contactId.Any() ? contactId.Single() : 0;
            }
            catch (MySqlException exception)
            {
                // TODO: log expection
                Console.WriteLine(exception);
                throw;
            }
            catch (InvalidOperationException exception)
            {
                // TODO: log expection
                Console.WriteLine(exception);
                throw;
            }
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
                await using var _connection = new MySqlConnection(_mySqlConfig.ConnectionString);
                var contactID = await _connection.QueryAsync<int>(insertContactStatement,
                    new
                    {
                        contact.Name,
                        contact.Phone,
                        contact.Email,
                        CreatedDate = DateTime.Now
                    });

                return contactID.Single();
            }
            catch (MySqlException exception)
            {
                // TODO: log expection
                Console.WriteLine(exception);
                throw;
            }
            catch (InvalidOperationException exception)
            {
                // TODO: log expection
                Console.WriteLine(exception);
                throw;
            }
        }

        public async Task<Contact> CheckDuplicate(Contact contact)
        {
            const string checkDuplicateContact = @"
            SELECT *
            FROM contact 
            WHERE Name = @Name 
                AND Phone = @Phone";

            try
            {
                await using var _connection = new MySqlConnection(_mySqlConfig.ConnectionString);
                var contactFound = await _connection.QueryAsync<Contact>(checkDuplicateContact, new
                {
                    contact.Name, contact.Phone
                });

                if (!contactFound.Any()) return new Contact();

                return contactFound.SingleOrDefault();
            }
            catch (MySqlException exception)
            {
                // TODO: log expection
                Console.WriteLine(exception);
                throw;
            }
            catch (InvalidOperationException exception)
            {
                // TODO: log expection
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}