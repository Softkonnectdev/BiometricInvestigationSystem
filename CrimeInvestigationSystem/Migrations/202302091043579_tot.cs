namespace CrimeInvestigationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "LeftThumb", c => c.String());
            AddColumn("dbo.Profiles", "RightThumb", c => c.String());
            DropColumn("dbo.Profiles", "LThumb");
            DropColumn("dbo.Profiles", "rThumb");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profiles", "rThumb", c => c.String());
            AddColumn("dbo.Profiles", "LThumb", c => c.String());
            DropColumn("dbo.Profiles", "RightThumb");
            DropColumn("dbo.Profiles", "LeftThumb");
        }
    }
}
