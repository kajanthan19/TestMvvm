using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Domain.Entities;
using TestMvvm.Domain.IRepository;

namespace TestMvvm.Web.UnitTests.Mocks
{
    public static class MockAircraftRepository
    {
        public static Mock<IAircraftRepository> GetAircraftRepository()
        {
            var aircrafts = new List<Aircraft>
            {
                new Aircraft
                {
                    Id = 1,
                    Make = "Boeing",
                    Model = "700-100DD",
                    Registration = "G-GHTS",
                    Location = "London Gatwick",
                    AircraftSeen = DateTime.Parse("2022-09-14T19:54:00.000Z"),
                    IsDeleted= false,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now
                },
                new Aircraft
                {
                    Id = 2,
                    Make = "Boeing2",
                    Model = "700-100FR",
                    Registration = "G-IOPD",
                    Location = "Amsterdam",
                    AircraftSeen = DateTime.Parse("2022-09-13T19:54:00.000Z"),
                    IsDeleted= false,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now
                },
                new Aircraft
                {
                    Id = 3,
                    Make = "Boeing3",
                    Model = "722-200ER",
                    Registration = "G-YYY",
                    Location = "London Heathrow",
                    AircraftSeen = DateTime.Parse("2022-09-12T19:54:00.000Z"),
                    IsDeleted= false,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now
                },
                new Aircraft
                {
                    Id = 4,
                    Make = "Boeing4",
                    Model = "776-400ER",
                    Registration = "GD-AAA",
                    Location = "Frankfurt Airport",
                    AircraftSeen = DateTime.Parse("2022-09-11T19:54:00.000Z"),
                    IsDeleted= false,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now
                },
            };

            var mockRepo = new Mock<IAircraftRepository>();

            mockRepo.Setup(r => r.GetAll()).ReturnsAsync(aircrafts);


            mockRepo.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync((int id) => {
                var response = aircrafts.Where(x => x.Id == id).Single();
                return response;
            });

            mockRepo.Setup(r => r.GetAll(It.IsAny<Expression<Func<Aircraft, bool>>>())).ReturnsAsync((Expression<Func<Aircraft, bool>> expressions) => {
                var response = aircrafts.AsQueryable().Where(expressions).ToList();
                return response;
            });


            mockRepo.Setup(r => r.Add(It.IsAny<Aircraft>())).ReturnsAsync((Aircraft aircraft) =>
            {
                aircrafts.Add(aircraft);
                return aircraft;
            });

            return mockRepo;

        }
    }
}
