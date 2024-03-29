﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    rId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Desc = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.rId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    uId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Mail = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.uId);
                });

            migrationBuilder.CreateTable(
                name: "UserRestaurantRate",
                columns: table => new
                {
                    uId = table.Column<Guid>(type: "uuid", nullable: true),
                    rId = table.Column<Guid>(type: "uuid", nullable: true),
                    rating = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                });
            // InsertUserData
            migrationBuilder.InsertData(
                table: "User",
                columns: new[]{
                    "uId",
                    "UserName",
                    "Password",
                    "Mail",
                    "Role"
                },
                values: new object[,]{
                    {Guid.NewGuid().ToString(),"WenTee", "jp4wu6", "god@gmail.com", "Admin"}
                }
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Restaurant");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserRestaurantRate");
        }
    }
}
