using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster
{
    public class KinmuModel : BasePageModel
    {
        public List<M_Kinmu> _targetList { get; set; }
        [BindProperty]
        public M_Kinmu MKinmu { get; set; }
        [BindProperty]
        public M_Kinmu MKinmu1 { get; set; }

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
            var shain = GetCurrentUserShainAsync().Result;
            var action = Request.Form["action"];
            switch (action)
            {
                case "search":
                    Search(shain);
                    break;
                default: break;
            }
            var register_action = Request.Form["register_action"];
            switch (register_action)
            {
                case "register":
                    Register(shain);
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
        private void Search(M_Shain? shain)
        {
            _targetList = _kintaiDbContext.m_kinmus
                .Where(e => e.KigyoCd.Equals(shain.KigyoCd))
                .WhereIf(MKinmu.KinmuCd != null, e => e.KinmuCd!.Equals(MKinmu.KinmuCd))
                .WhereIf(MKinmu.KinmuNm != null, e => e.KinmuNm!.Equals(MKinmu.KinmuNm))
                .WhereIf(MKinmu.KinmuBunrui != null, e => e.KinmuBunrui!.Equals(MKinmu.KinmuBunrui!))
                .WhereIf(MKinmu.ValidFlg != null, e => e.ValidFlg!.Equals(MKinmu.ValidFlg!))
                .ToList();
        }
        private void Register(M_Shain? shain)
        {
            MKinmu1.KigyoCd = shain.KigyoCd;

            string a = Request.Form["KyukeiAuto"];
            if (a == null)
                a = "0";
            string b = Request.Form["KinmuAuto"];
            if (b == null)
                b = "0";
            /*if(MKinmu1.KinmuFrCtrlFlg != null && MKinmu1.KinmuFrCtrlFlg.Equals("false"))
                MKinmu1.KinmuFrCtrlFlg = "0";
            else 
                MKinmu1.KinmuFrCtrlFlg = "1";

            if (MKinmu1.KyukeiAutoFlg != null && MKinmu1.KyukeiAutoFlg.Equals("false"))
            else
                MKinmu1.KyukeiAutoFlg = "1";*/

            MKinmu1.KyukeiAutoFlg = a;
            MKinmu1.KinmuFrCtrlFlg = b;

            MKinmu1.KinmuFrTm = MKinmu1.KinmuFrTm.Replace(":", "");
            MKinmu1.KinmuToTm = MKinmu1.KinmuToTm.Replace(":", "");
            MKinmu1.Kyukei1FrTm = MKinmu1.Kyukei1FrTm.Replace(":", "");
            MKinmu1.Kyukei1ToTm = MKinmu1.Kyukei1ToTm.Replace(":", "");
            MKinmu1.Kyukei2FrTm = MKinmu1.Kyukei2FrTm.Replace(":", "");
            MKinmu1.Kyukei2ToTm = MKinmu1.Kyukei2ToTm.Replace(":", "");
            MKinmu1.Kyukei3FrTm = MKinmu1.Kyukei3FrTm.Replace(":", "");
            MKinmu1.Kyukei3ToTm = MKinmu1.Kyukei3ToTm.Replace(":", "");
            _kintaiDbContext.Add(MKinmu1);
            _kintaiDbContext.SaveChanges();
        }
    }
}
