namespace iConcerto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventsToUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "UserData_ID", "dbo.UserDatas");
            DropIndex("dbo.Events", new[] { "UserData_ID" });
            CreateTable(
                "dbo.UserDataEvents",
                c => new
                    {
                        UserData_ID = c.Int(nullable: false),
                        Events_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserData_ID, t.Events_ID })
                .ForeignKey("dbo.UserDatas", t => t.UserData_ID, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.Events_ID, cascadeDelete: true)
                .Index(t => t.UserData_ID)
                .Index(t => t.Events_ID);

            //DropColumn("dbo.Events", "UserId");
            //DropColumn("dbo.Events", "UserData_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "UserData_ID", c => c.Int());
            AddColumn("dbo.Events", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserDataEvents", "Events_ID", "dbo.Events");
            DropForeignKey("dbo.UserDataEvents", "UserData_ID", "dbo.UserDatas");
            DropIndex("dbo.UserDataEvents", new[] { "Events_ID" });
            DropIndex("dbo.UserDataEvents", new[] { "UserData_ID" });
            DropTable("dbo.UserDataEvents");
            CreateIndex("dbo.Events", "UserData_ID");
            AddForeignKey("dbo.Events", "UserData_ID", "dbo.UserDatas", "ID");
        }
    }
}
