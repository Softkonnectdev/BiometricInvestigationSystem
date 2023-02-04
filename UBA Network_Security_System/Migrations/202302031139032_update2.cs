namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transfers", "CashaierID", "dbo.Employees");
            DropForeignKey("dbo.Employees", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Withdrawals", "CashaierID", "dbo.Employees");
            DropForeignKey("dbo.Deposits", "CashaierID", "dbo.Employees");
            DropIndex("dbo.Employees", new[] { "UserId" });
            AddForeignKey("dbo.Deposits", "CashaierID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Transfers", "CashaierID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Withdrawals", "CashaierID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropTable("dbo.Employees");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        SurName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false, maxLength: 11),
                        DOB = c.DateTime(nullable: false),
                        DOE = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false),
                        Referee = c.String(nullable: false),
                        RefereePhone = c.String(nullable: false),
                        ResidentialAddress = c.String(nullable: false),
                        LGA = c.String(nullable: false),
                        State = c.String(nullable: false),
                        ResidentialCountry = c.String(nullable: false),
                        Nationality = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            DropForeignKey("dbo.Withdrawals", "CashaierID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transfers", "CashaierID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Deposits", "CashaierID", "dbo.AspNetUsers");
            CreateIndex("dbo.Employees", "UserId");
            AddForeignKey("dbo.Deposits", "CashaierID", "dbo.Employees", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Withdrawals", "CashaierID", "dbo.Employees", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Employees", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Transfers", "CashaierID", "dbo.Employees", "UserId", cascadeDelete: true);
        }
    }
}
