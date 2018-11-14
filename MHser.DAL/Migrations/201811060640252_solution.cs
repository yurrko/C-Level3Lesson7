namespace MHser.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class solution : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Disruptions", "Solution", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Disruptions", "Solution");
        }
    }
}
