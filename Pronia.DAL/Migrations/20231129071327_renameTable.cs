using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace manytomany.task.Migrations
{
    /// <inheritdoc />
    public partial class renameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tags_products_ProductId",
                table: "tags");

            migrationBuilder.DropForeignKey(
                name: "FK_tags_tagsImage_TagId",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tagsImage",
                table: "tagsImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tags",
                table: "tags");

            migrationBuilder.RenameTable(
                name: "tagsImage",
                newName: "tag");

            migrationBuilder.RenameTable(
                name: "tags",
                newName: "productTag");

            migrationBuilder.RenameIndex(
                name: "IX_tags_TagId",
                table: "productTag",
                newName: "IX_productTag_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_tags_ProductId",
                table: "productTag",
                newName: "IX_productTag_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tag",
                table: "tag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productTag",
                table: "productTag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_productTag_products_ProductId",
                table: "productTag",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_productTag_tag_TagId",
                table: "productTag",
                column: "TagId",
                principalTable: "tag",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productTag_products_ProductId",
                table: "productTag");

            migrationBuilder.DropForeignKey(
                name: "FK_productTag_tag_TagId",
                table: "productTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tag",
                table: "tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productTag",
                table: "productTag");

            migrationBuilder.RenameTable(
                name: "tag",
                newName: "tagsImage");

            migrationBuilder.RenameTable(
                name: "productTag",
                newName: "tags");

            migrationBuilder.RenameIndex(
                name: "IX_productTag_TagId",
                table: "tags",
                newName: "IX_tags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_productTag_ProductId",
                table: "tags",
                newName: "IX_tags_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tagsImage",
                table: "tagsImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tags",
                table: "tags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tags_products_ProductId",
                table: "tags",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tags_tagsImage_TagId",
                table: "tags",
                column: "TagId",
                principalTable: "tagsImage",
                principalColumn: "Id");
        }
    }
}
