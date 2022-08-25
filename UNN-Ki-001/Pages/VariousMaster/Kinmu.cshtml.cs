using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster
{
    public class KinmuModel : BasePageModel
    {
        private readonly KintaiDbContext _context;
        private readonly ApplicationDbContext context1;
        public List<Display> Data = new List<Display>();

        public KinmuModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public List<M_Kinmu> Kinmu { get; set; }
        public string? Kinmu_cd { get; set; }
        public string? Kinmu_nm { get; set; }
        public string? Valid_flg { get; set; }
        public string? Kinmu_bunrui { get; set; }
        public string? Message { get; set; }
        public void OnGet()
        {
            var shain = GetCurrentUserShainAsync().Result;
            Kinmu = (from e in _kintaiDbContext.m_kinmus
                     where e.KigyoCd!.Equals(shain!.KigyoCd)
                     orderby e.KinmuBunrui                   
                     select e).ToList();
            
            

        }                                                     
        public void OnPost()
        {
            var shain = GetCurrentUserShainAsync().Result;
            Kinmu = 
                (from e in _kintaiDbContext.m_kinmus
                     where e.KigyoCd!.Equals(shain!.KigyoCd)
                     orderby e.KinmuBunrui                   
                     select e).ToList();


            var action = Request.Form["action"];
            switch (action)
            {
                case "search":
                    Search();
                    break;
                default: break;
            }
            var register_action = Request.Form["register_action"];
            switch (register_action)
            {
                case "register":
                    Register();
                    break;
                default: break;
            }
            /*var update_action = Request.Form["update_action"];
            switch (update_action)
            {
                case "update":
                    Update();
                    break;
                default: break;
            }
            var delete_action = Request.Form["delete_action"];
            switch (delete_action)
            {
                case "delete":
                    Delete();
                    break;
                default: break;
            }*/
        }
        //　検索
        private void Search()
        {
            Kinmu_cd = Request.Form["kinmu_cd"];
            Kinmu_nm = Request.Form["kinmu_nm"];
            Kinmu_bunrui = Request.Form["kinmu_bunrui"];
            Valid_flg = Request.Form["valid_flg"];
            var no = from m_kinmu in _kintaiDbContext.m_kinmus
                     orderby m_kinmu.KinmuCd
                     select new { m_kinmu.KinmuCd, m_kinmu.KinmuNm, m_kinmu.ValidFlg , m_kinmu.KinmuBunrui ,m_kinmu.KinmuFrTm , m_kinmu.KinmuToTm , m_kinmu.Kyukei1FrTm , m_kinmu.Kyukei1ToTm};
            // 条件による検索すること(value＝nullは検索条件にならないこと。)
            if (!string.IsNullOrEmpty(Kinmu_cd))
            {
                no = no.Where(e => e.KinmuCd.Equals(Kinmu_cd));
            }

            if (!string.IsNullOrEmpty(Kinmu_nm))
            {
                no = no.Where(e => e.KinmuNm.Equals(Kinmu_nm));
            }
            if (!string.IsNullOrEmpty(Kinmu_bunrui))
            {
                no = no.Where(e => e.KinmuBunrui.Equals(Kinmu_bunrui));
            }

            if (!string.IsNullOrEmpty(Valid_flg))
            {
                no = no.Where(e => e.ValidFlg.Equals(Valid_flg));
            }

            foreach (var item in no)
            {

                Display d = new Display();
                d.kinmu_cd = item.KinmuCd;
                d.kinmu_nm = item.KinmuNm;
                d.kinmu_bunrui = item.KinmuBunrui;
                d.valid_flg = item.ValidFlg;
                d.kinmu_fr_tm = item.KinmuFrTm;
                d.kinmu_to_tm = item.KinmuToTm;
                d.kyuke1_fr_tm = item.Kyukei1FrTm;
                d.kyuke1_to_tm = item.Kyukei1ToTm;
                Data.Add(d);
            }

        }
        private void Register()
        {
            string kigyocd1 = "C001";
            string kinmu_cd1 = Request.Form["kinmu_cd1"];
            string kinmu_nm1 = Request.Form["kinmu_nm1"];
            string kinmu_bunrui1 = Request.Form["kinmu_bunrui1"];
            string kinmu_fr_tm1 = Request.Form["kinmu_fr_tm1"];
            string kinmu_to_tm1 = Request.Form["kinmu_to_tm1"];
            string kyuke1_fr_tm1 = Request.Form["kyuke1_fr_tm1"];
            string kyuke1_to_tm1 = Request.Form["kyuke1_to_tm1"];
            string kyukei_auto_flg1 = Request.Form["kyukei_auto_flg1"];
            string kinmu_fr_ctrl_flg1 = Request.Form["kinmu_fr_ctrl_flg1"];

            M_Kinmu mk = new M_Kinmu( kigyocd1 , kinmu_cd1, kinmu_nm1 , kinmu_bunrui1);
            _context.m_kinmus.Add(mk);
            var a = _context.SaveChanges();
            if (a < 0)
            {
                Message = "登録できませんでした。";
            }
            else
            {
                Message = "登録できました。";
            }
        }

    }
}
