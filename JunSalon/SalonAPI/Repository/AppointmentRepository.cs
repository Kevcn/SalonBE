using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using MySql.Data.MySqlClient;
using SalonAPI.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SalonAPI.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Dapper;


namespace SalonAPI.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly MySqlConfig _mySqlConfig;

        public AppointmentRepository(IOptions<MySqlConfig> mySqlConfig)
        {
            _mySqlConfig = mySqlConfig.Value;

        }

        public async Task<List<BookingRecord>> GetAppointmentsByDay(DateTime startDate, DateTime endDate)
        {
            const string selectBookingRecords = @"
            SELECT 
                TimeSlotID, 
                Date 
            FROM bookingrecord 
            WHERE Date > @StartDate 
                AND Date < @EndDate";
            
            try
            {
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.Local);
                var bookingRecords = await _connection.QueryAsync<BookingRecord>(selectBookingRecords, new
                {
                    StartDate = startDate,
                    EndDate = endDate
                });

                return bookingRecords.ToList();
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

        public async Task<List<BookingRecord>> GetSingleDayAppointments(DateTime date)
        {
            const string selectBookingRecords = @"
            SELECT 
                TimeSlotID, 
                Date 
            FROM bookingrecord 
            WHERE Date = @Date";
            
            try
            {
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.Local);
                var bookingRecords = await _connection.QueryAsync<BookingRecord>(selectBookingRecords, new
                {
                    Date = date
                });

                return bookingRecords.ToList();
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

        public async Task<bool> AddAppointment(BookingRecord bookingRecord, int contactID)
        {
            const string addAppointment = @"
            INSERT INTO bookingrecord (
                ContactID, 
                TimeSlotID, 
                Date, 
                Description,
                CreatedDate) 
            VALUES (
                @ContactID, 
                @TimeSlotID, 
                @Date, 
                @Description, 
                @CreatedDate);";

            try
            {
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.Local);
                var inserted = await _connection.ExecuteAsync(addAppointment,
                    new
                    {
                        ContactID = contactID,
                        TimeslotID = bookingRecord.TimeSlotID,
                        Date = bookingRecord.Date,
                        Description = bookingRecord.Description,
                        CreatedDate = DateTime.Now.ToShortDateString()
                    });

                return inserted > 0;

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

        public async Task<bool> VerifyTimeSlotAvailable(BookingRecord bookingRecord)
        {
            const string verifyTimeSlot = @"
            SELECT 
                Count(*)
            FROM bookingrecord 
            WHERE Date = @Date
                AND TimeSlotID = @TimeSlotID";
            
            try
            {
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.Local);
                var bookingRecords = await _connection.QueryAsync<int>(verifyTimeSlot, new
                {
                    Date = bookingRecord.Date,
                    TimeSlotID = bookingRecord.TimeSlotID
                });

                return bookingRecords.Single() > 0;
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