namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xyz : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransactionLedgers", "TransactionType", c => c.String(nullable: false));
            DropColumn("dbo.TransactionLedgers", "AccountType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransactionLedgers", "AccountType", c => c.String(nullable: false));
            DropColumn("dbo.TransactionLedgers", "TransactionType");
        }
    }
}
