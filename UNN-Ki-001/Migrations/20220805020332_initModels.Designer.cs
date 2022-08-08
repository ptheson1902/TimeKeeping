﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UNN_Ki_001.Data;

#nullable disable

namespace UNN_Ki_001.Migrations
{
    [DbContext(typeof(KintaiDbContext))]
    [Migration("20220805020332_initModels")]
    partial class initModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("UNN_Ki_001.Data.Models.M_Kinmu", b =>
                {
                    b.Property<string>("KigyoCd")
                        .HasColumnType("text")
                        .HasColumnName("kigyo_cd");

                    b.Property<string>("KinmuCd")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_cd");

                    b.Property<string>("CreatePgm")
                        .HasColumnType("text")
                        .HasColumnName("create_pgm");

                    b.Property<string>("CreateUsr")
                        .HasColumnType("text")
                        .HasColumnName("create_usr");

                    b.Property<string>("KinmuBunrui")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_bunrui");

                    b.Property<string>("KinmuFrCtrlFlg")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_fr_ctrl_flg");

                    b.Property<string>("KinmuFrKbn")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_fr_kbn");

                    b.Property<string>("KinmuFrMarumeKbn")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_fr_marume_kbn");

                    b.Property<int?>("KinmuFrMarumeTm")
                        .HasColumnType("integer")
                        .HasColumnName("kinmu_fr_marume_tm");

                    b.Property<string>("KinmuFrTm")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_fr_tm");

                    b.Property<string>("KinmuNm")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_nm");

                    b.Property<string>("KinmuToKbn")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_to_kbn");

                    b.Property<string>("KinmuToMarumeKbn")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_to_marume_kbn");

                    b.Property<int?>("KinmuToMarumeTm")
                        .HasColumnType("integer")
                        .HasColumnName("kinmu_to_marume_tm");

                    b.Property<string>("KinmuToTm")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_to_tm");

                    b.Property<string>("Kyukei1FrKbn")
                        .HasColumnType("text")
                        .HasColumnName("kyukei1_fr_kbn");

                    b.Property<string>("Kyukei1FrTm")
                        .HasColumnType("text")
                        .HasColumnName("kyukei1_fr_tm");

                    b.Property<string>("Kyukei1ToKbn")
                        .HasColumnType("text")
                        .HasColumnName("kyukei1_to_kbn");

                    b.Property<string>("Kyukei1ToTm")
                        .HasColumnType("text")
                        .HasColumnName("kyukei1_to_tm");

                    b.Property<string>("Kyukei2FrKbn")
                        .HasColumnType("text")
                        .HasColumnName("kyukei2_fr_kbn");

                    b.Property<string>("Kyukei2FrTm")
                        .HasColumnType("text")
                        .HasColumnName("kyukei2_fr_tm");

                    b.Property<string>("Kyukei2ToKbn")
                        .HasColumnType("text")
                        .HasColumnName("kyukei2_to_kbn");

                    b.Property<string>("Kyukei2ToTm")
                        .HasColumnType("text")
                        .HasColumnName("kyukei2_to_tm");

                    b.Property<string>("Kyukei3FrKbn")
                        .HasColumnType("text")
                        .HasColumnName("kyukei3_fr_kbn");

                    b.Property<string>("Kyukei3FrTm")
                        .HasColumnType("text")
                        .HasColumnName("kyukei3_fr_tm");

                    b.Property<string>("Kyukei3ToKbn")
                        .HasColumnType("text")
                        .HasColumnName("kyukei3_to_kbn");

                    b.Property<string>("Kyukei3ToTm")
                        .HasColumnType("text")
                        .HasColumnName("kyukei3_to_tm");

                    b.Property<string>("KyukeiAutoFlg")
                        .HasColumnType("text")
                        .HasColumnName("kyukei_auto_flg");

                    b.Property<string>("KyukeiFrMarumeKbn")
                        .HasColumnType("text")
                        .HasColumnName("kyukei_fr_marume_kbn");

                    b.Property<int?>("KyukeiFrMarumeTm")
                        .HasColumnType("integer")
                        .HasColumnName("kyukei_fr_marume_tm");

                    b.Property<string>("KyukeiToMarumeKbn")
                        .HasColumnType("text")
                        .HasColumnName("kyukei_to_marume_kbn");

                    b.Property<int?>("KyukeiToMarumeTm")
                        .HasColumnType("integer")
                        .HasColumnName("kyukei_to_marume_tm");

                    b.Property<string>("ShoteiTm")
                        .HasColumnType("text")
                        .HasColumnName("shotei_tm");

                    b.Property<DateTime?>("UpdateDt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("update_dt");

                    b.Property<string>("UpdatePgm")
                        .HasColumnType("text")
                        .HasColumnName("update_pgm");

                    b.Property<string>("UpdateUsr")
                        .HasColumnType("text")
                        .HasColumnName("update_usr");

                    b.Property<string>("ValidFlg")
                        .HasColumnType("text")
                        .HasColumnName("valid_flg");

                    b.HasKey("KigyoCd", "KinmuCd");

                    b.ToTable("m_kinmu", "public");
                });

            modelBuilder.Entity("UNN_Ki_001.Data.Models.T_Kinmu", b =>
                {
                    b.Property<string>("KigyoCd")
                        .HasColumnType("text")
                        .HasColumnName("kigyo_cd");

                    b.Property<string>("ShainNo")
                        .HasColumnType("text")
                        .HasColumnName("shain_no");

                    b.Property<string>("KinmuDt")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_dt");

                    b.Property<string>("Biko")
                        .HasColumnType("text")
                        .HasColumnName("biko");

                    b.Property<string>("CreatePgm")
                        .HasColumnType("text")
                        .HasColumnName("create_pgm");

                    b.Property<string>("CreateUsr")
                        .HasColumnType("text")
                        .HasColumnName("create_usr");

                    b.Property<string>("DakokuFrDt")
                        .HasColumnType("text")
                        .HasColumnName("dakoku_fr_dt");

                    b.Property<string>("DakokuFrKbn")
                        .HasColumnType("text")
                        .HasColumnName("dakoku_fr_kbn");

                    b.Property<string>("DakokuFrTm")
                        .HasColumnType("text")
                        .HasColumnName("dakoku_fr_tm");

                    b.Property<string>("DakokuToDt")
                        .HasColumnType("text")
                        .HasColumnName("dakoku_to_dt");

                    b.Property<string>("DakokuToKbn")
                        .HasColumnType("text")
                        .HasColumnName("dakoku_to_kbn");

                    b.Property<string>("DakokuToTm")
                        .HasColumnType("text")
                        .HasColumnName("dakoku_to_tm");

                    b.Property<int?>("Hoteigai")
                        .HasColumnType("integer")
                        .HasColumnName("hoteigai");

                    b.Property<int?>("Hoteikyu")
                        .HasColumnType("integer")
                        .HasColumnName("hoteikyu");

                    b.Property<int?>("Hoteinai")
                        .HasColumnType("integer")
                        .HasColumnName("hoteinai");

                    b.Property<string>("KinmuCd")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_cd");

                    b.Property<string>("KinmuFrDt")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_fr_dt");

                    b.Property<string>("KinmuFrKbn")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_fr_kbn");

                    b.Property<string>("KinmuFrTm")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_fr_tm");

                    b.Property<string>("KinmuToDt")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_to_dt");

                    b.Property<string>("KinmuToKbn")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_to_kbn");

                    b.Property<string>("KinmuToTm")
                        .HasColumnType("text")
                        .HasColumnName("kinmu_to_tm");

                    b.Property<int?>("Kojo")
                        .HasColumnType("integer")
                        .HasColumnName("kojo");

                    b.Property<int?>("Kyukei")
                        .HasColumnType("integer")
                        .HasColumnName("kyukei");

                    b.Property<int?>("Shinya")
                        .HasColumnType("integer")
                        .HasColumnName("shinya");

                    b.Property<int?>("Shotei")
                        .HasColumnType("integer")
                        .HasColumnName("shotei");

                    b.Property<int?>("Sorodo")
                        .HasColumnType("integer")
                        .HasColumnName("sorodo");

                    b.Property<DateTime?>("UpdateDt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("update_dt");

                    b.Property<string>("UpdatePgm")
                        .HasColumnType("text")
                        .HasColumnName("update_pgm");

                    b.Property<string>("UpdateUsr")
                        .HasColumnType("text")
                        .HasColumnName("update_usr");

                    b.HasKey("KigyoCd", "ShainNo", "KinmuDt");

                    b.ToTable("t_kinmu", "public");
                });
#pragma warning restore 612, 618
        }
    }
}
