using MHser.Domain.Interfaces;
using Newtonsoft.Json;

namespace MHser.Domain.Entities
{
    public class Responsable : IPerson
    {
        public int ResponsableId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int? LocationId { get; set; }
        [JsonIgnore]
        public Location Location { get; set; }
    }
}