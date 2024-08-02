namespace AmazonPupSpace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedtheDog : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dogs", "DogxTrick_DogTrickId", "dbo.DogxTricks");
            DropForeignKey("dbo.TrickDogxTricks", "Trick_TrickId", "dbo.Tricks");
            DropForeignKey("dbo.TrickDogxTricks", "DogxTrick_DogTrickId", "dbo.DogxTricks");
            DropIndex("dbo.Dogs", new[] { "DogxTrick_DogTrickId" });
            DropIndex("dbo.TrickDogxTricks", new[] { "Trick_TrickId" });
            DropIndex("dbo.TrickDogxTricks", new[] { "DogxTrick_DogTrickId" });
            AddColumn("dbo.DogxTricks", "DogId", c => c.Int(nullable: false));
            AddColumn("dbo.DogxTricks", "TrickId", c => c.Int(nullable: false));
            CreateIndex("dbo.DogxTricks", "DogId");
            CreateIndex("dbo.DogxTricks", "TrickId");
            AddForeignKey("dbo.DogxTricks", "DogId", "dbo.Dogs", "DogId", cascadeDelete: true);
            AddForeignKey("dbo.DogxTricks", "TrickId", "dbo.Tricks", "TrickId", cascadeDelete: true);
            DropColumn("dbo.Dogs", "DogxTrick_DogTrickId");
            DropTable("dbo.TrickDogxTricks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TrickDogxTricks",
                c => new
                    {
                        Trick_TrickId = c.Int(nullable: false),
                        DogxTrick_DogTrickId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trick_TrickId, t.DogxTrick_DogTrickId });
            
            AddColumn("dbo.Dogs", "DogxTrick_DogTrickId", c => c.Int());
            DropForeignKey("dbo.DogxTricks", "TrickId", "dbo.Tricks");
            DropForeignKey("dbo.DogxTricks", "DogId", "dbo.Dogs");
            DropIndex("dbo.DogxTricks", new[] { "TrickId" });
            DropIndex("dbo.DogxTricks", new[] { "DogId" });
            DropColumn("dbo.DogxTricks", "TrickId");
            DropColumn("dbo.DogxTricks", "DogId");
            CreateIndex("dbo.TrickDogxTricks", "DogxTrick_DogTrickId");
            CreateIndex("dbo.TrickDogxTricks", "Trick_TrickId");
            CreateIndex("dbo.Dogs", "DogxTrick_DogTrickId");
            AddForeignKey("dbo.TrickDogxTricks", "DogxTrick_DogTrickId", "dbo.DogxTricks", "DogTrickId", cascadeDelete: true);
            AddForeignKey("dbo.TrickDogxTricks", "Trick_TrickId", "dbo.Tricks", "TrickId", cascadeDelete: true);
            AddForeignKey("dbo.Dogs", "DogxTrick_DogTrickId", "dbo.DogxTricks", "DogTrickId");
        }
    }
}
