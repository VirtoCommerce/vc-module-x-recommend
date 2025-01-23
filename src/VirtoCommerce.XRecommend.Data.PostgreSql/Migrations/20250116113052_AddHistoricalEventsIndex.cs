using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.XRecommend.Data.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoricalEventsIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HistoricalEvents_EventType_ProductId_StoreId_UserId",
                table: "HistoricalEvents",
                columns: new[] { "EventType", "ProductId", "StoreId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HistoricalEvents_EventType_ProductId_StoreId_UserId",
                table: "HistoricalEvents");
        }
    }
}
