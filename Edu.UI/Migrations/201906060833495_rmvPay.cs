namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rmvPay : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.LiveHostShows", "QrPay");
            DropColumn("dbo.LiveHostShows", "QrAlipay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LiveHostShows", "QrAlipay", c => c.Binary());
            AddColumn("dbo.LiveHostShows", "QrPay", c => c.Binary());
        }
    }
}
