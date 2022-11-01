using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace iden.Data.Migrations
{
    public partial class SeedingRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table:"Role",
                columns:new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values:new object[] { Guid.NewGuid().ToString(), "user", "user".ToUpper(), Guid.NewGuid().ToString() },
                             schema: "Identity"


                );
            migrationBuilder.InsertData(
           table: "Role",
           columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
           values: new object[] { Guid.NewGuid().ToString(), "dmin", "dmin".ToUpper(), Guid.NewGuid().ToString() },
                        schema: "Identity"


           );



            

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Identity].[Roles]");
        }
    }
}
