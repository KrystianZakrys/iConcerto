namespace iConcerto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modele_DodanieEmail1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserDatas", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserDatas", "Email", c => c.String());
        }
    }
}
