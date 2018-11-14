using MHser.DAL.Contexts;
using MHser.Domain.Entities;
using MHser.Domain.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MHser.Services
{
    public class LocationData : ILocationData
    {
        public IEnumerable<Location> GetLocations()
        {
            using ( var context = new MessoyahaContext() )
            {
                return context.Locations.Include( "Responsables" ).ToList();
            }
        }

        public Location GetLocationById( int id )
        {
            using ( var context = new MessoyahaContext() )
            {
                return context.Locations.Include( "Responsables" ).FirstOrDefault( l => l.LocationId == id );
            }
        }

        public Location AddLocation( Location location )
        {
            using ( var context = new MessoyahaContext() )
            {
                context.Locations.Add( location );
                context.SaveChanges();
                return location;
            }
        }

        public void ModifyLocation( Location location )
        {
            using ( var context = new MessoyahaContext() )
            {
                context.Locations.Attach( location );
                context.Entry( location ).State = EntityState.Modified;
            }
        }
        public void DeleteLocation( int id )
        {
            var locationToDelete = GetLocationById( id );

            if (locationToDelete is null) return;

            using ( var context = new MessoyahaContext() )
            {
                context.Locations.Attach( locationToDelete );
                context.Entry( locationToDelete ).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}