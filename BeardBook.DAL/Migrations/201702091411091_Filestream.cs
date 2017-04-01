namespace BeardBook.DAL.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Configuration;
    
    public partial class Filestream : DbMigration
    {
        public override void Up()
        {
            var filestreamGroupName = ConfigurationManager.AppSettings["FilestreamGroupName"];
            var filestreamFileName = ConfigurationManager.AppSettings["FilestreamFileName"];
            var filestreamPath = ConfigurationManager.AppSettings["FilestreamPath"];

            Sql($"alter database BeardBook add filegroup {filestreamGroupName} contains FILESTREAM", true);
            Sql($@"alter database BeardBook add file (NAME='{filestreamFileName}', FILENAME='{filestreamPath}{filestreamFileName}') to filegroup {filestreamGroupName}", true);
            Sql($"alter table [dbo].[Files] set (filestream_on={filestreamGroupName})", true);

            Sql("alter table [dbo].[Files] add [RowId] uniqueidentifier rowguidcol not null");
            Sql("alter table [dbo].[Files] add constraint [UQ_Files_RowId] UNIQUE NONCLUSTERED ([RowId])");
            Sql("alter table [dbo].[Files] add constraint [DF_Files_RowId] default (newid()) for [RowId]");
            Sql("alter table [dbo].[Files] add [Data] varbinary(max) FILESTREAM not null");
            Sql("alter table [dbo].[Files] add constraint [DF_Files_Data] default(0x) for [Data]");
        }

        public override void Down()
        {
            Sql("alter table [dbo].[Files] drop constraint [DF_Files_Data]");
            Sql("alter table [dbo].[Files] drop column [Data]");
            Sql("alter table [dbo].[Files] drop constraint [UQ_Files_RowId]");
            Sql("alter table [dbo].[Files] drop constraint [DF_Files_RowId]");
            Sql("alter table [dbo].[Files] drop column [RowId]");
        }
    }
}
