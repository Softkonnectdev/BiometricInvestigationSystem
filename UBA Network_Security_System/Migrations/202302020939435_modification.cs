namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modification : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Accounts", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Accounts", new[] { "UserId" });
            DropColumn("dbo.Accounts", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accounts", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Accounts", "UserId");
            AddForeignKey("dbo.Accounts", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
