using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medical_appointment_scheduling_api.Migrations
{
    /// <inheritdoc />
    public partial class AddCityStateToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "Users",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "state",
                table: "Users");
        }
    }
}
