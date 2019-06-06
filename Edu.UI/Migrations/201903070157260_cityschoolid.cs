namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cityschoolid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CitySchools",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 50),
                        schoolname = c.String(),
                        city_id = c.Int(nullable: false),
                        school_type = c.Int(nullable: false),
                        py = c.String(),
                        country_id = c.Int(nullable: false),
                        province_id = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                        sctype = c.Int(nullable: false),
                        user_define = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.SchoolEntities", "CitySchoolId", c => c.String(maxLength: 50));
            CreateIndex("dbo.SchoolEntities", "CitySchoolId");
            AddForeignKey("dbo.SchoolEntities", "CitySchoolId", "dbo.CitySchools", "id");
            DropColumn("dbo.SchoolEntities", "provinceId");
            DropColumn("dbo.SchoolEntities", "cityId");
            DropColumn("dbo.SchoolEntities", "countryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SchoolEntities", "countryId", c => c.Int(nullable: false));
            AddColumn("dbo.SchoolEntities", "cityId", c => c.Int(nullable: false));
            AddColumn("dbo.SchoolEntities", "provinceId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SchoolEntities", "CitySchoolId", "dbo.CitySchools");
            DropIndex("dbo.SchoolEntities", new[] { "CitySchoolId" });
            DropColumn("dbo.SchoolEntities", "CitySchoolId");
            DropTable("dbo.CitySchools");
        }
    }
}
