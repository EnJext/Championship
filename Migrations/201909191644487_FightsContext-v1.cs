namespace WebApplicationFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FightsContextv1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fights", "Losser_Id", c => c.Int());
            AddColumn("dbo.Fights", "Winner_Id", c => c.Int());
            AddColumn("dbo.Fights", "Fighter_Id", c => c.Int());
            AlterColumn("dbo.Fights", "Time", c => c.DateTime(nullable: false, storeType: "date"));
            CreateIndex("dbo.Fights", "Losser_Id");
            CreateIndex("dbo.Fights", "Winner_Id");
            CreateIndex("dbo.Fights", "Fighter_Id");
            AddForeignKey("dbo.Fights", "Losser_Id", "dbo.Fighters", "Id");
            AddForeignKey("dbo.Fights", "Winner_Id", "dbo.Fighters", "Id");
            AddForeignKey("dbo.Fights", "Fighter_Id", "dbo.Fighters", "Id");
            DropColumn("dbo.Fights", "WinnerId");
            DropColumn("dbo.Fights", "LosserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Fights", "LosserId", c => c.Int(nullable: false));
            AddColumn("dbo.Fights", "WinnerId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Fights", "Fighter_Id", "dbo.Fighters");
            DropForeignKey("dbo.Fights", "Winner_Id", "dbo.Fighters");
            DropForeignKey("dbo.Fights", "Losser_Id", "dbo.Fighters");
            DropIndex("dbo.Fights", new[] { "Fighter_Id" });
            DropIndex("dbo.Fights", new[] { "Winner_Id" });
            DropIndex("dbo.Fights", new[] { "Losser_Id" });
            AlterColumn("dbo.Fights", "Time", c => c.DateTime(nullable: false));
            DropColumn("dbo.Fights", "Fighter_Id");
            DropColumn("dbo.Fights", "Winner_Id");
            DropColumn("dbo.Fights", "Losser_Id");
        }
    }
}
