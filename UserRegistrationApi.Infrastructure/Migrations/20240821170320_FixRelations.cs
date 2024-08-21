using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRegistrationApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_PersonalInformation_AddressId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalInformation_Users_PersonalInformationId",
                table: "PersonalInformation");

            migrationBuilder.DropColumn(
                name: "PersonalInformationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AdressId",
                table: "PersonalInformation");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalInformation_UserId",
                table: "PersonalInformation",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalInformation_Address_PersonalInformationId",
                table: "PersonalInformation",
                column: "PersonalInformationId",
                principalTable: "Address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalInformation_Users_UserId",
                table: "PersonalInformation",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalInformation_Address_PersonalInformationId",
                table: "PersonalInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalInformation_Users_UserId",
                table: "PersonalInformation");

            migrationBuilder.DropIndex(
                name: "IX_PersonalInformation_UserId",
                table: "PersonalInformation");

            migrationBuilder.AddColumn<Guid>(
                name: "PersonalInformationId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AdressId",
                table: "PersonalInformation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Address_PersonalInformation_AddressId",
                table: "Address",
                column: "AddressId",
                principalTable: "PersonalInformation",
                principalColumn: "PersonalInformationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalInformation_Users_PersonalInformationId",
                table: "PersonalInformation",
                column: "PersonalInformationId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
