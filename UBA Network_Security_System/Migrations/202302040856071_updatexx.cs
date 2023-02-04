namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatexx : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Deposits", "AccountNumber");
            RenameColumn(table: "dbo.Deposits", name: "AccountID", newName: "AccountNumber");
            RenameIndex(table: "dbo.Deposits", name: "IX_AccountID", newName: "IX_AccountNumber");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Deposits", name: "IX_AccountNumber", newName: "IX_AccountID");
            RenameColumn(table: "dbo.Deposits", name: "AccountNumber", newName: "AccountID");
            AddColumn("dbo.Deposits", "AccountNumber", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
