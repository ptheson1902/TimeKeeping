using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster
{
    public class KinmuModel : BasePageModel
    {
        public List<M_Kinmu>? _searchList { get; set; }
        public string? _searchListString { get; set; }
        [BindProperty]
        public M_Kinmu? MKinmuSearch { get; set; }
        [BindProperty]
        public M_Kinmu? MKinmuCRUD { get; set; }
        public string? Message { get; set; }
        public string? ValidFlg { get; set; }
        public KinmuModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }
        
        public IActionResult OnGet()
        {
            var shain = GetCurrentUserShainAsync().Result;
            if (shain == null)
                return RedirectToPage("/");
            return Page();
        }                                                     
        public IActionResult OnPost(string action)
        {
            var shain = GetCurrentUserShainAsync().Result;
            Console.WriteLine(action);
            if (shain == null)
                return RedirectToPage("/");
            switch (action)
            {
                case "search":
                    Search(shain);
                    break;
                case "insert":
                    Insert(shain);
                    break;
                case "update":
                    Update(shain);
                    break;
                case "delete":
                    Delete(shain);
                    break;
                default: break;
            }
            return Page();
        }
        //　検索
        private void Search(M_Shain shain)
        {
            _searchList = _kintaiDbContext.m_kinmus
                .Where(e => e.KigyoCd.Equals(shain.KigyoCd))
                .WhereIf(MKinmuSearch!.KinmuCd != null, e => e.KinmuCd!.Equals(MKinmuSearch.KinmuCd))
                .WhereIf(MKinmuSearch.KinmuNm != null, e => e.KinmuNm.Equals(MKinmuSearch.KinmuNm))
                .WhereIf(MKinmuSearch.KinmuBunrui != null, e => e.KinmuBunrui.Equals(MKinmuSearch.KinmuBunrui!))
                .WhereIf(MKinmuSearch.ValidFlg != null, e => e.ValidFlg.Equals(MKinmuSearch.ValidFlg!))
                .OrderBy(e => e.KinmuCd)
                .OrderBy(e => e.KinmuNm)
                .ToList();
            foreach(var item in _searchList)
            {
                item.KinmuFrTm = Cal(item.KinmuFrTm);
                item.KinmuToTm = Cal(item.KinmuToTm);
                item.Kyukei1FrTm = Cal(item.Kyukei1FrTm);
                item.Kyukei1ToTm = Cal(item.Kyukei1ToTm);
                item.Kyukei2FrTm = Cal(item.Kyukei2FrTm);
                item.Kyukei2ToTm = Cal(item.Kyukei2ToTm);
                item.Kyukei3FrTm = Cal(item.Kyukei3FrTm);
                item.Kyukei3ToTm = Cal(item.Kyukei3ToTm);
                if (item.KinmuBunrui != null && item.KinmuBunrui.Equals("01"))
                    item.KinmuBunrui = "通常勤務";
                if (item.KinmuBunrui != null && item.KinmuBunrui.Equals("02"))
                    item.KinmuBunrui = "所定休日";
                if (item.KinmuBunrui != null && item.KinmuBunrui.Equals("03"))
                    item.KinmuBunrui = "法定休日";
                if (item.KinmuBunrui != null && item.KinmuBunrui.Equals("04"))
                    item.KinmuBunrui = "振替休日";
                if (item.KinmuBunrui != null && item.KinmuBunrui.Equals("05"))
                    item.KinmuBunrui = "代休";
                if (item.KinmuBunrui != null && item.KinmuBunrui.Equals("06"))
                    item.KinmuBunrui = "A有給休暇";
                if (item.KinmuBunrui != null && item.KinmuBunrui.Equals("07"))
                    item.KinmuBunrui = "午前半休";
                if (item.KinmuBunrui != null && item.KinmuBunrui.Equals("08"))
                    item.KinmuBunrui = "午後半休";
            }
            ValidFlg = MKinmuSearch.ValidFlg;
            _searchListString = JsonConvert.SerializeObject(_searchList);
        }

        private string Cal(string? time)
        {
            if (time == null)
                return "";
            return time.Substring(0, 2) + ":" + time.Substring(2);
        }

        private void Insert(M_Shain shain)
        {
            string KyukeiAutoFlg = Request.Form["KyukeiAutoFlg"];
            string KinmuFrCtrlFlg = Request.Form["KinmuFrCtrlFlg"];
            MKinmuCRUD.KyukeiAutoFlg = KyukeiAutoFlg;
            MKinmuCRUD.KinmuFrCtrlFlg = KinmuFrCtrlFlg;
            MKinmuCRUD = Required(MKinmuCRUD, shain.KigyoCd);

            try
            {
                _kintaiDbContext.Add(MKinmuCRUD);
                _kintaiDbContext.SaveChanges();
                Message = "勤務マスタの登録が出来ました。";
            }
            catch
            {
                Message = "勤務マスタの登録に失敗しました。";
            }
        }
        private void Update(M_Shain shain)
        {
            string KyukeiAutoFlg = Request.Form["KyukeiAutoFlg"];
            string KinmuFrCtrlFlg = Request.Form["KinmuFrCtrlFlg"];
            MKinmuCRUD.KyukeiAutoFlg = KyukeiAutoFlg;
            MKinmuCRUD.KinmuFrCtrlFlg = KinmuFrCtrlFlg;
            MKinmuCRUD = Required(MKinmuCRUD, shain.KigyoCd);
            try
            {
                _kintaiDbContext.Update(MKinmuCRUD);
                _kintaiDbContext.SaveChanges();
                Message = "勤務マスタの更新が出来ました。";
            }
            catch
            {
                Message = "勤務マスタの更新に失敗しました。";
            }
        }
        private void Delete(M_Shain shain)
        {
            string KyukeiAutoFlg = Request.Form["KyukeiAutoFlg"];
            string KinmuFrCtrlFlg = Request.Form["KinmuFrCtrlFlg"];
            MKinmuCRUD.KyukeiAutoFlg = KyukeiAutoFlg;
            MKinmuCRUD.KinmuFrCtrlFlg = KinmuFrCtrlFlg;
            MKinmuCRUD = Required(MKinmuCRUD, shain.KigyoCd);
            try
            {
                _kintaiDbContext.Remove(MKinmuCRUD);
                _kintaiDbContext.SaveChanges();
                Message = "勤務マスタの削除が出来ました。";
            }
            catch
            {
                Message = "勤務マスタの削除に失敗しました。";
            }
        }
        private M_Kinmu Required(M_Kinmu m_Kinmu, string kigyoCd)
        {
            m_Kinmu.KigyoCd = kigyoCd;
            if (m_Kinmu.KyukeiAutoFlg == null)
                m_Kinmu.KyukeiAutoFlg = "0";
            if (m_Kinmu.KinmuFrCtrlFlg == null)
                m_Kinmu.KinmuFrCtrlFlg = "0";
            if (m_Kinmu.KinmuFrTm != null)
                m_Kinmu.KinmuFrTm = m_Kinmu.KinmuFrTm.Replace(":", "");

            if (m_Kinmu.KinmuToTm != null)
                m_Kinmu.KinmuToTm = m_Kinmu.KinmuToTm.Replace(":", "");

            if (m_Kinmu.Kyukei1FrTm != null)
                m_Kinmu.Kyukei1FrTm = m_Kinmu.Kyukei1FrTm.Replace(":", "");

            if (m_Kinmu.Kyukei1ToTm != null)
                m_Kinmu.Kyukei1ToTm = m_Kinmu.Kyukei1ToTm.Replace(":", "");

            if (m_Kinmu.Kyukei2FrTm != null)
                m_Kinmu.Kyukei2FrTm = m_Kinmu.Kyukei2FrTm.Replace(":", "");

            if (m_Kinmu.Kyukei2ToTm != null)
                m_Kinmu.Kyukei2ToTm = m_Kinmu.Kyukei2ToTm.Replace(":", "");

            if (m_Kinmu.Kyukei3FrTm != null)
                m_Kinmu.Kyukei3FrTm = m_Kinmu.Kyukei3FrTm.Replace(":", "");

            if (m_Kinmu.Kyukei3ToTm != null)
                m_Kinmu.Kyukei3ToTm = m_Kinmu.Kyukei3ToTm.Replace(":", "");

            return m_Kinmu;
        }
    }
}
