namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserLsn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLessons",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        TrainBaseLessonId = c.String(nullable: false, maxLength: 128),
                        VcrId = c.String(nullable: false, maxLength: 128),
                        TimeSpanViewed = c.Double(nullable: false),
                        LastViewDay = c.DateTime(nullable: false),
                        Memo = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => new { t.UserId, t.TrainBaseLessonId, t.VcrId })
                .ForeignKey("dbo.TrainBaseLessons", t => t.TrainBaseLessonId, cascadeDelete: true)
                .ForeignKey("dbo.Vcrs", t => t.VcrId, cascadeDelete: true)
                .Index(t => t.TrainBaseLessonId)
                .Index(t => t.VcrId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLessons", "VcrId", "dbo.Vcrs");
            DropForeignKey("dbo.UserLessons", "TrainBaseLessonId", "dbo.TrainBaseLessons");
            DropIndex("dbo.UserLessons", new[] { "VcrId" });
            DropIndex("dbo.UserLessons", new[] { "TrainBaseLessonId" });
            DropTable("dbo.UserLessons");
        }
    }
}
