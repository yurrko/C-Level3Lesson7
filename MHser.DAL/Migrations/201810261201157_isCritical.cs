namespace MHser.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isCritical : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Disruptions", "isCritical", c => c.Boolean(nullable: false));
            AddColumn("dbo.Disruptions", "Events", c => c.String());
            AddColumn("dbo.Disruptions", "ReasonNotCompleted", c => c.String());
            AddColumn("dbo.Users", "LastAccess", c => c.DateTime());
            AddColumn("dbo.DisruptionLogs", "Events", c => c.String());
            AddColumn("dbo.DisruptionLogs", "ReasonNotCompleted", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DisruptionLogs", "ReasonNotCompleted");
            DropColumn("dbo.DisruptionLogs", "Events");
            DropColumn("dbo.Users", "LastAccess");
            DropColumn("dbo.Disruptions", "ReasonNotCompleted");
            DropColumn("dbo.Disruptions", "Events");
            DropColumn("dbo.Disruptions", "isCritical");
        }
    }
}
