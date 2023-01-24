namespace UBA_Network_Security_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AccountTypeID = c.String(nullable: false),
                        AccountNumber = c.String(nullable: false, maxLength: 10),
                        SurName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false, maxLength: 11),
                        DOB = c.DateTime(nullable: false),
                        DOO = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false),
                        ResidentialAddress = c.String(nullable: false),
                        PermanentResident = c.String(nullable: false),
                        LGA = c.String(nullable: false),
                        State = c.String(nullable: false),
                        ResidentialCountry = c.String(nullable: false),
                        Nationality = c.String(nullable: false),
                        BVN = c.String(nullable: false, maxLength: 11),
                        BranchOffice = c.String(nullable: false),
                        MaritalStatus = c.String(nullable: false),
                        Occupation = c.String(nullable: false),
                        IntroducedBy = c.String(),
                        SecurePass = c.String(nullable: false, maxLength: 6),
                        Signature = c.Binary(nullable: false),
                        Passport = c.Binary(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IdentificationType = c.String(nullable: false),
                        IdentificationNumber = c.String(nullable: false),
                        IdentificationIssuedDate = c.String(nullable: false),
                        IdentificationValidTill = c.String(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AccountManagerName = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BVNBanks", t => t.BVN, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.BVN);
            
            CreateTable(
                "dbo.BVNBanks",
                c => new
                    {
                        BVN = c.String(nullable: false, maxLength: 11),
                        SurName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false, maxLength: 11),
                        DOB = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BVN);
            
            CreateTable(
                "dbo.Deposits",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AccountNumber = c.String(nullable: false, maxLength: 10),
                        AccountName = c.String(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Boolean(nullable: false),
                        Remark = c.String(nullable: false),
                        DepositorName = c.String(nullable: false),
                        DepositorPhone = c.String(nullable: false),
                        Cashaier = c.String(nullable: false),
                        AccountID = c.String(nullable: false, maxLength: 128),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountID, cascadeDelete: true)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.Transfers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AccountNumber = c.String(nullable: false, maxLength: 10),
                        AccountName = c.String(nullable: false),
                        SenderName = c.String(nullable: false),
                        SenderPhone = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Remark = c.String(nullable: false),
                        SecurePass = c.String(nullable: false, maxLength: 6),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AccountID = c.String(nullable: false, maxLength: 128),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountID, cascadeDelete: true)
                .Index(t => t.AccountID);
            
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
                "dbo.Employees",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        SurName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(nullable: false),
                        Email = c.String(nullable: false),
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
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
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
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Withdrawals",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AccountNumber = c.String(nullable: false, maxLength: 10),
                        AccountName = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Remark = c.String(nullable: false),
                        SecurePass = c.String(nullable: false, maxLength: 6),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AccountID = c.String(nullable: false, maxLength: 128),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountID, cascadeDelete: true)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.AccountStatements",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TransactionType = c.String(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Boolean(nullable: false),
                        Remark = c.String(nullable: false),
                        TransactionId = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Event = c.String(nullable: false),
                        Initiator = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.TransactionLedgers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TransactionId = c.String(nullable: false),
                        AccountType = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Withdrawals", "AccountID", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Employees", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transfers", "AccountID", "dbo.Accounts");
            DropForeignKey("dbo.Deposits", "AccountID", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "BVN", "dbo.BVNBanks");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Withdrawals", new[] { "AccountID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Employees", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Transfers", new[] { "AccountID" });
            DropIndex("dbo.Deposits", new[] { "AccountID" });
            DropIndex("dbo.Accounts", new[] { "BVN" });
            DropIndex("dbo.Accounts", new[] { "UserId" });
            DropTable("dbo.TransactionLedgers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Logs");
            DropTable("dbo.AccountStatements");
            DropTable("dbo.Withdrawals");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Employees");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Transfers");
            DropTable("dbo.Deposits");
            DropTable("dbo.BVNBanks");
            DropTable("dbo.Accounts");
        }
    }
}
