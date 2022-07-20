using Microsoft.AspNetCore.Identity;

namespace UNN_Ki_001.Areas.Identity.Model
{
    public class ApplicationUser: IdentityUser
    {
        /// <summary>
        /// システムログイン時のID。
        /// 既存のID（GUID）
        /// </summary>
        public string? UserId { get; set; }
    }
}
