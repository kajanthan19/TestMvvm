using AutoMapper;
using MediatR;
using TestMvvm.Domain.Dtos;
using TestMvvm.Domain.IUnitOfWork;

namespace TestMvvm.Core.Handlers.Queries
{
    public class GetAllAircraftsQuery : IRequest<IList<AircraftDto>>
    {
        public GetAllAircraftsQuery()
        {
        }
    }

    public class GetAllAircraftsHandler : IRequestHandler<GetAllAircraftsQuery, IList<AircraftDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAircraftsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<AircraftDto>> Handle(GetAllAircraftsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.AircraftRepository.GetAll(x => x.IsDeleted == false);
            var result = _mapper.Map<IList<AircraftDto>>(entities);

            return result;

        }
    }
}
