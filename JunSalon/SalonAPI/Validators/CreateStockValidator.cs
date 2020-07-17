using Contracts.V1.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalonAPI.Validators
{
    public class CreateStockValidator : AbstractValidator<CreateStockRequest>
    {
        public CreateStockValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotEqual("string");
        }
    }
}
