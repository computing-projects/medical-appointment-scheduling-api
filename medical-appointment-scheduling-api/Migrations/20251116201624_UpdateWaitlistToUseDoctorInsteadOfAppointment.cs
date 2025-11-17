using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medical_appointment_scheduling_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWaitlistToUseDoctorInsteadOfAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the old foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "waitlist_appointment_id_fkey",
                table: "Waitlist");

            // Rename the column
            migrationBuilder.RenameColumn(
                name: "appointment_id",
                table: "Waitlist",
                newName: "doctor_id");

            // Add clinic_id column
            migrationBuilder.AddColumn<int>(
                name: "clinic_id",
                table: "Waitlist",
                type: "integer",
                nullable: true);

            // Add foreign key constraint for doctor_id
            migrationBuilder.AddForeignKey(
                name: "waitlist_doctor_id_fkey",
                table: "Waitlist",
                column: "doctor_id",
                principalTable: "Doctors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            // Add foreign key constraint for clinic_id (optional)
            migrationBuilder.AddForeignKey(
                name: "waitlist_clinic_id_fkey",
                table: "Waitlist",
                column: "clinic_id",
                principalTable: "Clinics",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the new foreign key constraints
            migrationBuilder.DropForeignKey(
                name: "waitlist_clinic_id_fkey",
                table: "Waitlist");

            migrationBuilder.DropForeignKey(
                name: "waitlist_doctor_id_fkey",
                table: "Waitlist");

            // Drop clinic_id column
            migrationBuilder.DropColumn(
                name: "clinic_id",
                table: "Waitlist");

            // Rename the column back
            migrationBuilder.RenameColumn(
                name: "doctor_id",
                table: "Waitlist",
                newName: "appointment_id");

            // Restore the old foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "waitlist_appointment_id_fkey",
                table: "Waitlist",
                column: "appointment_id",
                principalTable: "Appointments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
