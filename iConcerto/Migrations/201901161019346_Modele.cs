namespace iConcerto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modele : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Lat = c.Single(nullable: false),
                        Lng = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                        UserData_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserDatas", t => t.UserData_ID)
                .Index(t => t.UserData_ID);
            
            CreateTable(
                "dbo.UserDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstMidName = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Locations_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Locations", t => t.Locations_ID)
                .Index(t => t.Locations_ID);
            
            CreateTable(
                "dbo.EventsLocations",
                c => new
                    {
                        Events_ID = c.Int(nullable: false),
                        Locations_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Events_ID, t.Locations_ID })
                .ForeignKey("dbo.Events", t => t.Events_ID, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.Locations_ID, cascadeDelete: true)
                .Index(t => t.Events_ID)
                .Index(t => t.Locations_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDatas", "Locations_ID", "dbo.Locations");
            DropForeignKey("dbo.Events", "UserData_ID", "dbo.UserDatas");
            DropForeignKey("dbo.EventsLocations", "Locations_ID", "dbo.Locations");
            DropForeignKey("dbo.EventsLocations", "Events_ID", "dbo.Events");
            DropIndex("dbo.EventsLocations", new[] { "Locations_ID" });
            DropIndex("dbo.EventsLocations", new[] { "Events_ID" });
            DropIndex("dbo.UserDatas", new[] { "Locations_ID" });
            DropIndex("dbo.Events", new[] { "UserData_ID" });
            DropTable("dbo.EventsLocations");
            DropTable("dbo.UserDatas");
            DropTable("dbo.Events");
            DropTable("dbo.Locations");
        }
    }
}
