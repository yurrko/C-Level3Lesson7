namespace MHser.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class category : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "TypeOfObject", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "TypeOfObject");
        }
    }
}
