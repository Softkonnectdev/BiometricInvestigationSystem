namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatexy : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Deposits", "Remark", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Deposits", "Remark", c => c.String(nullable: false));
        }
    }
}