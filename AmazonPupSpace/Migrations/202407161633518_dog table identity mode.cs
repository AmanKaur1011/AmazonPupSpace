namespace AmazonPupSpace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dogtableidentitymode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dogs",
                c => new
                    {
                        DogId = c.Int(nullable: false, identity: true),
                        DogName = c.String(),
                        DogAge = c.Int(nullable: false),
                        DogBreed = c.String(),
                        DogBirthday = c.DateTime(nullable: false),
                        DogOwner = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DogId)
                .ForeignKey("dbo.AspNetUsers", t => t.DogOwner)
                .Index(t => t.DogOwner);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dogs", "DogOwner", "dbo.AspNetUsers");
            DropIndex("dbo.Dogs", new[] { "DogOwner" });
            DropTable("dbo.Dogs");
        }
    }
}
