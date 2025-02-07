using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coupon.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Coupon_SellerId_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Coupons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Coupons");
        }
    }
}
