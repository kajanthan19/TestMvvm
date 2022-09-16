using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Domain.IRepository;

namespace TestMvvm.Domain.IUnitOfWork
{
    public interface IUnitOfWork
    {
        IAircraftRepository AircraftRepository { get; }
        Task<int> Save();
    }
}
