namespace BeardBook.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Conversation_Active_Field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conversations", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conversations", "Active");
        }
    }
}
