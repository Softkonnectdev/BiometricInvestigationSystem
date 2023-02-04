namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatexyr : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Deposits", "Status", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Deposits", "Status", c => c.Boolean(nullable: false));
        }
    }
}
