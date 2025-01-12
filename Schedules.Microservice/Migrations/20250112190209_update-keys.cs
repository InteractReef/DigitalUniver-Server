using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Schedules.Microservice.Migrations
{
    /// <inheritdoc />
    public partial class updatekeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleItems_Schedules_ScheduleId",
                table: "ScheduleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleItems_Schedules_ScheduleId1",
                table: "ScheduleItems");

            migrationBuilder.RenameColumn(
                name: "ScheduleId1",
                table: "ScheduleItems",
                newName: "ScheduleIdNumerator");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "ScheduleItems",
                newName: "ScheduleIdDenominator");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleItems_ScheduleId1",
                table: "ScheduleItems",
                newName: "IX_ScheduleItems_ScheduleIdNumerator");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleItems_ScheduleId",
                table: "ScheduleItems",
                newName: "IX_ScheduleItems_ScheduleIdDenominator");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SubjectItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrgId",
                table: "Schedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleItems_Schedules_ScheduleIdDenominator",
                table: "ScheduleItems",
                column: "ScheduleIdDenominator",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleItems_Schedules_ScheduleIdNumerator",
                table: "ScheduleItems",
                column: "ScheduleIdNumerator",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleItems_Schedules_ScheduleIdDenominator",
                table: "ScheduleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleItems_Schedules_ScheduleIdNumerator",
                table: "ScheduleItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SubjectItems");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "ScheduleIdNumerator",
                table: "ScheduleItems",
                newName: "ScheduleId1");

            migrationBuilder.RenameColumn(
                name: "ScheduleIdDenominator",
                table: "ScheduleItems",
                newName: "ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleItems_ScheduleIdNumerator",
                table: "ScheduleItems",
                newName: "IX_ScheduleItems_ScheduleId1");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleItems_ScheduleIdDenominator",
                table: "ScheduleItems",
                newName: "IX_ScheduleItems_ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleItems_Schedules_ScheduleId",
                table: "ScheduleItems",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleItems_Schedules_ScheduleId1",
                table: "ScheduleItems",
                column: "ScheduleId1",
                principalTable: "Schedules",
                principalColumn: "Id");
        }
    }
}
