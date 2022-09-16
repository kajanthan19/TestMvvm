using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Domain.IUnitOfWork;

namespace TestMvvm.Web.UnitTests.Mocks
{
    public static class MockUnitOfWork
    {
        public static Mock<IUnitOfWork> GetUnitOfWork()
        {
            var mockUow = new Mock<IUnitOfWork>();
            var mockAircraftRepo = MockAircraftRepository.GetAircraftRepository();

            mockUow.Setup(r => r.AircraftRepository).Returns(mockAircraftRepo.Object);
            return mockUow;
        }
    }
}
