namespace AirportDatabaseMaintenance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeAirportICAOCodeUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Airports", "AirportICAOCode", c => c.String(maxLength: 4));
            AlterColumn("dbo.Airports", "AirportIATACode", c => c.String(maxLength: 3));
            CreateIndex("dbo.Airports", "AirportICAOCode", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Airports", new[] { "AirportICAOCode" });
            AlterColumn("dbo.Airports", "AirportIATACode", c => c.String());
            AlterColumn("dbo.Airports", "AirportICAOCode", c => c.String());
        }
    }
}
