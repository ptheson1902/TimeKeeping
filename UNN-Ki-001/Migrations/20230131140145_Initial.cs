using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UNN_Ki_001.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "m_kinmu",
                schema: "public",
                columns: table => new
                {
                    kigyo_cd = table.Column<string>(type: "text", nullable: false),
                    kinmu_cd = table.Column<string>(type: "text", nullable: false),
                    kinmu_nm = table.Column<string>(type: "text", nullable: true),
                    kinmu_bunrui = table.Column<string>(type: "text", nullable: true),
                    kinmu_fr_tm = table.Column<string>(type: "text", nullable: true),
                    kinmu_fr_kbn = table.Column<string>(type: "text", nullable: true),
                    kinmu_to_tm = table.Column<string>(type: "text", nullable: true),
                    kinmu_to_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei1_fr_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei1_fr_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei1_to_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei1_to_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei2_fr_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei2_fr_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei2_to_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei2_to_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei3_fr_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei3_fr_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei3_to_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei3_to_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei_auto_flg = table.Column<string>(type: "text", nullable: true),
                    shotei_tm = table.Column<int>(type: "integer", nullable: true),
                    kinmu_fr_ctrl_flg = table.Column<string>(type: "text", nullable: true),
                    kinmu_fr_marume_tm = table.Column<int>(type: "integer", nullable: true),
                    kinmu_fr_marume_kbn = table.Column<string>(type: "text", nullable: true),
                    kinmu_to_marume_tm = table.Column<int>(type: "integer", nullable: true),
                    kinmu_to_marume_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei_fr_marume_tm = table.Column<int>(type: "integer", nullable: true),
                    kyukei_fr_marume_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei_to_marume_tm = table.Column<int>(type: "integer", nullable: true),
                    kyukei_to_marume_kbn = table.Column<string>(type: "text", nullable: true),
                    valid_flg = table.Column<string>(type: "text", nullable: true),
                    create_usr = table.Column<string>(type: "text", nullable: true),
                    create_pgm = table.Column<string>(type: "text", nullable: true),
                    update_dt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_usr = table.Column<string>(type: "text", nullable: true),
                    update_pgm = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_kinmu", x => new { x.kigyo_cd, x.kinmu_cd });
                });

            migrationBuilder.CreateTable(
                name: "m_koyokeitai",
                schema: "public",
                columns: table => new
                {
                    kigyo_cd = table.Column<string>(type: "text", nullable: false),
                    koyokeitai_cd = table.Column<string>(type: "text", nullable: false),
                    koyokeitai_nm = table.Column<string>(type: "text", nullable: true),
                    valid_flg = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_koyokeitai", x => new { x.kigyo_cd, x.koyokeitai_cd });
                });

            migrationBuilder.CreateTable(
                name: "m_settings",
                schema: "public",
                columns: table => new
                {
                    kigyo_cd = table.Column<string>(type: "text", nullable: false),
                    shime_dt = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_settings", x => x.kigyo_cd);
                });

            migrationBuilder.CreateTable(
                name: "m_shokushu",
                schema: "public",
                columns: table => new
                {
                    kigyo_cd = table.Column<string>(type: "text", nullable: false),
                    shokushu_cd = table.Column<string>(type: "text", nullable: false),
                    shokushu_nm = table.Column<string>(type: "text", nullable: true),
                    valid_flg = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_shokushu", x => new { x.kigyo_cd, x.shokushu_cd });
                });

            migrationBuilder.CreateTable(
                name: "m_shozoku",
                schema: "public",
                columns: table => new
                {
                    kigyo_cd = table.Column<string>(type: "text", nullable: false),
                    shozoku_cd = table.Column<string>(type: "text", nullable: false),
                    shozoku_nm = table.Column<string>(type: "text", nullable: true),
                    valid_flg = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_shozoku", x => new { x.kigyo_cd, x.shozoku_cd });
                });

            migrationBuilder.CreateTable(
                name: "t_kinmu",
                schema: "public",
                columns: table => new
                {
                    kigyo_cd = table.Column<string>(type: "text", nullable: false),
                    shain_no = table.Column<string>(type: "text", nullable: false),
                    kinmu_dt = table.Column<string>(type: "text", nullable: false),
                    kinmu_cd = table.Column<string>(type: "text", nullable: true),
                    dakoku_fr_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    dakoku_to_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    kinmu_fr_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    kinmu_to_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    shotei = table.Column<int>(type: "integer", nullable: true),
                    sorodo = table.Column<int>(type: "integer", nullable: true),
                    kojo = table.Column<int>(type: "integer", nullable: true),
                    kyukei = table.Column<int>(type: "integer", nullable: true),
                    hoteinai = table.Column<int>(type: "integer", nullable: true),
                    hoteigai = table.Column<int>(type: "integer", nullable: true),
                    shinya = table.Column<int>(type: "integer", nullable: true),
                    hoteikyu = table.Column<int>(type: "integer", nullable: true),
                    biko = table.Column<string>(type: "text", nullable: true),
                    create_usr = table.Column<string>(type: "text", nullable: true),
                    create_pgm = table.Column<string>(type: "text", nullable: true),
                    update_dt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_usr = table.Column<string>(type: "text", nullable: true),
                    update_pgm = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_kinmu", x => new { x.kigyo_cd, x.shain_no, x.kinmu_dt });
                    table.ForeignKey(
                        name: "FK_t_kinmu_m_kinmu_kigyo_cd_kinmu_cd",
                        columns: x => new { x.kigyo_cd, x.kinmu_cd },
                        principalSchema: "public",
                        principalTable: "m_kinmu",
                        principalColumns: new[] { "kigyo_cd", "kinmu_cd" });
                });

            migrationBuilder.CreateTable(
                name: "m_shain",
                schema: "public",
                columns: table => new
                {
                    kigyo_cd = table.Column<string>(type: "text", nullable: false),
                    shain_no = table.Column<string>(type: "text", nullable: false),
                    name_sei = table.Column<string>(type: "text", nullable: true),
                    name_mei = table.Column<string>(type: "text", nullable: true),
                    name_kana_sei = table.Column<string>(type: "text", nullable: true),
                    name_kana_mei = table.Column<string>(type: "text", nullable: true),
                    nyusha_dt = table.Column<string>(type: "text", nullable: true),
                    taishoku_dt = table.Column<string>(type: "text", nullable: true),
                    shozoku_cd = table.Column<string>(type: "text", nullable: true),
                    shokushu_cd = table.Column<string>(type: "text", nullable: true),
                    koyokeitai_cd = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_shain", x => new { x.kigyo_cd, x.shain_no });
                    table.ForeignKey(
                        name: "FK_m_shain_m_koyokeitai_kigyo_cd_koyokeitai_cd",
                        columns: x => new { x.kigyo_cd, x.koyokeitai_cd },
                        principalSchema: "public",
                        principalTable: "m_koyokeitai",
                        principalColumns: new[] { "kigyo_cd", "koyokeitai_cd" });
                    table.ForeignKey(
                        name: "FK_m_shain_m_shokushu_kigyo_cd_shokushu_cd",
                        columns: x => new { x.kigyo_cd, x.shokushu_cd },
                        principalSchema: "public",
                        principalTable: "m_shokushu",
                        principalColumns: new[] { "kigyo_cd", "shokushu_cd" });
                    table.ForeignKey(
                        name: "FK_m_shain_m_shozoku_kigyo_cd_shozoku_cd",
                        columns: x => new { x.kigyo_cd, x.shozoku_cd },
                        principalSchema: "public",
                        principalTable: "m_shozoku",
                        principalColumns: new[] { "kigyo_cd", "shozoku_cd" });
                });

            migrationBuilder.CreateTable(
                name: "t_kyukei",
                schema: "public",
                columns: table => new
                {
                    kigyo_cd = table.Column<string>(type: "text", nullable: false),
                    shain_no = table.Column<string>(type: "text", nullable: false),
                    kinmu_dt = table.Column<string>(type: "text", nullable: false),
                    seq_no = table.Column<int>(type: "integer", nullable: false),
                    dakoku_fr_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    dakoku_to_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    kyukei = table.Column<int>(type: "integer", nullable: true),
                    create_usr = table.Column<string>(type: "text", nullable: true),
                    create_pgm = table.Column<string>(type: "text", nullable: true),
                    update_dt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_usr = table.Column<string>(type: "text", nullable: true),
                    update_pgm = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_kyukei", x => new { x.kigyo_cd, x.shain_no, x.kinmu_dt, x.seq_no });
                    table.ForeignKey(
                        name: "FK_t_kyukei_t_kinmu_kigyo_cd_shain_no_kinmu_dt",
                        columns: x => new { x.kigyo_cd, x.shain_no, x.kinmu_dt },
                        principalSchema: "public",
                        principalTable: "t_kinmu",
                        principalColumns: new[] { "kigyo_cd", "shain_no", "kinmu_dt" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_m_shain_kigyo_cd_koyokeitai_cd",
                schema: "public",
                table: "m_shain",
                columns: new[] { "kigyo_cd", "koyokeitai_cd" });

            migrationBuilder.CreateIndex(
                name: "IX_m_shain_kigyo_cd_shokushu_cd",
                schema: "public",
                table: "m_shain",
                columns: new[] { "kigyo_cd", "shokushu_cd" });

            migrationBuilder.CreateIndex(
                name: "IX_m_shain_kigyo_cd_shozoku_cd",
                schema: "public",
                table: "m_shain",
                columns: new[] { "kigyo_cd", "shozoku_cd" });

            migrationBuilder.CreateIndex(
                name: "IX_t_kinmu_kigyo_cd_kinmu_cd",
                schema: "public",
                table: "t_kinmu",
                columns: new[] { "kigyo_cd", "kinmu_cd" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_settings",
                schema: "public");

            migrationBuilder.DropTable(
                name: "m_shain",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_kyukei",
                schema: "public");

            migrationBuilder.DropTable(
                name: "m_koyokeitai",
                schema: "public");

            migrationBuilder.DropTable(
                name: "m_shokushu",
                schema: "public");

            migrationBuilder.DropTable(
                name: "m_shozoku",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_kinmu",
                schema: "public");

            migrationBuilder.DropTable(
                name: "m_kinmu",
                schema: "public");
        }
    }
}
