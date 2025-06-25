using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwtUsers.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSubscriptionToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NewSubscription",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewSubscription",
                table: "Users");
        }
    }
}
