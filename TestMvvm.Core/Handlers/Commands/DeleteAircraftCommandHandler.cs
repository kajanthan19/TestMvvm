using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Domain.IUnitOfWork;

namespace TestMvvm.Core.Handlers.Commands
{
    public class DeleteAircraftCommand : IRequest<bool>
    {

        public int Id { get; }
        public DeleteAircraftCommand(int id)
        {
            this.Id = id;
        }
    }

    public class DeleteAircraftCommandHandler : IRequestHandler<DeleteAircraftCommand, bool>
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteAircraftCommandHandler> _logger;

        public DeleteAircraftCommandHandler(ILogger<DeleteAircraftCommandHandler> logger, IUnitOfWork unitofwork,
         IMapper mapper)
        {
            _unitofwork = unitofwork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> Handle(DeleteAircraftCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var entity = await _unitofwork.AircraftRepository.Get(id);

            if (entity == null)
            {
                _logger.LogError($"Aircraft doesnot found  : {id}");
                throw new Exception("Aircraft not found");
            }

            entity.IsDeleted = true;
            await _unitofwork.AircraftRepository.SetIsDeleteTrue(entity);
            await _unitofwork.Save();

            return true;
        }
    }


}
