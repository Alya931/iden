using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace iden.Data.Migrations
{
    public partial class seedPermissiom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
    
 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Identity].[AspNetUserClaims]");


        }
    }
}
