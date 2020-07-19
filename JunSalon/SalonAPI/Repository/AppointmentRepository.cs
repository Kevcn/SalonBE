using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using MySql.Data.MySqlClient;
using SalonAPI.Domain;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace SalonAPI.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly MySqlConnection _connection;

        public AppointmentRepository(IConfiguration configuration)
        {
            _connection = new MySqlConnection(connectionString: configuration.GetConnectionString("LocalMySQL"));
        }

        public async Task<List<BookingRecord>> GetAppointmentsByDay(DateTime startDate, DateTime endDate)
        {
            await _connection.OpenAsync();
            MySqlCommand cmd = new MySqlCommand("", _connection);
            
            
            throw new NotImplementedException();
        }

        public async Task<List<BookingRecord>> GetSingleDayAppointments(DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddAppointment(BookingRecord bookingRecord, int contactID)
        {
            await _connection.OpenAsync();
            MySqlCommand cmd = new MySqlCommand("", _connection);
            
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveAppointment(BookingRecord bookingRecord)
        {
            throw new NotImplementedException();
        }

        public async Task<BookingRecord> GetRecord(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}