using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance.Record
{
    public class KyukeiModel : BasePageModel
    {
        public KyukeiModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public JsonResult? OnGet(string kinmuDt = "", string shainNo = "", string kigyoCd = "")
        {
            // �K�{���͂������Ă���ꍇ
            if(kinmuDt == "" || shainNo == "" || kigyoCd == "")
            {
                return null;
            }

            // �x�e���R�[�h���擾
            List<T_Kyukei> kyukeis = _kintaiDbContext.t_Kyukeis
                .Where(e => e.KinmuDt == kinmuDt && shainNo == e.ShainNo && e.KigyoCd == kigyoCd)
                .ToList();

            // �x�e���R�[�h�𐮌`
            int targetDtInt = int.Parse(kinmuDt);
            List<KyukeiOutputModel> response = new List<KyukeiOutputModel>();
            foreach(var item in kyukeis)
            {
                KyukeiOutputModel kom = new();

                // �J�n����
                if(item.DakokuFrDate != null)
                {
                    // ����
                    DateTime dt = (DateTime)item.DakokuFrDate;
                    kom.Start = dt.ToString("HH:mm");

                    // �敪
                    int dtInt = int.Parse(dt.ToString("yyyyMMdd"));
                     kom.StartKbn = (dtInt == targetDtInt) ? 0 : (dtInt > targetDtInt) ? 2 : 1;
                }

                // �I������
                if (item.DakokuToDate != null)
                {
                    // ����
                    DateTime dt = (DateTime)item.DakokuToDate;
                    kom.End = dt.ToString("HH:mm");

                    // �敪
                    int dtInt = int.Parse(dt.ToString("yyyyMMdd"));
                    kom.EndKbn = (dtInt == targetDtInt) ? 0 : (dtInt > targetDtInt) ? 2 : 1;
                }

                // ���X�g�ɒǉ�
                response.Add(kom);
            }

            // json���쐬
            return new JsonResult(response) ;
        }
    }

    public class KyukeiOutputModel
    {
        public int StartKbn { get; set; }
        public int EndKbn { get; set; }
        public string Start { get; set; } = "";
        public string End { get; set; } = "";
    }
}


