using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UNN_Ki_001.Migrations
{
    public partial class initModels : Migration
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
                    kinmu_fr_kbn = table.Column<string>(type: "text", nullable: true),
                    kinmu_fr_tm = table.Column<string>(type: "text", nullable: true),
                    kinmu_to_kbn = table.Column<string>(type: "text", nullable: true),
                    kinmu_to_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei1_fr_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei1_fr_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei1_to_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei1_to_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei2_fr_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei2_fr_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei2_to_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei2_to_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei3_fr_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei3_fr_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei3_to_kbn = table.Column<string>(type: "text", nullable: true),
                    kyukei3_to_tm = table.Column<string>(type: "text", nullable: true),
                    kyukei_auto_flg = table.Column<string>(type: "text", nullable: true),
                    shotei_tm = table.Column<string>(type: "text", nullable: true),
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
                    update_dt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    update_usr = table.Column<string>(type: "text", nullable: true),
                    update_pgm = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_kinmu", x => new { x.kigyo_cd, x.kinmu_cd });
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
                    dakoku_fr_kbn = table.Column<string>(type: "text", nullable: true),
                    dakoku_fr_dt = table.Column<string>(type: "text", nullable: true),
                    dakoku_fr_tm = table.Column<string>(type: "text", nullable: true),
                    dakoku_to_kbn = table.Column<string>(type: "text", nullable: true),
                    dakoku_to_dt = table.Column<string>(type: "text", nullable: true),
                    dakoku_to_tm = table.Column<string>(type: "text", nullable: true),
                    kinmu_fr_kbn = table.Column<string>(type: "text", nullable: true),
                    kinmu_fr_dt = table.Column<string>(type: "text", nullable: true),
                    kinmu_fr_tm = table.Column<string>(type: "text", nullable: true),
                    kinmu_to_kbn = table.Column<string>(type: "text", nullable: true),
                    kinmu_to_dt = table.Column<string>(type: "text", nullable: true),
                    kinmu_to_tm = table.Column<string>(type: "text", nullable: true),
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
                    update_dt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    update_usr = table.Column<string>(type: "text", nullable: true),
                    update_pgm = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_kinmu", x => new { x.kigyo_cd, x.shain_no, x.kinmu_dt });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_kinmu",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_kinmu",
                schema: "public");
        }
    }
}
