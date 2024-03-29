﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internship_Application.Migrations
{
    public partial class data_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    wu_id = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    student_questions = table.Column<string>(unicode: false, maxLength: 1000, nullable: false, defaultValueSql: "('{}')"),
                    employer_questions = table.Column<string>(unicode: false, maxLength: 1000, nullable: false, defaultValueSql: "('{}')"),
                    faculty_questions = table.Column<string>(unicode: false, maxLength: 1000, nullable: false, defaultValueSql: "('{}')"),
                    student_services_questions = table.Column<string>(unicode: false, maxLength: 1000, nullable: false, defaultValueSql: "('{}')"),
                    administrator_questions = table.Column<string>(unicode: false, maxLength: 1000, nullable: false, defaultValueSql: "('{}')"),
                    template_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StatusCodes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    status_code = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    details = table.Column<string>(unicode: false, maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusCodes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    retired_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    template_name = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    display_name = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    disclaimer = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    questions = table.Column<string>(unicode: false, nullable: false, defaultValueSql: "('[{\"Prompt\":\"\", \"InputType\": \"\", \"HelperText\": \"\", \"Order\": \"\", \"Role\": \"\", \"Required\": \"\", \"ProcessQuestion\": \"\", \"DateSigned\": \"\", \"Options\": \"[]\"}]')"),
                    is_active = table.Column<bool>(nullable: false),
                    is_retired = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "StatusCodes");

            migrationBuilder.DropTable(
                name: "Templates");
        }
    }
}
