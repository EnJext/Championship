namespace WebApplicationFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FightsContextv2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Fights", "Fighter_Id", "dbo.Fighters");
            DropForeignKey("dbo.Fights", "Winner_Id", "dbo.Fighters");
            DropIndex("dbo.Fights", new[] { "Winner_Id" });
            DropIndex("dbo.Fights", new[] { "Fighter_Id" });
            DropColumn("dbo.Fights", "Winner_Id");
            RenameColumn(table: "dbo.Fights", name: "Fighter_Id", newName: "Winner_Id");
            AlterColumn("dbo.Fights", "Winner_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Fights", "Winner_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Fights", "Winner_Id");
            AddForeignKey("dbo.Fights", "Winner_Id", "dbo.Fighters", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fights", "Winner_Id", "dbo.Fighters");
            DropIndex("dbo.Fights", new[] { "Winner_Id" });
            AlterColumn("dbo.Fights", "Winner_Id", c => c.Int());
            AlterColumn("dbo.Fights", "Winner_Id", c => c.Int());
            RenameColumn(table: "dbo.Fights", name: "Winner_Id", newName: "Fighter_Id");
            AddColumn("dbo.Fights", "Winner_Id", c => c.Int());
            CreateIndex("dbo.Fights", "Fighter_Id");
            CreateIndex("dbo.Fights", "Winner_Id");
            AddForeignKey("dbo.Fights", "Winner_Id", "dbo.Fighters", "Id");
            AddForeignKey("dbo.Fights", "Fighter_Id", "dbo.Fighters", "Id");
        }
    }
}
