namespace iConcerto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _105617012019_DodawanieAppNetUserDoUserDataZlokacja : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EventsLocations", newName: "LocationsEvents");
            DropPrimaryKey("dbo.LocationsEvents");
            AddPrimaryKey("dbo.LocationsEvents", new[] { "Locations_ID", "Events_ID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.LocationsEvents");
            AddPrimaryKey("dbo.LocationsEvents", new[] { "Events_ID", "Locations_ID" });
            RenameTable(name: "dbo.LocationsEvents", newName: "EventsLocations");
        }
    }
}
