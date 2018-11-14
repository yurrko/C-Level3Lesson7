using MHser.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MHser.Domain.Interfaces
{
    public interface IDisruptionData : IOrganizer, IReports
    {
        IEnumerable<Disruption> GetDisruptions();
        IEnumerable<Disruption> GetDisruptionsWithFilter( DisruptionFilter disruptionFilter );

        Disruption GetDisruptionById( int id );

        Disruption ModifyDisruption( Disruption disruption );

        Disruption AddDisruption( Disruption disruption );

        Disruption DeleteDisruption( int id );

        int TotalDisruptions();

        IEnumerable<Disruption> GetDisruptionDataForExcel( DateTime? from, DateTime? to, int? locationId, int? userId, int? categoryId, int? characterId, int? status );
        byte[] DisruptionsExcel( DateTime? from, DateTime? to, int? locationId, int? userId, int? categoryId, int? characterId, int? status );
        byte[] Excel();
    }
}