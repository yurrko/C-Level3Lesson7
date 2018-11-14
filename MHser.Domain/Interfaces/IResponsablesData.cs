using MHser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHser.Domain.Interfaces
{
    public interface IResponsablesData
    {
        IEnumerable<Responsable> GetResponsables();
        Responsable GetResponsableById( int id );
        void ModifyResponsable( Responsable responsable );
        Responsable AddResponsable( Responsable responsable );
        void DeleteResponsable( int id );
    }
}
