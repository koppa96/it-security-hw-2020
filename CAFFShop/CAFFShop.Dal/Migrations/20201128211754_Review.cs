using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CAFFShop.Dal.Migrations
{
    public partial class Review : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animations_AspNetUsers_ApprovedById",
                table: "Animations");

            migrationBuilder.DropIndex(
                name: "IX_Animations_ApprovedById",
                table: "Animations");

            migrationBuilder.DropColumn(
                name: "ApprovedById",
                table: "Animations");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Animations");

            migrationBuilder.AddColumn<int>(
                name: "ReviewState",
                table: "Animations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewedById",
                table: "Animations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animations_ReviewedById",
                table: "Animations",
                column: "ReviewedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Animations_AspNetUsers_ReviewedById",
                table: "Animations",
                column: "ReviewedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animations_AspNetUsers_ReviewedById",
                table: "Animations");

            migrationBuilder.DropIndex(
                name: "IX_Animations_ReviewedById",
                table: "Animations");

            migrationBuilder.DropColumn(
                name: "ReviewState",
                table: "Animations");

            migrationBuilder.DropColumn(
                name: "ReviewedById",
                table: "Animations");

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedById",
                table: "Animations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Animations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Animations_ApprovedById",
                table: "Animations",
                column: "ApprovedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Animations_AspNetUsers_ApprovedById",
                table: "Animations",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
