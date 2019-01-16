namespace iConcerto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modele_DodanieEmailDel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "ImageURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "ImageURL");
        }
    }
}
