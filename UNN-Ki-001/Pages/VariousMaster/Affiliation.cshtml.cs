using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster
{
    public class AffiliationModel : PageModel
    {
        private readonly KintaiDbContext _context;
        private readonly ApplicationDbContext context1;
        public List<Display> Data = new List<Display>();

        public AffiliationModel(UNN_Ki_001.Data.KintaiDbContext context, ApplicationDbContext application)
        {
            _context = context;
            context1 = application;
        }
        public void OnGet()
        {
            Data.Clear();
        }
        public  async Task OnPostAsync()
        {
            var action = Request.Form["action"];
            switch (action)
            {
                case "search":
                    await SearchAsync();
                    break;
                case "register":
                    await RegisterAsync();
                    break;
                default: break;
            }
        }
        private Task SearchAsync()
        {
            string shozoku_cd = Request.Form["shozoku_cd"];
            string shozoku_nm = Request.Form["shozoku_nm"];
            string valid_flg = Request.Form["valid_flg"];
            var no = from m_shozoku in _context.shozoku
                     select new { m_shozoku.shozoku_cd, m_shozoku.shozoku_nm, m_shozoku.valid_flg };
            // ðŒ‚É‚æ‚éŒŸõ‚·‚é‚±‚Æ(valuenull‚ÍŒŸõðŒ‚É‚È‚ç‚È‚¢‚±‚ÆB)
            if (!string.IsNullOrEmpty(shozoku_cd))
            {
                no = no.Where(e => e.shozoku_cd.Equals(shozoku_cd));
            }

            if (!string.IsNullOrEmpty(shozoku_nm))
            {
                no = no.Where(e => e.shozoku_nm.Equals(shozoku_nm));
            }

            if (!string.IsNullOrEmpty(valid_flg))
            {
                no = no.Where(e => e.valid_flg.Equals(valid_flg));
            }
            foreach (var item in no)
            {
                Display d = new Display();
                d.shozoku_cd = item.shozoku_cd;
                d.shozoku_nm = item.shozoku_nm;
                d.valid_flg = item.valid_flg;
                Data.Add(d);
            }

            return Task.CompletedTask;
        }
        private Task RegisterAsync()
        {
            NpgsqlConnection connect = new NpgsqlConnection ("Server=49.212.187.222;Database=Kintai;User Id=UNN999;Password=Od12U,Aknr.18");                      
                connect.Open();        
                string shozoku_cd = Request.Form["shozoku_cd1"];
                string shozoku_nm = Request.Form["shozoku_nm1"];
                string valid_flg = Request.Form["valid_flg1"];
                string kigyo_cd = "C001";
                string sqlText = @"
                                    INSERT INTO m_shozoku
                                        (shozoku_cd , shozoku_nm , valid_flg , kigyo_cd)
                                    VALUES 
                                        (@shozoku_cd , @shozoku_nm , @valid_flg , @kigyo_cd)
                                ";
                NpgsqlCommand sqlInsert = new NpgsqlCommand(sqlText, connect);
                sqlInsert.Parameters.AddWithValue("@shozoku_cd",shozoku_cd);
                sqlInsert.Parameters.AddWithValue("@shozoku_nm",shozoku_nm);
                sqlInsert.Parameters.AddWithValue("@valid_flg", valid_flg);
                sqlInsert.Parameters.AddWithValue("@kigyo_cd", kigyo_cd);
                sqlInsert.Prepare();
                sqlInsert.ExecuteNonQuery();   
            return Task.CompletedTask;
        }
    }
}
