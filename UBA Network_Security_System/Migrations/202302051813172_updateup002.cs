namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateup002 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transfers", "Status", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transfers", "Status", c => c.Boolean(nullable: false));
        }
    }
}
