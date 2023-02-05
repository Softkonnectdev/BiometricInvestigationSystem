namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateup003 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Transfers", "SecurePass");
            DropColumn("dbo.Withdrawals", "SecurePass");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Withdrawals", "SecurePass", c => c.String(nullable: false, maxLength: 6));
            AddColumn("dbo.Transfers", "SecurePass", c => c.String(nullable: false, maxLength: 6));
        }
    }
}
