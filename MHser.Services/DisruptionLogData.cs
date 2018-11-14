using MHser.DAL.Contexts;
using MHser.Domain.Entities;
using MHser.Domain.Interfaces;

namespace MHser.Services
{
    public class DisruptionLogData : IDisruptionLogData
    {
        public void WriteLog( Disruption disruption )
        {
            using ( var context = new MessoyahaContext() )
            {
                var disruptionLog = new DisruptionLog
                {
                    DisruptionId = disruption.DisruptionId,
                    DetectionTime = disruption.DetectionTime,
                    ExecuteUntil = disruption.ExecuteUntil,
                    Description = disruption.Description,
                    Documentation = disruption.Documentation,
                    UserId = disruption.UserId,
                    LocationId = disruption.LocationId,
                    Place = disruption.Place,
                    CategoryId = disruption.CategoryId,
                    CharacterId = disruption.CharacterId,
                    ResponsableId = disruption.ResponsableId,
                    IsCritical = disruption.IsCritical,
                    IsDone = disruption.IsDone,
                    Events = disruption.Events,
                    ReasonNotCompleted = disruption.ReasonNotCompleted,
                    CreationDate = disruption.CreationDate,
                    UpdateDate = disruption.UpdateDate,
                    UpdateUserNameId = disruption.UpdateUserNameId,
                    Solution = disruption.Solution,
                };
                context.DisruptionsLog.Add( disruptionLog );
                context.SaveChanges();
            }
        }
    }
}