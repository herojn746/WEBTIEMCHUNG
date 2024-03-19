using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemChung.Migrations
{
    /// <inheritdoc />
    public partial class version4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CMND",
                table: "ThongTinTiemChung",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaHoGiaDinh",
                table: "ThongTinTiemChung",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenNguoiDK",
                table: "ThongTinTiemChung",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HoGiaDinh",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenChuHo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoGiaDinh", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinTiemChung_MaHoGiaDinh",
                table: "ThongTinTiemChung",
                column: "MaHoGiaDinh");

            migrationBuilder.AddForeignKey(
                name: "FK_ThongTinTiemChung_HoGiaDinh_MaHoGiaDinh",
                table: "ThongTinTiemChung",
                column: "MaHoGiaDinh",
                principalTable: "HoGiaDinh",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThongTinTiemChung_HoGiaDinh_MaHoGiaDinh",
                table: "ThongTinTiemChung");

            migrationBuilder.DropTable(
                name: "HoGiaDinh");

            migrationBuilder.DropIndex(
                name: "IX_ThongTinTiemChung_MaHoGiaDinh",
                table: "ThongTinTiemChung");

            migrationBuilder.DropColumn(
                name: "CMND",
                table: "ThongTinTiemChung");

            migrationBuilder.DropColumn(
                name: "MaHoGiaDinh",
                table: "ThongTinTiemChung");

            migrationBuilder.DropColumn(
                name: "TenNguoiDK",
                table: "ThongTinTiemChung");
        }
    }
}
