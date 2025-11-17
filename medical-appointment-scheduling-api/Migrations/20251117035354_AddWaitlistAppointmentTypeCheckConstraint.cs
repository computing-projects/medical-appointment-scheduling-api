using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medical_appointment_scheduling_api.Migrations
{
    /// <inheritdoc />
    public partial class AddWaitlistAppointmentTypeCheckConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update existing rows to have default value
            migrationBuilder.Sql("UPDATE \"Waitlist\" SET appointment_type = 'in_person' WHERE appointment_type = '' OR appointment_type IS NULL");

            // Change column type and set default
            migrationBuilder.AlterColumn<string>(
                name: "appointment_type",
                table: "Waitlist",
                type: "character varying",
                nullable: false,
                defaultValue: "in_person",
                oldClrType: typeof(string),
                oldType: "text");

            // Add CHECK constraint
            migrationBuilder.AddCheckConstraint(
                name: "waitlist_appointment_type_check",
                table: "Waitlist",
                sql: "appointment_type::text = ANY (ARRAY['in_person'::character varying, 'online'::character varying]::text[])");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop constraint if it exists
            migrationBuilder.Sql("ALTER TABLE \"Waitlist\" DROP CONSTRAINT IF EXISTS waitlist_appointment_type_check;");

            migrationBuilder.AlterColumn<string>(
                name: "appointment_type",
                table: "Waitlist",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldDefaultValue: "in_person");
        }
    }
}
