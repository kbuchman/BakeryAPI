using BakeryAPI.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator(BakeryContext context)
        {
            RuleFor(x => x.Name)
                .Custom((value, _context) =>
                {
                    var productExist = context.Products.Any(p => p.Name.Equals(value));
                    if (productExist)
                    {
                        _context.AddFailure("Name", "This name is already in use.");
                    }

                });
        }
    }
}
