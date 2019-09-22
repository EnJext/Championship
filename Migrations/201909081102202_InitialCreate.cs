namespace WebApplicationFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fighters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Age = c.Int(nullable: false),
                        Weight = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fights",
                c => new
                    {
                        FightId = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Rounds = c.Int(nullable: false),
                        WinnerId = c.Int(nullable: false),
                        LosserId = c.Int(nullable: false),
                        Judge1 = c.Int(nullable: false),
                        Judge2 = c.Int(nullable: false),
                        Judge3 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FightId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Fights");
            DropTable("dbo.Fighters");
        }
    }
}
