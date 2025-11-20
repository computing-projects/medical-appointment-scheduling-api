using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medical_appointment_scheduling_api.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentTypeAndReasonToWaitlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "appointment_type",
                table: "Waitlist",
                type: "character varying",
                nullable: false,
                defaultValue: "in_person");

            migrationBuilder.AddCheckConstraint(
                name: "waitlist_appointment_type_check",
                table: "Waitlist",
                sql: "appointment_type::text = ANY (ARRAY['in_person'::character varying, 'online'::character varying]::text[])");

            migrationBuilder.AddColumn<string>(
                name: "reason",
                table: "Waitlist",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "waitlist_appointment_type_check",
                table: "Waitlist");

            migrationBuilder.DropColumn(
                name: "appointment_type",
                table: "Waitlist");

            migrationBuilder.DropColumn(
                name: "reason",
                table: "Waitlist");
        }
    }
}
