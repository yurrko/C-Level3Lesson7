namespace MHser.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class too : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "TypeOfObject", c => c.String());
            DropColumn("dbo.Locations", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "Category", c => c.String());
            DropColumn("dbo.Locations", "TypeOfObject");
        }
    }
}
