using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHser.Domain.Entities
{
    public class ResultReport
    {
        public int MonthId { get; set; }
        public int IndustrialSafety { get; set; }
        public int FireSafety { get; set; }
        public int ElectroSafety { get; set; }
        public int WorkWithPS { get; set; }
        public int TransportSafety { get; set; }
        public int EcoSafety { get; set; }
        public int WorkOnHeight { get; set; }
        public int FireWork { get; set; }
        public int Other { get; set; }

    }
}
