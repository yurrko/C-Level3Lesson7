using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHser.Domain.Entities
{
    public class DisruptionFilter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int[] CategoryIds { get; set; }
        public int[] CharacterIds { get; set; }
    }
}
