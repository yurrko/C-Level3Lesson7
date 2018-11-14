using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHser.Domain.Interfaces
{
    public interface ILogsData
    {
        byte[] GetLogFile( string path );
    }
}
