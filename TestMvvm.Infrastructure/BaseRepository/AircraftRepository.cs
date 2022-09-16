using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Domain.Entities;
using TestMvvm.Domain.IRepository;
using TestMvvm.Migrations;

namespace TestMvvm.Infrastructure.BaseRepository
{
    public class AircraftRepository: BaseRepository<Aircraft>, IAircraftRepository
    {
        public AircraftRepository(TestMvvmContext context) : base(context)
        {
        }
    }
}
