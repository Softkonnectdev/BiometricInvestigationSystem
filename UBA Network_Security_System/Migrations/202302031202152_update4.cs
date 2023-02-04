namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Accounts", "CreatedOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accounts", "CreatedOn", c => c.DateTime(nullable: false));
        }
    }
}
