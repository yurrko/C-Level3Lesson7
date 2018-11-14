namespace MHser.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdNameMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "AdName", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "AdName", c => c.String());
        }
    }
}
