namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateup : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Withdrawals", "AccountNumber");
            RenameColumn(table: "dbo.Withdrawals", name: "AccountID", newName: "AccountNumber");
            RenameIndex(table: "dbo.Withdrawals", name: "IX_AccountID", newName: "IX_AccountNumber");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Withdrawals", name: "IX_AccountNumber", newName: "IX_AccountID");
            RenameColumn(table: "dbo.Withdrawals", name: "AccountNumber", newName: "AccountID");
            AddColumn("dbo.Withdrawals", "AccountNumber", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
