namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatexyru : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Deposits", "AccountName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Deposits", "AccountName", c => c.String(nullable: false));
        }
    }
}
