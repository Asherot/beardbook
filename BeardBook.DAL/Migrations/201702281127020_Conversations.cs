namespace BeardBook.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Conversations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Created = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        ConversationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conversations", t => t.ConversationId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ConversationId);
            
            CreateTable(
                "dbo.UserConversations",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Conversation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Conversation_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Conversations", t => t.Conversation_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Conversation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserConversations", "Conversation_Id", "dbo.Conversations");
            DropForeignKey("dbo.UserConversations", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "ConversationId", "dbo.Conversations");
            DropIndex("dbo.UserConversations", new[] { "Conversation_Id" });
            DropIndex("dbo.UserConversations", new[] { "User_Id" });
            DropIndex("dbo.Messages", new[] { "ConversationId" });
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropTable("dbo.UserConversations");
            DropTable("dbo.Messages");
            DropTable("dbo.Conversations");
        }
    }
}
