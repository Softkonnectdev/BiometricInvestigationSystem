namespace CrimeInvestigationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Crimes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Type = c.String(nullable: false),
                        Tried = c.Boolean(nullable: false),
                        AddressofCrime = c.String(nullable: false),
                        DefaultedLaw = c.String(nullable: false),
                        CommittedDate = c.DateTime(nullable: false),
                        Photo = c.Binary(),
                        ProfileID = c.String(nullable: false, maxLength: 10),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profiles", t => t.ProfileID, cascadeDelete: true)
                .Index(t => t.ProfileID);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        ProfileId = c.String(nullable: false, maxLength: 10),
                        SurName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Passport = c.Binary(),
                        LeftThumb = c.String(),
                        RightThumb = c.String(),
                        Phone = c.String(nullable: false, maxLength: 11),
                        DOB = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false),
                        ResidentialAddress = c.String(nullable: false),
                        PermanentResident = c.String(nullable: false),
                        LGA = c.String(nullable: false),
                        State = c.String(nullable: false),
                        ResidentialCountry = c.String(nullable: false),
                        Nationality = c.String(nullable: false),
                        MaritalStatus = c.String(nullable: false),
                        NextofKinName = c.String(nullable: false),
                        NextofKinPhone = c.String(nullable: false),
                        Occupation = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IdentificationType = c.String(nullable: false),
                        IdentificationNumber = c.String(nullable: false),
                        IdentificationIssuedDate = c.Int(nullable: false),
                        IdentificationValidTill = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProfileId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Crimes", "ProfileID", "dbo.Profiles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Crimes", new[] { "ProfileID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Profiles");
            DropTable("dbo.Crimes");
        }
    }
}
