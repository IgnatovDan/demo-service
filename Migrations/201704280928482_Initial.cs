namespace Demo.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Incidents",
                c => new
                    {
                        Oid = c.Guid(nullable: false),
                        Subject = c.String(nullable: false),
                        Status = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        Id = c.Int(nullable: false),
                        ContactOid = c.Guid(nullable: false),
                        StatusString = c.String(),
                    })
                .PrimaryKey(t => t.Oid)
                .ForeignKey("dbo.People", t => t.ContactOid, cascadeDelete: true)
                .Index(t => t.ContactOid);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Oid = c.Guid(nullable: false),
                        Email = c.String(),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.Oid);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Oid = c.Guid(nullable: false),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        DescriptionText = c.String(),
                        StatusText = c.String(),
                        Incident_Oid = c.Guid(),
                    })
                .PrimaryKey(t => t.Oid)
                .ForeignKey("dbo.Incidents", t => t.Incident_Oid)
                .Index(t => t.Incident_Oid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "Incident_Oid", "dbo.Incidents");
            DropForeignKey("dbo.Incidents", "ContactOid", "dbo.People");
            DropIndex("dbo.Messages", new[] { "Incident_Oid" });
            DropIndex("dbo.Incidents", new[] { "ContactOid" });
            DropTable("dbo.Messages");
            DropTable("dbo.People");
            DropTable("dbo.Incidents");
        }
    }
}
