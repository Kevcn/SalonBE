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
        private readonly IConfiguration _configuration;


        public AppointmentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<BookingRecord>> GetAppointmentsByDay(DateTime startDate, DateTime endDate)
        {
            var bookingRecords = new List<BookingRecord>();
            
            await using (var _connection = new MySqlConnection(connectionString: _configuration.GetConnectionString("LocalMySQL")))
            {
                await _connection.OpenAsync();

                MySqlCommand cmd = new MySqlCommand(
                    @"SELECT TimeSlotID, Date FROM bookingrecord WHERE Date > @StartDate AND Date < @EndDate",
                    _connection);

                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                var reader = await cmd.ExecuteReaderAsync();

                while (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        var bookingRecord = new BookingRecord
                        {
                            Date = (DateTime) reader["Date"],
                            TimeSlotID = (int) reader["TimeSlotID"]
                        };

                        bookingRecords.Add(bookingRecord);
                    }
                    await reader.NextResultAsync();
                }

                return bookingRecords;
            }
        }

        public async Task<List<BookingRecord>> GetSingleDayAppointments(DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddAppointment(BookingRecord bookingRecord, int contactID)
        {
            await using (var _connection = new MySqlConnection(connectionString: _configuration.GetConnectionString("LocalMySQL")))
            {
                await _connection.OpenAsync();

                MySqlCommand cmd = new MySqlCommand(
                    @"INSERT INTO bookingrecord (ContactID, TimeslotID, Date, Description, CreatedDate) VALUES (@ContactID, @TimeSlotID, @Date, @Description, @CreatedDate)",
                    _connection);

                cmd.Parameters.AddWithValue("@ContactID", contactID);
                cmd.Parameters.AddWithValue("@TimeSlotID", bookingRecord.TimeSlotID);
                cmd.Parameters.AddWithValue("@Date", bookingRecord.Date);
                cmd.Parameters.AddWithValue("@Description", bookingRecord.Description);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToShortDateString());

                int inserted = await cmd.ExecuteNonQueryAsync();

                return inserted > 0;
            }
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