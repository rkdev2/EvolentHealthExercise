using Dapper;
using EvolentHealthExercise.Core.Interfaces;
using EvolentHealthExercise.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EvolentHealthExercise.Core.Repositories
{
    public class ContactRepository : ConnectionRepository, IContactRepository
    {
        public ContactRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Contact> GetContact(int id)
        {
            var sqlQuery = "select * from Contact (nolock) where Id= @id";

            using (var connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Contact>(sqlQuery, new
                {
                    id
                });

                return result.FirstOrDefault();
            }
        }

        public async Task<List<Contact>> GetContacts()
        {
            var sqlQuery = "select * from Contact (nolock) where Status = 1";

            using (var connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Contact>(sqlQuery);

                return result.ToList();
            }
        }

        public async Task InsertContact(Contact contact)
        {
            var sqlQuery = @"Insert into Contact (FirstName, LastName, Email, PhoneNumber, Status) 
                           Values (@firstName, @lastName, @email, @phoneNumber, @status)";

            using (var connection = await OpenConnectionAsync())
            {
                await connection.ExecuteAsync(sqlQuery, new
                {
                    firstName = contact.FirstName,
                    lastName = contact.LastName,
                    email = contact.Email,
                    phoneNumber = contact.PhoneNumber,
                    status = 1,//when insert set is as active
                });
            }
        }

        public async Task UpdateContact(Contact contact)
        {
            var sqlQuery = @"Update  Contact set FirstName =@firstName, LastName=@lastName, Email=@email, PhoneNumber=@phoneNumber, Status=@status
                           where Id = @Id";

            using (var connection = await OpenConnectionAsync())
            {
                await connection.ExecuteAsync(sqlQuery, new
                {
                    firstName = contact.FirstName,
                    lastName = contact.LastName,
                    email = contact.Email,
                    phoneNumber = contact.PhoneNumber,
                    status = contact.Status,
                    id = contact.Id
                });
            }
        }

        public async Task DeleteContact(int id)
        {
            //soft delete records
            var sqlQuery = @"Update Contact Set Status = 0 where Id = @Id";

            using (var connection = await OpenConnectionAsync())
            {
                await connection.ExecuteAsync(sqlQuery, new
                {
                    id
                });
            }
        }
    }
}