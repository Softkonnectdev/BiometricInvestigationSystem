namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Controller1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BVNBanks", "Gender", c => c.String(nullable: false));
            AddColumn("dbo.Deposits", "CashaierID", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Transfers", "SenderAccountNumber", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Transfers", "RecieverAccountNumber", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Transfers", "RecieverAccountName", c => c.String(nullable: false));
            AddColumn("dbo.Transfers", "CashaierID", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Employees", "Phone", c => c.String(nullable: false, maxLength: 11));
            AddColumn("dbo.Withdrawals", "CashaierID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Accounts", "Signature", c => c.Binary());
            AlterColumn("dbo.Accounts", "Passport", c => c.Binary());
            AlterColumn("dbo.Accounts", "IdentificationIssuedDate", c => c.Int(nullable: false));
            AlterColumn("dbo.Accounts", "IdentificationValidTill", c => c.Int(nullable: false));
            CreateIndex("dbo.Deposits", "CashaierID");
            CreateIndex("dbo.Transfers", "CashaierID");
            CreateIndex("dbo.Withdrawals", "CashaierID");
            AddForeignKey("dbo.Transfers", "CashaierID", "dbo.Employees", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Withdrawals", "CashaierID", "dbo.Employees", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Deposits", "CashaierID", "dbo.Employees", "UserId", cascadeDelete: true);
            DropColumn("dbo.Deposits", "Cashaier");
            DropColumn("dbo.Transfers", "AccountNumber");
            DropColumn("dbo.Transfers", "AccountName");
            DropColumn("dbo.Transfers", "SenderName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transfers", "SenderName", c => c.String(nullable: false));
            AddColumn("dbo.Transfers", "AccountName", c => c.String(nullable: false));
            AddColumn("dbo.Transfers", "AccountNumber", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Deposits", "Cashaier", c => c.String(nullable: false));
            DropForeignKey("dbo.Deposits", "CashaierID", "dbo.Employees");
            DropForeignKey("dbo.Withdrawals", "CashaierID", "dbo.Employees");
            DropForeignKey("dbo.Transfers", "CashaierID", "dbo.Employees");
            DropIndex("dbo.Withdrawals", new[] { "CashaierID" });
            DropIndex("dbo.Transfers", new[] { "CashaierID" });
            DropIndex("dbo.Deposits", new[] { "CashaierID" });
            AlterColumn("dbo.Accounts", "IdentificationValidTill", c => c.String(nullable: false));
            AlterColumn("dbo.Accounts", "IdentificationIssuedDate", c => c.String(nullable: false));
            AlterColumn("dbo.Accounts", "Passport", c => c.Binary(nullable: false));
            AlterColumn("dbo.Accounts", "Signature", c => c.Binary(nullable: false));
            DropColumn("dbo.Withdrawals", "CashaierID");
            DropColumn("dbo.Employees", "Phone");
            DropColumn("dbo.Transfers", "CashaierID");
            DropColumn("dbo.Transfers", "RecieverAccountName");
            DropColumn("dbo.Transfers", "RecieverAccountNumber");
            DropColumn("dbo.Transfers", "SenderAccountNumber");
            DropColumn("dbo.Deposits", "CashaierID");
            DropColumn("dbo.BVNBanks", "Gender");
        }
    }
}
