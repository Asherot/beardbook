namespace BeardBook.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Consistent_DateTime_Properties : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Posts", name: "DateTime", newName: "Created");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Posts", name: "Created", newName: "DateTime");
        }
    }
}
