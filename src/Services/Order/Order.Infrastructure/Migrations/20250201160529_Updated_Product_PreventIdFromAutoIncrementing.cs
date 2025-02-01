using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable


namespace Order.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Product_PreventIdFromAutoIncrementing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //  1: Drop the Foreign Key from OrderItems
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            //  2: Drop Primary Key on Products
            migrationBuilder.DropPrimaryKey("PK_Products", "Products");

            //  3: Create a new column without IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            //  4: Copy existing Id values to NewId
            migrationBuilder.Sql("UPDATE Products SET NewId = Id");

            //  5: Drop the old identity column
            migrationBuilder.DropColumn("Id", "Products");

            //  6: Rename NewId to Id
            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Products",
                newName: "Id");

            //  7: Re-add Primary Key
            migrationBuilder.AddPrimaryKey("PK_Products", "Products", "Id");

            //  8: Re-add Foreign Key to OrderItems
            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade); // Adjust delete behavior if needed
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //  1: Drop the Foreign Key from OrderItems
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            //  2: Drop Primary Key on Products
            migrationBuilder.DropPrimaryKey("PK_Products", "Products");

            //  3: Recreate ID column with IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "OldId",
                table: "Products",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            //  4: Copy back the Id values
            migrationBuilder.Sql("UPDATE Products SET OldId = Id");

            //  5: Drop the modified column
            migrationBuilder.DropColumn("Id", "Products");

            //  6: Rename OldId back to Id
            migrationBuilder.RenameColumn(
                name: "OldId",
                table: "Products",
                newName: "Id");

            //  7: Re-add Primary Key
            migrationBuilder.AddPrimaryKey("PK_Products", "Products", "Id");

            //  8: Re-add Foreign Key to OrderItems
            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade); // Adjust delete behavior if needed
        }
    }
}