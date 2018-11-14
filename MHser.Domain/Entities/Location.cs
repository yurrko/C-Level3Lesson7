using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MHser.Domain.Entities
{

    public class Location
    {
        public int LocationId { get;set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string TypeOfObject { get; set; }
        public ICollection<Responsable> Responsables { get; set; }
    }
}