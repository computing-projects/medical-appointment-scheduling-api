using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medical_appointment_scheduling_api.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthDataToAnamnese : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "blood_type",
                table: "Anamnese",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "height_cm",
                table: "Anamnese",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "weight_kg",
                table: "Anamnese",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "blood_type",
                table: "Anamnese");

            migrationBuilder.DropColumn(
                name: "height_cm",
                table: "Anamnese");

            migrationBuilder.DropColumn(
                name: "weight_kg",
                table: "Anamnese");
        }
    }
}
