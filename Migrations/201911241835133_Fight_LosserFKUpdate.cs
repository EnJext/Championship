namespace WebApplicationFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fight_LosserFKUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Fights", "Winner_Id", "dbo.Fighters");
            DropForeignKey("dbo.Fights", "Losser_Id", "dbo.Fighters");
            DropIndex("dbo.Fights", new[] { "Losser_Id" });
            AddColumn("dbo.Fights", "Fighter_Id", c => c.Int());
            AlterColumn("dbo.Fights", "Losser_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Fights", "Losser_Id");
            CreateIndex("dbo.Fights", "Fighter_Id");
            AddForeignKey("dbo.Fights", "Fighter_Id", "dbo.Fighters", "Id");
            AddForeignKey("dbo.Fights", "Losser_Id", "dbo.Fighters", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fights", "Losser_Id", "dbo.Fighters");
            DropForeignKey("dbo.Fights", "Fighter_Id", "dbo.Fighters");
            DropIndex("dbo.Fights", new[] { "Fighter_Id" });
            DropIndex("dbo.Fights", new[] { "Losser_Id" });
            AlterColumn("dbo.Fights", "Losser_Id", c => c.Int());
            DropColumn("dbo.Fights", "Fighter_Id");
            CreateIndex("dbo.Fights", "Losser_Id");
            AddForeignKey("dbo.Fights", "Losser_Id", "dbo.Fighters", "Id");
            AddForeignKey("dbo.Fights", "Winner_Id", "dbo.Fighters", "Id", cascadeDelete: true);
        }
    }
}
