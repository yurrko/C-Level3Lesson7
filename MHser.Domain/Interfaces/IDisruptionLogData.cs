using MHser.Domain.Entities;

namespace MHser.Domain.Interfaces
{
    public interface IDisruptionLogData
    {
        void WriteLog( Disruption disruption);
    }
}
