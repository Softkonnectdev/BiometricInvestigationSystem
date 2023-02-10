namespace CrimeInvestigationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebv : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Profiles", "lThumb", c => c.String());
            AlterColumn("dbo.Profiles", "rThumb", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Profiles", "rThumb", c => c.String(nullable: false));
            AlterColumn("dbo.Profiles", "lThumb", c => c.String(nullable: false));
        }
    }
}
