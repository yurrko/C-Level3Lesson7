using MHser.Domain.Entities;
using System.Collections.Generic;

namespace MHser.Domain.Interfaces
{
    public interface IOrganizer
    {
        IEnumerable<Disruption> GetExpired();
        IEnumerable<Disruption> GetDeadlineToday();
        IEnumerable<Disruption> GetAddedToday();
        IEnumerable<Disruption> GetImportant();
        IEnumerable<Disruption> GetSoon();
        IEnumerable<Disruption> GetThisWeek();
        IEnumerable<Disruption> GetCompleted();
        IEnumerable<Disruption> GetAll();
    }
}
