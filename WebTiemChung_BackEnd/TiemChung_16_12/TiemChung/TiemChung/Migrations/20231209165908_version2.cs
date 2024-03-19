using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemChung.Migrations
{
    /// <inheritdoc />
    public partial class version2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CMND = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoaiVaccine",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenLoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiVaccine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNCC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCap", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNhanVien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CMND = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vaccine",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenVaccine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NhaSanXuat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    NgaySX = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GiaTien = table.Column<float>(type: "real", nullable: false),
                    MaLoaiVaccine = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vaccine_LoaiVaccine_MaLoaiVaccine",
                        column: x => x.MaLoaiVaccine,
                        principalTable: "LoaiVaccine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoaiTiemChung",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenLoaiTiem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiaTien = table.Column<float>(type: "real", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiTiemChung", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoaiTiemChung_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NhapVaccine",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNhaCungCap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhapVaccine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhapVaccine_NhaCungCap_MaNhaCungCap",
                        column: x => x.MaNhaCungCap,
                        principalTable: "NhaCungCap",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NhapVaccine_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "XuatVaccine",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XuatVaccine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XuatVaccine_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietVaccine",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoTuoi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TanSo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LieuLuong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaVaccine = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietVaccine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietVaccine_Vaccine_MaVaccine",
                        column: x => x.MaVaccine,
                        principalTable: "Vaccine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoiTiemChung",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiamGia = table.Column<float>(type: "real", nullable: false),
                    TongTien = table.Column<float>(type: "real", nullable: false),
                    MaLoaiTiemChung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoiTiemChung", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoiTiemChung_LoaiTiemChung_MaLoaiTiemChung",
                        column: x => x.MaLoaiTiemChung,
                        principalTable: "LoaiTiemChung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CTNhapVaccine",
                columns: table => new
                {
                    MaVaccine = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaNhapVaccine = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTNhapVaccine", x => new { x.MaVaccine, x.MaNhapVaccine });
                    table.ForeignKey(
                        name: "FK_CTNhapVaccine_NhapVaccine_MaNhapVaccine",
                        column: x => x.MaNhapVaccine,
                        principalTable: "NhapVaccine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CTNhapVaccine_Vaccine_MaVaccine",
                        column: x => x.MaVaccine,
                        principalTable: "Vaccine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CTXuatVaccine",
                columns: table => new
                {
                    MaVaccine = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaXuatVaccine = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTXuatVaccine", x => new { x.MaVaccine, x.MaXuatVaccine });
                    table.ForeignKey(
                        name: "FK_CTXuatVaccine_Vaccine_MaVaccine",
                        column: x => x.MaVaccine,
                        principalTable: "Vaccine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CTXuatVaccine_XuatVaccine_MaXuatVaccine",
                        column: x => x.MaXuatVaccine,
                        principalTable: "XuatVaccine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CTGoiTiemChung",
                columns: table => new
                {
                    MaGoiTiemChung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaVaccine = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTGoiTiemChung", x => new { x.MaGoiTiemChung, x.MaVaccine });
                    table.ForeignKey(
                        name: "FK_CTGoiTiemChung_GoiTiemChung_MaGoiTiemChung",
                        column: x => x.MaGoiTiemChung,
                        principalTable: "GoiTiemChung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CTGoiTiemChung_Vaccine_MaVaccine",
                        column: x => x.MaVaccine,
                        principalTable: "Vaccine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThongTinTiemChung",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayTiem = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LanTiem = table.Column<int>(type: "int", nullable: false),
                    GioTiem = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiaDiemTiem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KetQua = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HTTruocTiem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HTSauTiem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaKhachHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaGoiTiemChung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinTiemChung", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongTinTiemChung_GoiTiemChung_MaGoiTiemChung",
                        column: x => x.MaGoiTiemChung,
                        principalTable: "GoiTiemChung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThongTinTiemChung_KhachHang_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietVaccine_MaVaccine",
                table: "ChiTietVaccine",
                column: "MaVaccine");

            migrationBuilder.CreateIndex(
                name: "IX_CTGoiTiemChung_MaVaccine",
                table: "CTGoiTiemChung",
                column: "MaVaccine");

            migrationBuilder.CreateIndex(
                name: "IX_CTNhapVaccine_MaNhapVaccine",
                table: "CTNhapVaccine",
                column: "MaNhapVaccine");

            migrationBuilder.CreateIndex(
                name: "IX_CTXuatVaccine_MaXuatVaccine",
                table: "CTXuatVaccine",
                column: "MaXuatVaccine");

            migrationBuilder.CreateIndex(
                name: "IX_GoiTiemChung_MaLoaiTiemChung",
                table: "GoiTiemChung",
                column: "MaLoaiTiemChung");

            migrationBuilder.CreateIndex(
                name: "IX_LoaiTiemChung_MaNhanVien",
                table: "LoaiTiemChung",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_NhapVaccine_MaNhaCungCap",
                table: "NhapVaccine",
                column: "MaNhaCungCap");

            migrationBuilder.CreateIndex(
                name: "IX_NhapVaccine_MaNhanVien",
                table: "NhapVaccine",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinTiemChung_MaGoiTiemChung",
                table: "ThongTinTiemChung",
                column: "MaGoiTiemChung");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinTiemChung_MaKhachHang",
                table: "ThongTinTiemChung",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_Vaccine_MaLoaiVaccine",
                table: "Vaccine",
                column: "MaLoaiVaccine");

            migrationBuilder.CreateIndex(
                name: "IX_XuatVaccine_MaNhanVien",
                table: "XuatVaccine",
                column: "MaNhanVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietVaccine");

            migrationBuilder.DropTable(
                name: "CTGoiTiemChung");

            migrationBuilder.DropTable(
                name: "CTNhapVaccine");

            migrationBuilder.DropTable(
                name: "CTXuatVaccine");

            migrationBuilder.DropTable(
                name: "ThongTinTiemChung");

            migrationBuilder.DropTable(
                name: "NhapVaccine");

            migrationBuilder.DropTable(
                name: "Vaccine");

            migrationBuilder.DropTable(
                name: "XuatVaccine");

            migrationBuilder.DropTable(
                name: "GoiTiemChung");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "NhaCungCap");

            migrationBuilder.DropTable(
                name: "LoaiVaccine");

            migrationBuilder.DropTable(
                name: "LoaiTiemChung");

            migrationBuilder.DropTable(
                name: "NhanVien");
        }
    }
}
