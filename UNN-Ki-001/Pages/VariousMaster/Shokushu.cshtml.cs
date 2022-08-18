using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster
{
    public class ShokushuModel : PageModel
    {
        private readonly KintaiDbContext _context;
        private readonly ApplicationDbContext context1;
        public List<Display> Data = new List<Display>();
        public Display? Data1;
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Shokushu_cd { get; set; }
        public string? Shokushu_nm { get; set; }
        public string? Valid_flg { get; set; }


        public ShokushuModel(UNN_Ki_001.Data.KintaiDbContext context, ApplicationDbContext application)
        {
            _context = context;
            context1 = application;
        }

        public void OnGet()
        {
/*            Data.Clear();
            if (id != null)
            {　
                M_Shokushu sks = _context.m_shokushus.Where(e => e.ShokushuCd.Equals(id.ToString())).FirstOrDefault();
                Data1 = new Display();
                Data1.shokushu_nm = sks.ShokushuNm;
                Data1.shokushu_cd = sks.ShokushuCd;
                Data1.valid_flg = 　sks.ValidFlg;
            }*/
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
            Shokushu_cd = Request.Form["shokushu_cd"];
            Shokushu_nm = Request.Form["shokushu_nm"];
            Valid_flg = Request.Form["valid_flg"];
            var no = from m_shokushu in _context.m_shokushus
                     orderby m_shokushu.ShokushuCd
                     select new { m_shokushu.ShokushuCd, m_shokushu.ShokushuNm, m_shokushu.ValidFlg };
            // 条件による検索すること(value＝nullは検索条件にならないこと。)
            if (!string.IsNullOrEmpty(Shokushu_cd))
            {
                no = no.Where(e => e.ShokushuCd.Equals(Shokushu_cd));
            }

            if (!string.IsNullOrEmpty(Shokushu_nm))
            {
                no = no.Where(e => e.ShokushuNm.Equals(Shokushu_nm));
            }

            if (!string.IsNullOrEmpty(Valid_flg))
            {
                no = no.Where(e => e.ValidFlg.Equals(Valid_flg));
            }

            foreach (var item in no)
            {

                Display d = new Display();
                d.shokushu_cd = item.ShokushuCd;
                d.shokushu_nm = item.ShokushuNm;
                d.valid_flg = item.ValidFlg;
                Data.Add(d);
            }

        }
        // 新規
        private void Register()
        {
            string shokushu_cd1 = Request.Form["shokushu_cd1"];
            string shokushu_nm1 = Request.Form["shokushu_nm1"];
            string valid_flg1 = Request.Form["valid_flg1"];
            string kigyo_cd1 = "C001";
            if (shokushu_cd1 == "")
            {
                ErrorMessage += "職種コードを入力してください。";
            }
            if (shokushu_nm1 == "")
            {
                ErrorMessage += "職種名を入力してください。";
            }
            if (valid_flg1 == null)
            {
                ErrorMessage += "有効/無効をチェックしてください。";
            }
            if (valid_flg1 != null && shokushu_nm1 != "" && shokushu_cd1 != "")
            {
                M_Shokushu sk = new(shokushu_cd1, shokushu_nm1, valid_flg1, kigyo_cd1);
                _context.m_shokushus.Add(sk);
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
            string shokushu_cd2 = Request.Form["shokushu_cd2"];
            string shokushu_nm2 = Request.Form["shokushu_nm2"];
            string valid_flg2 = Request.Form["valid_flg2"];
            if (shokushu_nm2 == "")
            {
                ErrorMessage += "職種名を入力してください。";
            }
            if (valid_flg2 == null)
            {
                ErrorMessage += "有効/無効をチェックしてください。";
            }
            if (valid_flg2 != null && shokushu_nm2 != "")
            {
                M_Shokushu sk = _context.m_shokushus.Where(e => e.ShokushuCd.Equals(shokushu_cd2)).FirstOrDefault();
                sk.ShokushuNm = shokushu_nm2;
                _context.m_shokushus.Update(sk);
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
            string shokushu_cd2 = Request.Form["shokushu_cd2"];
            string shokushu_nm2 = Request.Form["shokushu_nm2"];
            string valid_flg2 = Request.Form["valid_flg2"];
            if (shokushu_nm2 == "")
            {
                ErrorMessage += "所属名を入力してください。";
            }
            if (valid_flg2 == null)
            {
                ErrorMessage += "有効/無効をチェックしてください。";
            }
            if (valid_flg2 != null && shokushu_nm2 != "")
            {
                M_Shokushu sk = _context.m_shokushus.Where(e => e.ShokushuCd.Equals(shokushu_cd2)).FirstOrDefault();
                sk.ShokushuNm = shokushu_nm2;
                _context.m_shokushus.Remove(sk);
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
