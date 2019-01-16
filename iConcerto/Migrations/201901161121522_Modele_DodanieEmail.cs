namespace iConcerto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modele_DodanieEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDatas", "Email", c => c.String());
            AddColumn("dbo.UserDatas", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserDatas", "ApplicationUser_Id");
            AddForeignKey("dbo.UserDatas", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDatas", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserDatas", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.UserDatas", "ApplicationUser_Id");
            DropColumn("dbo.UserDatas", "Email");
        }
    }
}
