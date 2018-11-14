using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHser.Domain.Entities
{
    public class DisruptionLog
    {
        [Key, DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public Guid dbid { get; set; }
        public int DisruptionId { get; set; }
        public DateTime DetectionTime { get; set; }
        public DateTime ExecuteUntil { get; set; }
        public string Description { get; set; }
        public string Documentation { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public string Place { get; set; }
        public int CategoryId { get; set; }
        public int CharacterId { get; set; }
        public int? ResponsableId { get; set; }
        public bool IsCritical { get; set; }
        public bool IsDone { get; set; }
        public string Events { get; set; }
        public string ReasonNotCompleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateUserNameId { get; set; }
        public string Solution { get; set; }
    }
}
