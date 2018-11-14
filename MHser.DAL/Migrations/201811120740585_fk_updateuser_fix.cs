namespace MHser.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fk_updateuser_fix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Disruptions", "User_UserId", "dbo.Users");
            DropIndex("dbo.Disruptions", new[] { "User_UserId" });
            DropColumn("dbo.Disruptions", "UserId");
            RenameColumn(table: "dbo.Disruptions", name: "User_UserId", newName: "UserId");
            AlterColumn("dbo.Disruptions", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Disruptions", "UserId");
            AddForeignKey("dbo.Disruptions", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Disruptions", "UserId", "dbo.Users");
            DropIndex("dbo.Disruptions", new[] { "UserId" });
            AlterColumn("dbo.Disruptions", "UserId", c => c.Int());
            RenameColumn(table: "dbo.Disruptions", name: "UserId", newName: "User_UserId");
            AddColumn("dbo.Disruptions", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Disruptions", "User_UserId");
            AddForeignKey("dbo.Disruptions", "User_UserId", "dbo.Users", "UserId");
        }
    }
}
