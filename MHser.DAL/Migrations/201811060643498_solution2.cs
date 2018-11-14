namespace MHser.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class solution2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DisruptionLogs", "Solution", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DisruptionLogs", "Solution");
        }
    }
}
