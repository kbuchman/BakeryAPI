using BakeryAPI.Models;
using BakeryAPI.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Validators
{
    public class RegisterUserVMValidator : AbstractValidator<RegisterUserVM>
    {
        public RegisterUserVMValidator(BakeryContext context)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Email)
                .Custom((value, _context) =>
                {
                    var emailExist = context.Users.Any(e => e.Email == value);
                    if (emailExist)
                    {
                        _context.AddFailure("Email", "This email is already in use.");
                    }

                });

            RuleFor(x => x.Password).MinimumLength(8);

            RuleFor(x => x.ConfirmPassword).Equal(y => y.Password);
        }
    }
}
