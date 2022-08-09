using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Security.Claims;

namespace UNN_Ki_001.Data.Models
{
    public class KinmuManager
    {
        private KintaiDbContext _context;

        public KinmuManager(KintaiDbContext context)
        {
            _context = context;
        }

        public void Shukkin(ClaimsPrincipal user)
        {
            // NULLをつぶす
            if (user == null || user.Identity == null || user.Identity.Name == null)
            {
                throw new Exception("ユーザー名が取得できませんでした。");
            }

            Shukkin(user.Identity.Name);
        }
        public void Shukkin(string name)
        {
            if (name == null) throw new Exception("ユーザー名を入力して下さい。");
            
            // 社員の詳細を取得
            
        }


        public void Taikin(string Name)
        {

        }
    }
}
