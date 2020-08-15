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
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.ConnectionString);
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
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.ConnectionString);
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
                CreatedDate,
                Cancel) 
            VALUES (
                @ContactID, 
                @TimeSlotID, 
                @Date, 
                @Description, 
                @CreatedDate,
                @Cancel);";

            try
            {
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.ConnectionString);
                var inserted = await _connection.ExecuteAsync(addAppointment,
                    new
                    {
                        ContactID = contactID,
                        TimeslotID = bookingRecord.TimeSlotID,
                        Date = bookingRecord.Date,
                        Description = bookingRecord.Description,
                        CreatedDate = DateTime.Now.ToShortDateString(),
                        Cancel = false
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
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.ConnectionString);
                var bookingRecords = await _connection.QueryAsync<int>(verifyTimeSlot, new
                {
                    Date = bookingRecord.Date,
                    TimeSlotID = bookingRecord.TimeSlotID
                });

                return bookingRecords.Single() == 0;
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

        public async Task<bool> CancelAppointment(int bookingID)
        {
            const string cancelAppointment = @"
            UPDATE bookingrecord
                SET Cancel = true
            WHERE ID = @ID";

            try
            {
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.ConnectionString);
                var cancelled = await _connection.ExecuteAsync(cancelAppointment,
                    new
                    {
                        ID = bookingID
                    });

                return cancelled == 1;
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
        
        public async Task<List<BookingRecord>> GetAppointmentsByContactID(int contactID)
        {
            const string selectBookingRecords = @"
            SELECT
                ID,
                TimeSlotID,
                Date,
                Description
            FROM bookingrecord
            WHERE ContactID = @ContactID
                AND Date < NOW()"; // TODO: change this to > now();
            
            try
            {
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.ConnectionString);
                var bookingRecords = await _connection.QueryAsync<BookingRecord>(selectBookingRecords, new
                {
                    ContactID = contactID
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

        public async Task<List<BookingRecord>> GetAppointmentsByDate(DateTime startDate, DateTime endDate)
        {
            const string selectBookingRecords = @"
            SELECT 
	            b.ID,
                TimeSlotID,
	            Date,
	            Description,
	            Name,                
	            Phone,
	            Email	            
            FROM bookingrecord b
            JOIN contact c
            ON b.ContactID = c.ID
            WHERE Date >= @StartDate
                AND	Date <= @EndDate";
            
            try
            {
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.ConnectionString);
                var bookingRecords = await _connection.QueryAsync<BookingRecord, Contact, BookingRecord>(selectBookingRecords, 
                    map: (bookingRecord, contact) =>
                    {
                        bookingRecord.contact = contact;
                        return bookingRecord;
                    },
                    splitOn: "Name",
                    param: new
                    {
                        StartDate = startDate,
                        EndDate = endDate
                    }
                    );

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

        public async Task<BookingRecord> GetAppointmentByID(int bookingID)
        {
            const string selectBookingRecord = @"
            SELECT 
	            b.ID,	
	            TimeSlotID,
	            Date,
	            Description,
                Name,
	            Phone,
	            Email
            FROM bookingrecord b
            JOIN contact c
            ON b.ContactID = c.ID
            WHERE b.ID = @BookingID";
            
            try
            {
                await using var _connection = new MySqlConnection(connectionString: _mySqlConfig.ConnectionString);
                var bookingRecord = await _connection.QueryAsync<BookingRecord, Contact, BookingRecord>(selectBookingRecord, 
                    map: (record, contact) =>
                    {
                        record.contact = contact;
                        return record;
                    },
                    splitOn: "Name",
                    param: new
                    {
                        BookingID = bookingID
                    }
                );

                return bookingRecord.Any() ?  bookingRecord.Single() : new BookingRecord();
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