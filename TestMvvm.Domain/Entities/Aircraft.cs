using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMvvm.Domain.Entities
{
    public class Aircraft : BaseEntity<int>
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Registration { get; set; }
        public string Location { get; set; }
        public DateTime AircraftSeen { get; set; }
        public string ImageUrl { get; set; }

    }
}
