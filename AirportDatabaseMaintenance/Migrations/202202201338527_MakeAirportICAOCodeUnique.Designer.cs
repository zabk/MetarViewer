﻿// <auto-generated />
namespace AirportDatabaseMaintenance.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.4.4")]
    public sealed partial class MakeAirportICAOCodeUnique : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(MakeAirportICAOCodeUnique));
        
        string IMigrationMetadata.Id
        {
            get { return "202202201338527_MakeAirportICAOCodeUnique"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}