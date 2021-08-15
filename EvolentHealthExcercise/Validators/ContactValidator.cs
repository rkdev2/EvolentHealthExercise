using EvolentHealthExercise.WebApi.Models;
using FluentValidation;

namespace EvolentHealthExercise.WebApi.Validators
{
    public class ContactValidator : AbstractValidator<ContactModel>
    {
        public ContactValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty();
            RuleFor(m => m.LastName).NotEmpty();
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
            RuleFor(m => m.PhoneNumber).NotEmpty();
        }
    }
}