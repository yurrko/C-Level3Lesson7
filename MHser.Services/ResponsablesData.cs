using MHser.DAL.Contexts;
using MHser.Domain.Entities;
using MHser.Domain.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MHser.Services
{
    public class ResponsablesData : IResponsablesData
    {
        public IEnumerable<Responsable> GetResponsables()
        {
            using ( var context = new MessoyahaContext() )
            {
                return context.Responsables.ToList();
            }
        }

        public Responsable GetResponsableById( int id )
        {
            using ( var context = new MessoyahaContext() )
            {
                return context.Responsables.FirstOrDefault( r => r.ResponsableId == id );
            }
        }

        public Responsable AddResponsable( Responsable responsable )
        {
            using ( var context = new MessoyahaContext() )
            {
                context.Responsables.Add( responsable );
                context.SaveChanges();
                return responsable;
            }
        }

        public void ModifyResponsable( Responsable responsable )
        {
            using ( var context = new MessoyahaContext() )
            {
                context.Responsables.Attach( responsable );
                context.Entry( responsable ).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteResponsable( int id )
        {
            var responsableToDelete = GetResponsableById( id );

            if (responsableToDelete is null) return;

            using ( var context = new MessoyahaContext() )
            {
                context.Responsables.Attach( responsableToDelete );
                context.Entry( responsableToDelete ).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}