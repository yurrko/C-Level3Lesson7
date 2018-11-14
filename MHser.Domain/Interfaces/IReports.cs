using MHser.Domain.Entities;
using System.Collections.Generic;

namespace MHser.Domain.Interfaces
{
    public interface IReports
    {
        object GetOrgData();
        IEnumerable<ResultReport> GetMainReport();
    }
}
