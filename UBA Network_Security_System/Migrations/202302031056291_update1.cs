namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Deposits", "AccountID", "dbo.Accounts");
            DropForeignKey("dbo.Transfers", "AccountID", "dbo.Accounts");
            DropForeignKey("dbo.Withdrawals", "AccountID", "dbo.Accounts");
            DropIndex("dbo.Deposits", new[] { "AccountID" });
            DropIndex("dbo.Transfers", new[] { "AccountID" });
            DropIndex("dbo.Withdrawals", new[] { "AccountID" });
            DropPrimaryKey("dbo.Accounts");
            AddColumn("dbo.Accounts", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Deposits", "AccountID", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Transfers", "AccountID", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Withdrawals", "AccountID", c => c.String(nullable: false, maxLength: 10));
            AddPrimaryKey("dbo.Accounts", "AccountNumber");
            CreateIndex("dbo.Deposits", "AccountID");
            CreateIndex("dbo.Transfers", "AccountID");
            CreateIndex("dbo.Withdrawals", "AccountID");
            AddForeignKey("dbo.Deposits", "AccountID", "dbo.Accounts", "AccountNumber", cascadeDelete: true);
            AddForeignKey("dbo.Transfers", "AccountID", "dbo.Accounts", "AccountNumber", cascadeDelete: true);
            AddForeignKey("dbo.Withdrawals", "AccountID", "dbo.Accounts", "AccountNumber", cascadeDelete: true);
            DropColumn("dbo.Accounts", "Id");
            DropColumn("dbo.Accounts", "CreatedAt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accounts", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Accounts", "Id", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.Withdrawals", "AccountID", "dbo.Accounts");
            DropForeignKey("dbo.Transfers", "AccountID", "dbo.Accounts");
            DropForeignKey("dbo.Deposits", "AccountID", "dbo.Accounts");
            DropIndex("dbo.Withdrawals", new[] { "AccountID" });
            DropIndex("dbo.Transfers", new[] { "AccountID" });
            DropIndex("dbo.Deposits", new[] { "AccountID" });
            DropPrimaryKey("dbo.Accounts");
            AlterColumn("dbo.Withdrawals", "AccountID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Transfers", "AccountID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Deposits", "AccountID", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Accounts", "CreatedOn");
            AddPrimaryKey("dbo.Accounts", "Id");
            CreateIndex("dbo.Withdrawals", "AccountID");
            CreateIndex("dbo.Transfers", "AccountID");
            CreateIndex("dbo.Deposits", "AccountID");
            AddForeignKey("dbo.Withdrawals", "AccountID", "dbo.Accounts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Transfers", "AccountID", "dbo.Accounts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Deposits", "AccountID", "dbo.Accounts", "Id", cascadeDelete: true);
        }
    }
}
