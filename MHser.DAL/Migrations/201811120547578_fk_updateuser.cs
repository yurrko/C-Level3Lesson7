namespace MHser.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fk_updateuser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Disruptions", "UserId", "dbo.Users");
            DropIndex("dbo.Disruptions", new[] { "UserId" });
            AddColumn("dbo.Disruptions", "User_UserId", c => c.Int());
            CreateIndex("dbo.Disruptions", "UpdateUserNameId");
            CreateIndex("dbo.Disruptions", "User_UserId");
            AddForeignKey("dbo.Disruptions", "UpdateUserNameId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Disruptions", "User_UserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Disruptions", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Disruptions", "UpdateUserNameId", "dbo.Users");
            DropIndex("dbo.Disruptions", new[] { "User_UserId" });
            DropIndex("dbo.Disruptions", new[] { "UpdateUserNameId" });
            DropColumn("dbo.Disruptions", "User_UserId");
            CreateIndex("dbo.Disruptions", "UserId");
            AddForeignKey("dbo.Disruptions", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
