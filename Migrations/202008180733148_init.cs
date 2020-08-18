namespace LibraryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Publishing = c.String(),
                        DateOfPublish = c.DateTime(nullable: false),
                        AviableCount = c.Int(nullable: false),
                        TotalPages = c.Int(nullable: false),
                        PictureId = c.Int(),
                        AuthorId = c.Int(),
                        SubjectId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authors", t => t.AuthorId)
                .ForeignKey("dbo.Pictures", t => t.PictureId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .Index(t => t.PictureId)
                .Index(t => t.AuthorId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.BookGivings",
                c => new
                    {
                        BooksGivingId = c.Int(nullable: false, identity: true),
                        BookId = c.Int(),
                        ApplicationUserId = c.String(maxLength: 128),
                        DateGiving = c.DateTime(nullable: false),
                        IsReturned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BooksGivingId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Books", t => t.BookId)
                .Index(t => t.BookId)
                .Index(t => t.ApplicationUserId);
            
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
                "dbo.Pages",
                c => new
                    {
                        PageId = c.Int(nullable: false, identity: true),
                        BytesText = c.Binary(),
                        Number = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PageId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        PictureId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.PictureId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Books", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Books", "PictureId", "dbo.Pictures");
            DropForeignKey("dbo.Pages", "BookId", "dbo.Books");
            DropForeignKey("dbo.BookGivings", "BookId", "dbo.Books");
            DropForeignKey("dbo.BookGivings", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Books", "AuthorId", "dbo.Authors");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Pages", new[] { "BookId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.BookGivings", new[] { "ApplicationUserId" });
            DropIndex("dbo.BookGivings", new[] { "BookId" });
            DropIndex("dbo.Books", new[] { "SubjectId" });
            DropIndex("dbo.Books", new[] { "AuthorId" });
            DropIndex("dbo.Books", new[] { "PictureId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Subjects");
            DropTable("dbo.Pictures");
            DropTable("dbo.Pages");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.BookGivings");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
