using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRegistrationApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAddressRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalInformation_Address_PersonalInformationId",
                table: "PersonalInformation");

            migrationBuilder.CreateIndex(
                name: "IX_Address_PersonalInformationId",
                table: "Address",
                column: "PersonalInformationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_PersonalInformation_PersonalInformationId",
                table: "Address",
                column: "PersonalInformationId",
                principalTable: "PersonalInformation",
                principalColumn: "PersonalInformationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_PersonalInformation_PersonalInformationId",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_PersonalInformationId",
                table: "Address");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalInformation_Address_PersonalInformationId",
                table: "PersonalInformation",
                column: "PersonalInformationId",
                principalTable: "Address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
