namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateup001 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Transfers", "SenderAccountNumber");
            RenameColumn(table: "dbo.Transfers", name: "AccountID", newName: "SenderAccountNumber");
            RenameIndex(table: "dbo.Transfers", name: "IX_AccountID", newName: "IX_SenderAccountNumber");
            AlterColumn("dbo.Transfers", "Remark", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transfers", "Remark", c => c.String(nullable: false));
            RenameIndex(table: "dbo.Transfers", name: "IX_SenderAccountNumber", newName: "IX_AccountID");
            RenameColumn(table: "dbo.Transfers", name: "SenderAccountNumber", newName: "AccountID");
            AddColumn("dbo.Transfers", "SenderAccountNumber", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
