namespace iConcerto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewBeginnin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDataEvents", "UserData_ID", "dbo.UserDatas");
            DropForeignKey("dbo.UserDataEvents", "Events_ID", "dbo.Events");
            DropIndex("dbo.UserDataEvents", new[] { "UserData_ID" });
            DropIndex("dbo.UserDataEvents", new[] { "Events_ID" });
            DropPrimaryKey("dbo.Events");
            DropPrimaryKey("dbo.UserDatas");
            CreateTable(
                "dbo.EventToUsers",
                c => new
                    {
                        UserDataId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                        Notified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserDataId, t.EventId })
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.UserDatas", t => t.UserDataId, cascadeDelete: true)
                .Index(t => t.UserDataId)
                .Index(t => t.EventId);
            DropColumn("dbo.Events", "ID");
            DropColumn("dbo.UserDatas", "ID");
            DropTable("dbo.UserDataEvents");
            AddColumn("dbo.Events", "EventId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.UserDatas", "UserDataId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Events", "EventId");
            AddPrimaryKey("dbo.UserDatas", "UserDataId");

        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserDataEvents",
                c => new
                    {
                        UserData_ID = c.Int(nullable: false),
                        Events_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserData_ID, t.Events_ID });
            
            AddColumn("dbo.UserDatas", "ID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Events", "ID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.EventToUsers", "UserDataId", "dbo.UserDatas");
            DropForeignKey("dbo.EventToUsers", "EventId", "dbo.Events");
            DropIndex("dbo.EventToUsers", new[] { "EventId" });
            DropIndex("dbo.EventToUsers", new[] { "UserDataId" });
            DropPrimaryKey("dbo.UserDatas");
            DropPrimaryKey("dbo.Events");
            DropColumn("dbo.UserDatas", "UserDataId");
            DropColumn("dbo.Events", "EventId");
            DropTable("dbo.EventToUsers");
            AddPrimaryKey("dbo.UserDatas", "ID");
            AddPrimaryKey("dbo.Events", "ID");
            CreateIndex("dbo.UserDataEvents", "Events_ID");
            CreateIndex("dbo.UserDataEvents", "UserData_ID");
            AddForeignKey("dbo.UserDataEvents", "Events_ID", "dbo.Events", "ID", cascadeDelete: true);
            AddForeignKey("dbo.UserDataEvents", "UserData_ID", "dbo.UserDatas", "ID", cascadeDelete: true);
        }
    }
}
