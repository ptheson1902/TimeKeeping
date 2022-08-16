using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Diagnostics;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster
{
    public class AffiliationModel : PageModel
    {
        private readonly KintaiDbContext _context;
        private readonly ApplicationDbContext context1;
        public List<Display> Data = new List<Display>();
        public Display? Data1;
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Shozoku_cd { get; set; }
        public string? Shozoku_nm { get; set; }
        public string? Valid_flg { get; set; }


        public AffiliationModel(UNN_Ki_001.Data.KintaiDbContext context, ApplicationDbContext application)
        {
            _context = context;
            context1 = application;
        }

        public void OnGet(int? id)
        {
            Data.Clear();
            if(id != null)
            { 
                shozokukensaku sz = _context.shozoku.Where(e => e.shozoku_cd.Equals(id.ToString())).FirstOrDefault();
                Data1 = new Display();
                Data1.shozoku_nm = sz.shozoku_nm;
                Data1.shozoku_cd = sz.shozoku_cd;
                Data1.valid_flg = sz.valid_flg;
            }
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
        private void Search()
        {
             Shozoku_cd = Request.Form["shozoku_cd"];
             Shozoku_nm = Request.Form["shozoku_nm"];
             Valid_flg = Request.Form["valid_flg"];
            var no = from m_shozoku in _context.shozoku
                     orderby m_shozoku.shozoku_cd
                     select new { m_shozoku.shozoku_cd, m_shozoku.shozoku_nm, m_shozoku.valid_flg } ;
            // 条件による検索すること(value＝nullは検索条件にならないこと。)
            if (!string.IsNullOrEmpty(Shozoku_cd))
            {
                no = no.Where(e => e.shozoku_cd.Equals(Shozoku_cd));
            }

            if (!string.IsNullOrEmpty(Shozoku_nm))
            {
                no = no.Where(e => e.shozoku_nm.Equals(Shozoku_nm));
            }

            if (!string.IsNullOrEmpty(Valid_flg))
            {
                no = no.Where(e => e.valid_flg.Equals(Valid_flg));
            }
            
            foreach (var item in no)
            {     
                
                    Display d = new Display();
                    d.shozoku_cd = item.shozoku_cd;
                    d.shozoku_nm = item.shozoku_nm;
                    d.valid_flg = item.valid_flg;
                    Data.Add(d);                
            }
            
        }
        // 新規
        private void Register()
        {
            string shozoku_cd1 = Request.Form["shozoku_cd1"];
            string shozoku_nm1 = Request.Form["shozoku_nm1"];
            string valid_flg1 = Request.Form["valid_flg1"];
            string kigyo_cd1 = "C001";
            if(shozoku_cd1 == "")
            {
                    ErrorMessage += "所属コードを入力してください。";
            }
            if (shozoku_nm1 == "")
            {
                    ErrorMessage += "所属名を入力してください。";
            }
            if (valid_flg1 == null)
            {
                    ErrorMessage += "有効/無効をチェックしてください。";
            }
            if (valid_flg1 != null && shozoku_nm1 != "" && shozoku_cd1 != "")
            {
                shozokukensaku sz = new(shozoku_cd1, shozoku_nm1, valid_flg1, kigyo_cd1);
                _context.shozoku.Add(sz);
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
            string shozoku_cd2 = Request.Form["shozoku_cd2"];
            string shozoku_nm2 = Request.Form["shozoku_nm2"];
            string valid_flg2 = Request.Form["valid_flg2"];
            if (shozoku_nm2 == "")
            {
                ErrorMessage += "所属名を入力してください。";
            }
            if (valid_flg2 == null)
            {
                ErrorMessage += "有効/無効をチェックしてください。";
            }
            if (valid_flg2 != null && shozoku_nm2 != "")
            {
                shozokukensaku sz = _context.shozoku.Where(e => e.shozoku_cd.Equals(shozoku_cd2)).FirstOrDefault();
                sz.shozoku_nm = shozoku_nm2;
                _context.shozoku.Update(sz);
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
        private void Delete()
        {
            string shozoku_cd2 = Request.Form["shozoku_cd2"];
            string shozoku_nm2 = Request.Form["shozoku_nm2"];
            string valid_flg2 = Request.Form["valid_flg2"];
            if (shozoku_nm2 == "")
            {
                ErrorMessage += "所属名を入力してください。";
            }
            if (valid_flg2 == null)
            {
                ErrorMessage += "有効/無効をチェックしてください。";
            }
            if (valid_flg2 != null && shozoku_nm2 != "")
            {
                shozokukensaku sz = _context.shozoku.Where(e => e.shozoku_cd.Equals(shozoku_cd2)).FirstOrDefault();
                sz.shozoku_nm = shozoku_nm2;
                _context.shozoku.Remove(sz);
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
