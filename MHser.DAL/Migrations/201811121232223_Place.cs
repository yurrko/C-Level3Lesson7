namespace MHser.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Place : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Disruptions", "Place", c => c.String());
            AddColumn("dbo.DisruptionLogs", "Place", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DisruptionLogs", "Place");
            DropColumn("dbo.Disruptions", "Place");
        }
    }
}
