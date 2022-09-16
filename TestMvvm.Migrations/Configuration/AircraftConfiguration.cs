using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Domain.Entities;

namespace TestMvvm.Migrations.Configuration
{
    public class AircraftConfiguration : BaseEntityConfiguration<Aircraft>
    {
        public override void Configure(EntityTypeBuilder<Aircraft> builder)
        {
            builder.ToTable("Aircrafts", "Aircraft");
            builder.Property(x => x.Make).IsRequired(true).HasMaxLength(128);
            builder.Property(x => x.Model).IsRequired(true).HasMaxLength(128);
            builder.Property(x => x.Registration).IsRequired(true).HasMaxLength(10);
            builder.Property(x => x.Location).IsRequired(true).HasMaxLength(255);
            builder.Property(x => x.ImageUrl).IsRequired(false).HasMaxLength(500);
            builder.Property(x => x.AircraftSeen).IsRequired(true);

            base.Configure(builder);
        }
    }
}
