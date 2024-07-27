namespace AmazonPupSpace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postandcommenttableadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        CommentText = c.String(nullable: false, maxLength: 100),
                        DateCommented = c.DateTime(nullable: false),
                        PostId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Caption = c.String(),
                        ImageURL = c.Boolean(nullable: false),
                        PicExtension = c.String(),
                        PostDate = c.DateTime(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: false)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Posts", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Comments", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Posts", new[] { "EmployeeId" });
            DropIndex("dbo.Comments", new[] { "EmployeeId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
        }
    }
}
