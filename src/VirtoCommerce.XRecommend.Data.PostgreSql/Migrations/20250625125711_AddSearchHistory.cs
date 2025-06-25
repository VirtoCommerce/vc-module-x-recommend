using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.XRecommend.Data.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchQuery",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    UserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    OrganizationId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    StoreId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Query = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ModifiedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchQuery", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchQuery_UserId_OrganizationId_StoreId",
                table: "SearchQuery",
                columns: new[] { "UserId", "OrganizationId", "StoreId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchQuery");
        }
    }
}
