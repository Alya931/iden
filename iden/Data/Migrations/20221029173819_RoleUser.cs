using Microsoft.EntityFrameworkCore.Migrations;

namespace iden.Data.Migrations
{
    public partial class RoleUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into [Identity].[AspNetUserRoles](UserId ,RoleId) SELECT 'd29a3557-bbb4-4611-95f0-12200d13eee5',Id FROM  [Identity].[Role]");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete FROM [Identity].[AspNetUserRoles] where UserId='d29a3557-bbb4-4611-95f0-12200d13eee5'");
        }
    }
}
