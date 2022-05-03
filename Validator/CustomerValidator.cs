using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApiProject.DTO;

namespace TestWebApiProject.Validator
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            //Validation for empty and null check for string field
            RuleFor(x => x.FirstName)                
                .NotEmpty()
                .WithMessage("First Name required");

            // Validation for nullable type
            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth required");

            //Validation based on When condition in a field
            When(e => e.DateOfBirth.HasValue, () =>
            {
                RuleFor(model => model.DateOfBirth.Value.Date)             
                .LessThanOrEqualTo(x => System.DateTime.Today)
                .WithMessage("Date of birth cannot be greater than todays date.");

                //Validation using custom rule and Validation context
                RuleFor(model => model.DateOfBirth.Value.Date)
                 .Custom((x, context) =>
                 {
                     if (x.AddYears(18) > DateTime.Now)
                     {
                         context.AddFailure($"Users with age less than 18 years not allowed to register, please check DateofBirth {x:dd-MMM-yyyy}");
                     }
                 });

            });
        }
    }
}
