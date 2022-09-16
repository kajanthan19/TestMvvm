using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Domain.Entities;
using TestMvvm.Domain.IRepository;
using TestMvvm.Domain.IUnitOfWork;
using TestMvvm.Migrations;

namespace TestMvvm.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TestMvvmContext _context;
        public UnitOfWork(TestMvvmContext context,
            IAircraftRepository aircraftRepository)
        {
            _context = context;
            this.AircraftRepository = aircraftRepository;
        }

        public IAircraftRepository AircraftRepository { get; set; }

        public async Task<int> Save()
        {
            try
            {

                var modifiedEntries = _context.ChangeTracker.Entries();

                foreach (var entry in modifiedEntries)
                {
                    var entity = entry.Entity as ITrackableEntity;
                    if (entity != null)
                    {

                        if (entry.State == EntityState.Added)
                        {
                            entity.CreatedOn = DateTime.UtcNow;

                        }
                        entity.LastModifiedOn = DateTime.UtcNow;
                    }

                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                //We dont do anything about this exception
            }

            int affectedRows = await this._context.SaveChangesAsync();
            return affectedRows;
        }
    }
}
