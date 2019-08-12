using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace wasco_q2.models.migrations
{
    public partial class init_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "m_user",
                columns: table => new
                {
                    m_user_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    full_name = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_user", x => x.m_user_id);
                });

            migrationBuilder.CreateTable(
                name: "t_login_history",
                columns: table => new
                {
                    t_login_history_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    m_user_id = table.Column<int>(nullable: false),
                    login_time = table.Column<DateTime>(nullable: false),
                    logout_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_login_history", x => x.t_login_history_id);
                    table.ForeignKey(
                        name: "FK_t_login_history_m_user_m_user_id",
                        column: x => x.m_user_id,
                        principalTable: "m_user",
                        principalColumn: "m_user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_login_history_m_user_id",
                table: "t_login_history",
                column: "m_user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_login_history");

            migrationBuilder.DropTable(
                name: "m_user");
        }
    }
}
