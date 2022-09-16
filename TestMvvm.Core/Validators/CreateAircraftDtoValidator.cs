using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Domain.Dtos;

namespace TestMvvm.Core.Validators
{
    public class CreateAircraftDtoValidator : AbstractValidator<CreateAircraftDto>
    {
        public CreateAircraftDtoValidator()
        {
            RuleFor(x => x.Make).NotEmpty().MaximumLength(128).WithMessage("Make is required");
            RuleFor(x => x.Model).NotEmpty().WithMessage("Model is required");
            RuleFor(x => x.Registration).NotEmpty().WithMessage("Registration is required");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Location is required");
            RuleFor(x => x.AircraftSeen).NotEmpty().WithMessage("Aircraft Date time is required");

        }
    }
}
