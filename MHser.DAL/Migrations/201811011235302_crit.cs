namespace MHser.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DisruptionLogs", "isCritical", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DisruptionLogs", "isCritical");
        }
    }
}
