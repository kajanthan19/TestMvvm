using AutoMapper;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Core.Handlers.Queries;
using TestMvvm.Core.Mapper;
using TestMvvm.Domain.Dtos;
using TestMvvm.Domain.IUnitOfWork;
using TestMvvm.Web.UnitTests.Mocks;
using Xunit;

namespace TestMvvm.Web.UnitTests.Handlers.Queries
{
    public class GetAllAircraftQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUow;
        public GetAllAircraftQueryHandlerTests()
        {
            _mockUow = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<AutoMapperProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetAllAircraftsTest()
        {
            var handler = new GetAllAircraftsHandler(_mockUow.Object, _mapper);

            var result = await handler.Handle(new GetAllAircraftsQuery(), CancellationToken.None);

            result.ShouldBeOfType<List<AircraftDto>>();

            result.Count.ShouldBe(4);
        }
    }
}
