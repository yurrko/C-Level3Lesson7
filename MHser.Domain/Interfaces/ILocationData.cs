using MHser.Domain.Entities;
using System.Collections.Generic;

namespace MHser.Domain.Interfaces
{
    public interface ILocationData
    {
        IEnumerable<Location> GetLocations();
        Location GetLocationById( int id );
        Location AddLocation( Location location );
        void ModifyLocation( Location location );
        void DeleteLocation( int id );
    }
}
