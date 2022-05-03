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
            RuleFor(x => x.FirstName)                
                .NotEmpty()
                .WithMessage("First Name required");
        }
    }
}
