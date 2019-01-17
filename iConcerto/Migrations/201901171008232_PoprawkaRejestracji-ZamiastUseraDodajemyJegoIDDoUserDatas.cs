namespace iConcerto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PoprawkaRejestracjiZamiastUseraDodajemyJegoIDDoUserDatas : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDatas", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserDatas", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.UserDatas", "ApplicationUserId", c => c.String());
            DropColumn("dbo.UserDatas", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserDatas", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.UserDatas", "ApplicationUserId");
            CreateIndex("dbo.UserDatas", "ApplicationUser_Id");
            AddForeignKey("dbo.UserDatas", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
