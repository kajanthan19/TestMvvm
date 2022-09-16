using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TestMvvm.Core.Exceptions;
using TestMvvm.Domain.Dtos;
using TestMvvm.Domain.Entities;
using TestMvvm.Domain.IUnitOfWork;

namespace TestMvvm.Core.Handlers.Commands
{
    public class CreateAircraftCommand : IRequest<AircraftDto>
    {
        public CreateAircraftDto Model { get; }
        public CreateAircraftCommand(CreateAircraftDto model)
        {
            this.Model = model;
        }

    }
    public class CreateAircraftCommandHandler : IRequestHandler<CreateAircraftCommand, AircraftDto>
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IValidator<CreateAircraftDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAircraftCommandHandler> _logger;

        public CreateAircraftCommandHandler(ILogger<CreateAircraftCommandHandler> logger, IUnitOfWork unitofwork,
            IValidator<CreateAircraftDto> validator, IMapper mapper)
        {
            _unitofwork = unitofwork;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<AircraftDto> Handle(CreateAircraftCommand request, CancellationToken cancellationToken)
        {
            CreateAircraftDto model = request.Model;

            var result = _validator.Validate(model);

            if (!result.IsValid)
            {
                _logger.LogError($"Create Aircraft Validation result: {result}");
                var errors = result.Errors.Select(x => x.ErrorMessage).ToArray();
                throw new InvalidRequestBodyException
                {
                    Errors = errors
                };
            }

            var entity = this._mapper.Map<Aircraft>(model);

            await _unitofwork.AircraftRepository.Add(entity);
            await _unitofwork.Save();

            return _mapper.Map<AircraftDto>(entity);
        }
    }
}
