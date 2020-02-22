namespace WebBanSach_2_0.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editdobIdentity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Dob", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Dob", c => c.DateTime(nullable: false));
        }
    }
}
