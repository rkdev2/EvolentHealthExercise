
namespace EvolentHealthExercise.WebApi.Models
{
    public class ContactModel
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