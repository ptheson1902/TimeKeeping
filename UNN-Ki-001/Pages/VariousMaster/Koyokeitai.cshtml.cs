using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster
{
    public class KoyokeitaiModel : PageModel
    {
        private readonly KintaiDbContext _context;
        private readonly ApplicationDbContext context1;
        public List<Display> Data = new List<Display>();
        public Display? Data1;
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Koyokeitai_cd { get; set; }
        public string? Koyokeitai_nm { get; set; }
        public string? Valid_flg { get; set; }


        public KoyokeitaiModel(UNN_Ki_001.Data.KintaiDbContext context, ApplicationDbContext application)
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
            var register_action = Request.Form["register_action"];
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
            }
        }
        //　検索
        private void Search()
        {
            Koyokeitai_cd = Request.Form["koyokeitai_cd"];
            Koyokeitai_nm = Request.Form["koyokeitai_nm"];
            Valid_flg = Request.Form["valid_flg"];
            var no = from m_koyokeitai in _context.m_koyokeitais
                     orderby m_koyokeitai.KoyokeitaiCd
                     select new { m_koyokeitai.KoyokeitaiCd, m_koyokeitai.KoyokeitaiNm, m_koyokeitai.ValidFlg };
            // 条件による検索すること(value＝nullは検索条件にならないこと。)
            if (!string.IsNullOrEmpty(Koyokeitai_cd))
            {
                no = no.Where(e => e.KoyokeitaiCd.Equals(Koyokeitai_cd));
            }

            if (!string.IsNullOrEmpty(Koyokeitai_nm))
            {
                no = no.Where(e => e.KoyokeitaiNm.Equals(Koyokeitai_nm));
            }

            if (!string.IsNullOrEmpty(Valid_flg))
            {
                no = no.Where(e => e.ValidFlg.Equals(Valid_flg));
            }

            foreach (var item in no)
            {

                Display d = new Display();
                d.koyokeitai_cd = item.KoyokeitaiCd;
                d.koyokeitai_nm = item.KoyokeitaiNm;
                d.valid_flg = item.ValidFlg;
                Data.Add(d);
            }

        }
        // 新規
        private void Register()
        {
            string koyokeitai_cd1 = Request.Form["koyokeitai_cd1"];
            string koyokeitai_nm1 = Request.Form["koyokeitai_nm1"];
            string valid_flg1 = Request.Form["valid_flg1"];
            string kigyo_cd1 = "C001";
            if (koyokeitai_cd1 == "")
            {
                ErrorMessage += "雇用形態コードを入力してください。";
            }
            if (koyokeitai_nm1 == "")
            {
                ErrorMessage += "雇用形態名を入力してください。";
            }
            if (valid_flg1 == null)
            {
                ErrorMessage += "有効/無効をチェックしてください。";
            }
            if (valid_flg1 != null && koyokeitai_nm1 != "" && koyokeitai_cd1 != "")
            {
                M_Koyokeitai kykt = new(koyokeitai_cd1, koyokeitai_nm1, valid_flg1, kigyo_cd1);
                _context.m_koyokeitais.Add(kykt);
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
        //　更新
        private void Update()
        {
            string koyokeitai_cd2 = Request.Form["koyokeitai_cd2"];
            string koyokeitai_nm2 = Request.Form["koyokeitai_nm2"];
            string valid_flg2 = Request.Form["valid_flg2"];
            if (koyokeitai_cd2 == "")
            {
                ErrorMessage += "所属名を入力してください。";
            }
            if (valid_flg2 == null)
            {
                ErrorMessage += "有効/無効をチェックしてください。";
            }
            if (valid_flg2 != null && koyokeitai_nm2 != "")
            {
                M_Koyokeitai kykt = _context.m_koyokeitais.Where(e => e.KoyokeitaiCd.Equals(koyokeitai_cd2)).FirstOrDefault();
                kykt.KoyokeitaiNm = koyokeitai_nm2;
                _context.m_koyokeitais.Update(kykt);
                var a = _context.SaveChanges();
                if (a < 0)
                {
                    Message = "更新できませんでした。";
                }
                else
                {
                    Message = "更新できました。";
                }
            }

        }
        // 削除
        private void Delete()
        {
            string koyokeitai_cd2 = Request.Form["koyokeitai_cd2"];
            string koyokeitai_nm2 = Request.Form["koyokeitai_nm2"];
            string valid_flg2 = Request.Form["valid_flg2"];
            if (koyokeitai_nm2 == "")
            {
                ErrorMessage += "雇用形態名を入力してください。";
            }
            if (valid_flg2 == null)
            {
                ErrorMessage += "有効/無効をチェックしてください。";
            }
            if (valid_flg2 != null && koyokeitai_nm2 != "")
            {
                M_Koyokeitai kykt = _context.m_koyokeitais.Where(e => e.KoyokeitaiCd.Equals(koyokeitai_cd2)).FirstOrDefault();
                kykt.KoyokeitaiNm = koyokeitai_nm2;
                _context.m_koyokeitais.Remove(kykt);
                var a = _context.SaveChanges();
                if (a < 0)
                {
                    Message = "削除できませんでした。";
                }
                else
                {
                    Message = "削除できました。";
                }
            }
        }
    }
}
