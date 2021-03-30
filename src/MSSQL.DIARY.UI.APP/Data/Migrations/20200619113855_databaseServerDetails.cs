using Microsoft.EntityFrameworkCore.Migrations;

namespace MSSQL.DIARY.UI.APP.Data.Migrations
{
    public partial class databaseServerDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.CreateTable(
               name: "DatabaseServerDetails",
               columns: table => new
               {
                   Id = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                   ServerName = table.Column<string>( nullable: true),
                   DatabaseConnection = table.Column<string>( nullable: true), 
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_DatabaseServerDetails", x => x.Id);
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                            name: "DatabaseServerDetails");
        }
    }
}
