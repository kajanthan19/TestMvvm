using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Core.Exceptions;
using TestMvvm.Domain.Dtos;
using TestMvvm.Domain.IUnitOfWork;

namespace TestMvvm.Core.Handlers.Commands
{
    public class UpdateAircraftCommand : IRequest<AircraftDto>
    {
        public CreateAircraftDto Model { get; }

        public int Id { get; }
        public UpdateAircraftCommand(int id, CreateAircraftDto model)
        {
            this.Model = model;
            this.Id = id;
        }
    }

    public class UpdateAircraftCommandHandler : IRequestHandler<UpdateAircraftCommand, AircraftDto>
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IValidator<CreateAircraftDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateAircraftCommandHandler> _logger;

        public UpdateAircraftCommandHandler(ILogger<UpdateAircraftCommandHandler> logger, IUnitOfWork unitofwork,
            IValidator<CreateAircraftDto> validator, IMapper mapper)
        {
            _unitofwork = unitofwork;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<AircraftDto> Handle(UpdateAircraftCommand request, CancellationToken cancellationToken)
        {
            CreateAircraftDto model = request.Model;
            var id = request.Id;

            var result = _validator.Validate(model);

            if (!result.IsValid)
            {
                _logger.LogError($"Update Aircraft Validation result: {result}");
                var errors = result.Errors.Select(x => x.ErrorMessage).ToArray();
                throw new InvalidRequestBodyException
                {
                    Errors = errors
                };
            }

            var entity = await _unitofwork.AircraftRepository.Get(id);
            if (entity == null)
            {
                _logger.LogError($"Aircraft doesnot found: {id}");
                throw new Exception("AircraftId not found");
            }
            entity.Registration = model.Registration;
            entity.Location = model.Location;
            entity.Make = model.Make;
            entity.Model = model.Model;
            entity.AircraftSeen = model.AircraftSeen;
            if (model.ImageUrl != null)
            {
                entity.ImageUrl = model.ImageUrl;
            }
            _unitofwork.AircraftRepository.Update(entity);
            await _unitofwork.Save();

            return _mapper.Map<AircraftDto>(entity);
        }
    }
}
