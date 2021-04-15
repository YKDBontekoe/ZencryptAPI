﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.EF.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AddBy",
                table: "UserLikedPost",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddBy",
                table: "UserLikedPost");
        }
    }
}
