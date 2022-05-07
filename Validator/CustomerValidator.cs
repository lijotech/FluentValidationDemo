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

            
            When(e => (!string.IsNullOrEmpty(e.Username) && !string.IsNullOrEmpty(e.Email)), () =>
            {
                //Validation based on two fields in request model
                RuleFor(e => new {e.Username,e.Email })
                        .Must(x => !x.Email.Contains(x.Username))
                        .WithMessage(x=>$"Email should not contain the username: {x.Username}");
            });

            //Validation based on external service method
            RuleFor(e => e.IdentificationNumber)
                .NotEmpty()
                .WithMessage("Identification number required.")
                .MustAsync(async (f, _) => await _serviceMethods.ValidateIdentificationNumber(f))
                .WithMessage("Identification number not valid");

            //Validation for Child List items
            RuleForEach(x => x.AddressList).ChildRules(items =>
            {
                items.RuleFor(e => e.PostBox)
                 .NotEmpty()
                 .WithMessage("Postbox required.");
            });

            //Validation with use of SetValidator seperate class
            When(e => (e.DocumentList != null && e.DocumentList.Count > 0), () =>
            {
                RuleForEach(x => x.DocumentList)
                    .SetValidator(model => new DocumentValidator());
            });

            //Validation with conditional rule in a list of items
            RuleForEach(model => model.DocumentList.Where(g => g.IsActive.HasValue
            && g.IsActive.Value)).ChildRules(items =>
            {
                items.RuleFor(x => x.Id)
                    .NotEmpty()
                    .WithMessage("DocumentList Id required when document active.");
            }).OverridePropertyName("DocumentListValidatorWhenActive");
        }
    }


    public class DocumentValidator : AbstractValidator<Document>
    {
        public DocumentValidator()
        {

            RuleFor(x => x.DocumentName)
               .NotEmpty()
               .WithMessage("Document name required");

            RuleFor(x => x.CreatedDate)
                    .LessThan(x => System.DateTime.Today)
                    .WithMessage("Document created date cannot be greater than todays date.");

            RuleFor(x => x.ExpiryDate)
                  .GreaterThan(x => System.DateTime.Today)
                  .WithMessage("Document expiry date should be greater than todays date.");

            RuleFor(x => x.ExpiryDate)
                 .GreaterThan(x => x.CreatedDate)
                 .WithMessage("Document expiry date should be greater than created date.");
        }
    }
}
