using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster
{
    public class KinmuModel : BasePageModel
    {
        public List<Display> Data = new List<Display>();

        public List<M_Kinmu> _targetList = new();

        public KinmuModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public List<M_Kinmu> Kinmu { get; set; }
        [BindProperty]
        public string? KinmuCd { get; set; }
        [BindProperty]
        public string? KinmuNm { get; set; }
        [BindProperty]
        public string? ValidFlg { get; set; }
        [BindProperty]
        public string? KinmuBunrui { get; set; }
        public string? Message { get; set; }
        
        public void OnGet()
        {
        }                                                     
        public void OnPost()
        {

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
        //@ŒŸõ
        private void Search()
        {
            _targetList = _kintaiDbContext.m_kinmus
                .WhereIf(KinmuNm != null, e => e.KinmuNm!.Equals(KinmuNm!))
                .WhereIf(KinmuCd != null, e => e.KinmuCd!.Equals(KinmuCd!))
                .WhereIf(KinmuBunrui != null, e => e.KinmuBunrui!.Equals(KinmuBunrui!))
                .WhereIf(ValidFlg != null, e => e.ValidFlg!.Equals(ValidFlg!))
                .ToList();
        }
        private void Register()
        {
            string kigyocd = "C001";
            string kinmu_cd = Request.Form["kinmu_cd"];
            string kinmu_nm = Request.Form["kinmu_nm"];
            string kinmu_bunrui = Request.Form["kinmu_bunrui"];
            string kinmu_fr_kbn = Request.Form["kinmu_fr_kbn"];
            string kinmu_to_kbn = Request.Form["kinmu_to_kbn"];
            string kinmu_fr_tm = Request.Form["kinmu_fr_tm"];
            string kinmu_to_tm = Request.Form["kinmu_to_tm"];
            string kyukei1_fr_kbn = Request.Form["kyukei1_fr_kbn"];
            string kyukei1_to_kbn = Request.Form["kyukei1_to_kbn"];
            string kyukei2_fr_kbn = Request.Form["kyukei2_fr_kbn"];
            string kyukei2_to_kbn = Request.Form["kyukei2_to_kbn"];
            string kyukei3_fr_kbn = Request.Form["kyukei3_fr_kbn"];
            string kyukei3_to_kbn = Request.Form["kyukei3_to_kbn"];
            string kyuke1_fr_tm = Request.Form["kyukei1_fr_tm"];
            string kyuke1_to_tm = Request.Form["kyukei1_to_tm"];
            string kyuke2_fr_tm = Request.Form["kyukei2_fr_tm"];
            string kyuke2_to_tm = Request.Form["kyukei2_to_tm"];
            string kyuke3_fr_tm = Request.Form["kyukei3_fr_tm"];
            string kyuke3_to_tm = Request.Form["kyukei3_to_tm"];
            string kyukei_auto_flg = Request.Form["kyukei_auto_flg1"];
            string kinmu_fr_ctrl_flg = Request.Form["kinmu_fr_ctrl_flg1"];
            string kinmu_fr_marume_kbn = Request.Form["kinmu_fr_marume_kbn"];
            string kinmu_to_marume_kbn = Request.Form["kinmu_to_marume_kbn"];
            string kinmu_fr_marume_tm = Request.Form["kinmu_fr_marume_tm"];
            string kinmu_to_marume_tm = Request.Form["kinmu_to_marume_tm"];
            string kyukei_fr_marume_kbn = Request.Form["kyukei_fr_marume_kbn"];
            string kyukei_to_marume_kbn = Request.Form["kyukei_to_marume_kbn"];
            string kyukei_fr_marume_tm = Request.Form["kyukei_fr_marume_tm"];
            string kyukei_to_marume_tm = Request.Form["kyukei_to_marume_tm"];
            string valid_flg = Request.Form["valid_flg1"];

            M_Kinmu mk = new M_Kinmu( kigyocd , kinmu_cd, kinmu_nm , kinmu_bunrui, kinmu_fr_tm.Replace(":",""),valid_flg);
            _kintaiDbContext.m_kinmus.Add(mk);
            var a = _kintaiDbContext.SaveChanges();
            if (a < 0)
            {
                Message = "“o˜^‚Å‚«‚Ü‚¹‚ñ‚Å‚µ‚½B";
            }
            else
            {
                Message = "“o˜^‚Å‚«‚Ü‚µ‚½B";
            }
        }

    }
}
