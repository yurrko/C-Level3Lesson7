namespace MHser.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Responsable_LocationId_AllowNull : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Responsables", "LocationId", "dbo.Locations");
            DropIndex("dbo.Responsables", new[] { "LocationId" });
            AlterColumn("dbo.Responsables", "LocationId", c => c.Int());
            CreateIndex("dbo.Responsables", "LocationId");
            AddForeignKey("dbo.Responsables", "LocationId", "dbo.Locations", "LocationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Responsables", "LocationId", "dbo.Locations");
            DropIndex("dbo.Responsables", new[] { "LocationId" });
            AlterColumn("dbo.Responsables", "LocationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Responsables", "LocationId");
            AddForeignKey("dbo.Responsables", "LocationId", "dbo.Locations", "LocationId", cascadeDelete: true);
        }
    }
}
