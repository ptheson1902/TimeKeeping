using Microsoft.AspNetCore.Identity;
namespace UNN_Ki_001.Data
{
    public class AppUser : IdentityUser
    {
        public string? Kigyo_cd { get; set; }
        public string? Shain_no { get; set; }
        public string? Name_sei { get; set; }
        public string? Name_mei { get; set; }
        public string? Name_kana_sei { get; set; }
        public string? Name_kana_mei { get; set; }
        public string? Nyusha_dt { get; set; }
        public string? Taishoku_dt { get; set; }
        public string? Shozoku_cd { get; set; }
        public string? Shokushu_cd { get; set; }
        public string? Koyokeitai_cd { get; set; }
        public string? User_kbn { get; set; }
        public string? Valid_flg { get; set; }
    }
    public class AppRole : IdentityRole
    {

    }

}
