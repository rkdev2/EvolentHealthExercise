using System;
using System.Collections.Generic;
using System.Text;

namespace EvolentHealthExercise.Core.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ContactStatus Status { get; set; }
    }

    public enum ContactStatus
    {
        Inactive = 0,
        Active = 1
    }
}
