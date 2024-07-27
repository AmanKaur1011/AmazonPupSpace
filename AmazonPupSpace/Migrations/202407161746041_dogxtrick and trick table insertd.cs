namespace AmazonPupSpace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dogxtrickandtricktableinsertd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DogxTricks",
                c => new
                    {
                        DogTrickId = c.Int(nullable: false, identity: true),
                        DogTrickDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DogTrickId);
            
            CreateTable(
                "dbo.Tricks",
                c => new
                    {
                        TrickId = c.Int(nullable: false, identity: true),
                        TrickName = c.String(),
                        TrickVideoLink = c.String(),
                        TrickDescription = c.String(),
                        TrickDifficulty = c.String(),
                    })
                .PrimaryKey(t => t.TrickId);
            
            CreateTable(
                "dbo.TrickDogxTricks",
                c => new
                    {
                        Trick_TrickId = c.Int(nullable: false),
                        DogxTrick_DogTrickId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trick_TrickId, t.DogxTrick_DogTrickId })
                .ForeignKey("dbo.Tricks", t => t.Trick_TrickId, cascadeDelete: true)
                .ForeignKey("dbo.DogxTricks", t => t.DogxTrick_DogTrickId, cascadeDelete: true)
                .Index(t => t.Trick_TrickId)
                .Index(t => t.DogxTrick_DogTrickId);
            
            AddColumn("dbo.Dogs", "DogxTrick_DogTrickId", c => c.Int());
            CreateIndex("dbo.Dogs", "DogxTrick_DogTrickId");
            AddForeignKey("dbo.Dogs", "DogxTrick_DogTrickId", "dbo.DogxTricks", "DogTrickId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrickDogxTricks", "DogxTrick_DogTrickId", "dbo.DogxTricks");
            DropForeignKey("dbo.TrickDogxTricks", "Trick_TrickId", "dbo.Tricks");
            DropForeignKey("dbo.Dogs", "DogxTrick_DogTrickId", "dbo.DogxTricks");
            DropIndex("dbo.TrickDogxTricks", new[] { "DogxTrick_DogTrickId" });
            DropIndex("dbo.TrickDogxTricks", new[] { "Trick_TrickId" });
            DropIndex("dbo.Dogs", new[] { "DogxTrick_DogTrickId" });
            DropColumn("dbo.Dogs", "DogxTrick_DogTrickId");
            DropTable("dbo.TrickDogxTricks");
            DropTable("dbo.Tricks");
            DropTable("dbo.DogxTricks");
        }
    }
}
