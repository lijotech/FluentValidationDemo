using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApiProject.DTO;
using TestWebApiProject.Extensions;
using TestWebApiProject.Services;
using static TestWebApiProject.Extensions.Enums;

namespace TestWebApiProject.Validator
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        readonly List<string> genderTypes = EnumExtension.GetEnumValuesAndDescriptions<GenderTypes>().Select(c => c.Value).ToList();
        public CustomerValidator(IServiceMethod _serviceMethods)
        {
            //Validation for empty and null check for string field
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name required");

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

            // Validation for Email id using Regular Expression
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email required")
                .Matches(@"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")
                .WithMessage("Email format is wrong.");

            //Validation using a static method call            
            RuleFor(e => e.Mobile)
                .NotEmpty()
                .WithMessage("Mobile number required.")
                .Must(x => Utility.ValidateMobileNumber(x))
                .WithMessage("Mobile number Not Valid");

            //Validation based on enum values with description, gender is optional value
            When(e => !string.IsNullOrEmpty(e.Gender), () =>
            {
                RuleFor(e => e.Gender)
                        .Must(x => genderTypes.Contains(x.ToUpper()))
                        .WithMessage($"Gender is invalid, allowed values are {String.Join(",", genderTypes)}");
            });

            RuleFor(e => e.IdentificationNumber)
                .NotEmpty()
                .WithMessage("Identification number required.")
                .MustAsync(async (f, _) => await _serviceMethods.ValidateIdentificationNumber(f))
                .WithMessage("Identification number not valid");


        }
    }
}
