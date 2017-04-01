namespace BeardBook.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilePost_navigation_property : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Files", name: "Post_Id", newName: "PostId");
            RenameIndex(table: "dbo.Files", name: "IX_Post_Id", newName: "IX_PostId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Files", name: "IX_PostId", newName: "IX_Post_Id");
            RenameColumn(table: "dbo.Files", name: "PostId", newName: "Post_Id");
        }
    }
}
