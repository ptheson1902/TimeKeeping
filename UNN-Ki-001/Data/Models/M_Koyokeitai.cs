﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_koyokeitai", Schema = "public")]
    public class M_Koyokeitai
    {
        public M_Koyokeitai()
        {

        }
        public M_Koyokeitai(string? koyokeitai_cd, string? koyokeitai_nm, string? valid_flg , string? kigyo_cd)
        {
            KoyokeitaiCd = koyokeitai_cd;
            KoyokeitaiNm = koyokeitai_nm;
            ValidFlg = valid_flg;
            KigyoCd = kigyo_cd;
        }

        [Key]
        [Column("koyokeitai_cd")]
        public string? KoyokeitaiCd { get; set; }
        [Column("koyokeitai_nm")]
        public string? KoyokeitaiNm { get; set; }
        [Column("valid_flg")]
        public string? ValidFlg { get; set; }
        [Column("kigyo_cd")]
        public string? KigyoCd { get; set; }


    }
}
