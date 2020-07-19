using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SalonAPI.Domain;

namespace SalonAPI.Repository
{
    public class ContactRepository : IContactRepository
    {
        // TODO: use using! new connection per request
        private readonly MySqlConnection _connection;

        public ContactRepository(IConfiguration configuration)
        {
            _connection = new MySqlConnection(connectionString: configuration.GetConnectionString("LocalMySQL"));
        }
        
        public async Task<int> AddContact(Contact contact)
        {
            await _connection.OpenAsync();
            
            MySqlCommand insertCmd = new MySqlCommand(
                @"INSERT INTO contact (Name, Phone, Email, CreatedDate) VALUES (@Name, @Phone, @Email, @CreatedDate)", 
                _connection);

            insertCmd.Parameters.AddWithValue("@Name", contact.Name);
            insertCmd.Parameters.AddWithValue("@Phone", contact.Phone);
            insertCmd.Parameters.AddWithValue("@Email", contact.Email);
            insertCmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToShortDateString());

            var inserted = await insertCmd.ExecuteNonQueryAsync();

            if (inserted == 0) return 0;
            
            MySqlCommand GetIdCmd = new MySqlCommand(@"SELECT LAST_INSERT_ID();", _connection);
            var contactID = await GetIdCmd.ExecuteScalarAsync();

            await _connection.CloseAsync();
            
            return Convert.ToInt32(contactID);
        }
    }
}