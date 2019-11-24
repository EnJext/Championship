namespace WebApplicationFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fight_WinnerFKUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Fights", "Fighter_Id", "dbo.Fighters");
            DropIndex("dbo.Fights", new[] { "Fighter_Id" });
            DropColumn("dbo.Fights", "Fighter_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Fights", "Fighter_Id", c => c.Int());
            CreateIndex("dbo.Fights", "Fighter_Id");
            AddForeignKey("dbo.Fights", "Fighter_Id", "dbo.Fighters", "Id");
        }
    }
}
