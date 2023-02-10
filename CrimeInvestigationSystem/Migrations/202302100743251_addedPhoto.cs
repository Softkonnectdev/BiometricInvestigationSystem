namespace CrimeInvestigationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Crimes", "Photo", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Crimes", "Photo");
        }
    }
}
