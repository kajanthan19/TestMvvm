using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Core.Handlers.Commands;
using TestMvvm.Core.Mapper;
using TestMvvm.Core.Validators;
using TestMvvm.Domain.Dtos;
using TestMvvm.Domain.IUnitOfWork;
using TestMvvm.Web.UnitTests.Mocks;
using Xunit;

namespace TestMvvm.Web.UnitTests.Handlers.Commands
{
    public class CreateAircraftCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUow;
        private CreateAircraftDto _aircraftDto;
        private readonly CreateAircraftCommandHandler _handler;
        private readonly IValidator<CreateAircraftDto> _validator;
        private readonly ILogger<CreateAircraftCommandHandler> _logger;

        public CreateAircraftCommandHandlerTests()
        {
            _mockUow = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<AutoMapperProfile>();
            });

            _mapper = mapperConfig.CreateMapper();

            var mock = new Mock<ILogger<CreateAircraftCommandHandler>>();
            _logger = mock.Object;

            _validator = new CreateAircraftDtoValidator();

            _handler = new CreateAircraftCommandHandler(_logger, _mockUow.Object, _validator, _mapper);

            _aircraftDto = new CreateAircraftDto
            {
                Make = "Boeing222",
                Model = "700-1S00DD",
                Registration = "G-GATS",
                Location = "Airport Gatwick",
                AircraftSeen = DateTime.Parse("2022-09-14T19:54:00.000Z"),
            };
        }

        [Fact]
        public async Task Valid_CreateAircraft_Added()
        {
            var result = await _handler.Handle(new CreateAircraftCommand(_aircraftDto), CancellationToken.None);

            var aircrafts = await _mockUow.Object.AircraftRepository.GetAll();

            result.ShouldBeOfType<AircraftDto>();

            aircrafts.Count.ShouldBe(5);
        }


        [Fact]
        public async Task InValid_CreateAircraft_Added()
        {

            try
            {
                _aircraftDto.Registration = null;

                var result = await _handler.Handle(new CreateAircraftCommand(_aircraftDto), CancellationToken.None);

                var aircrafts = await _mockUow.Object.AircraftRepository.GetAll();

                aircrafts.Count.ShouldBe(4);

                result.ShouldBeOfType<AircraftDto>();
            }
            catch (Exception)
            {
                var aircrafts = await _mockUow.Object.AircraftRepository.GetAll();
                aircrafts.Count.ShouldBe(4);
            }

        }
    }
}
