using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EvolentHealthExercise.Core.Models;

namespace EvolentHealthExercise.Core.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> GetContact(int id);
        Task<List<Contact>> GetContacts();
        Task InsertContact(Contact contact);
        Task UpdateContact(Contact contact);
        Task DeleteContact(int id);
    }
}
