namespace AmazonPupSpace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dogtableupdated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dogs", "DogOwner", "dbo.AspNetUsers");
            DropIndex("dbo.Dogs", new[] { "DogOwner" });
            AddColumn("dbo.Dogs", "EmployeeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Dogs", "EmployeeId");
            AddForeignKey("dbo.Dogs", "EmployeeId", "dbo.Employees", "EmployeeId", cascadeDelete: true);
            DropColumn("dbo.Dogs", "DogOwner");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dogs", "DogOwner", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Dogs", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Dogs", new[] { "EmployeeId" });
            DropColumn("dbo.Dogs", "EmployeeId");
            CreateIndex("dbo.Dogs", "DogOwner");
            AddForeignKey("dbo.Dogs", "DogOwner", "dbo.AspNetUsers", "Id");
        }
    }
}
