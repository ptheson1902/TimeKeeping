using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster
{
    public class KinmuModel : PageModel
    {
        private readonly KintaiDbContext _context;
        private readonly ApplicationDbContext context1;
        public List<Display> Data = new List<Display>();
        public string? Kinmu_cd { get; set; }
        public string? Kinmu_nm { get; set; }
        public string? Valid_flg { get; set; }
        public string? Kinmu_bunrui { get; set; }
        public KinmuModel(UNN_Ki_001.Data.KintaiDbContext context, ApplicationDbContext application)
        {
            _context = context;
            context1 = application;
        }
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
            /*var register_action = Request.Form["register_action"];
            switch (register_action)
            {
                case "register":
                    Register();
                    break;
                default: break;
            }
            var update_action = Request.Form["update_action"];
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
            Kinmu_cd = Request.Form["kinmu_cd"];
            Kinmu_nm = Request.Form["kinmu_nm"];
            Kinmu_bunrui = Request.Form["kinmu_bunrui"];
            Valid_flg = Request.Form["valid_flg"];
            var no = from m_kinmu in _context.m_kinmus
                     orderby m_kinmu.KinmuCd
                     select new { m_kinmu.KinmuCd, m_kinmu.KinmuNm, m_kinmu.ValidFlg , m_kinmu.KinmuBunrui ,m_kinmu.KinmuFrTm , m_kinmu.KinmuToTm , m_kinmu.Kyukei1FrTm , m_kinmu.Kyukei1ToTm};
            // ðŒ‚É‚æ‚éŒŸõ‚·‚é‚±‚Æ(valuenull‚ÍŒŸõðŒ‚É‚È‚ç‚È‚¢‚±‚ÆB)
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
                Data.Add(d);
            }

        }
    }
}
