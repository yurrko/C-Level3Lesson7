namespace MHser.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShortName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        CharacterId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShortName = c.String(),
                    })
                .PrimaryKey(t => t.CharacterId);
            
            CreateTable(
                "dbo.Disruptions",
                c => new
                    {
                        DisruptionId = c.Int(nullable: false, identity: true),
                        DetectionTime = c.DateTime(nullable: false),
                        ExecuteUntil = c.DateTime(nullable: false),
                        Description = c.String(),
                        Documentation = c.String(),
                        UserId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        CharacterId = c.Int(nullable: false),
                        ResponsableId = c.Int(),
                        IsDone = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        UpdateUserNameId = c.Int(),
                    })
                .PrimaryKey(t => t.DisruptionId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Characters", t => t.CharacterId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Responsables", t => t.ResponsableId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.LocationId)
                .Index(t => t.CategoryId)
                .Index(t => t.CharacterId)
                .Index(t => t.ResponsableId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.Responsables",
                c => new
                    {
                        ResponsableId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Position = c.String(),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResponsableId)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AdName = c.String(),
                        Position = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.DisruptionLogs",
                c => new
                    {
                        dbid = c.Guid(nullable: false, identity: true),
                        DisruptionId = c.Int(nullable: false),
                        DetectionTime = c.DateTime(nullable: false),
                        ExecuteUntil = c.DateTime(nullable: false),
                        Description = c.String(),
                        Documentation = c.String(),
                        UserId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        CharacterId = c.Int(nullable: false),
                        ResponsableId = c.Int(),
                        IsDone = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        UpdateUserNameId = c.Int(),
                    })
                .PrimaryKey(t => t.dbid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Disruptions", "UserId", "dbo.Users");
            DropForeignKey("dbo.Disruptions", "ResponsableId", "dbo.Responsables");
            DropForeignKey("dbo.Disruptions", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Responsables", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Disruptions", "CharacterId", "dbo.Characters");
            DropForeignKey("dbo.Disruptions", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Responsables", new[] { "LocationId" });
            DropIndex("dbo.Disruptions", new[] { "ResponsableId" });
            DropIndex("dbo.Disruptions", new[] { "CharacterId" });
            DropIndex("dbo.Disruptions", new[] { "CategoryId" });
            DropIndex("dbo.Disruptions", new[] { "LocationId" });
            DropIndex("dbo.Disruptions", new[] { "UserId" });
            DropTable("dbo.DisruptionLogs");
            DropTable("dbo.Users");
            DropTable("dbo.Responsables");
            DropTable("dbo.Locations");
            DropTable("dbo.Disruptions");
            DropTable("dbo.Characters");
            DropTable("dbo.Categories");
        }
    }
}
