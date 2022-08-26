using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster
{
    public class KoyokeitaiModel : BasePageModel
    {
        public List<Display> Data = new List<Display>();
        public Display? Data1;

        public KoyokeitaiModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Koyokeitai_cd { get; set; }
        public string? Koyokeitai_nm { get; set; }
        public string? Valid_flg { get; set; }


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
            var update_action = Request.Form["update_action"];
            switch (update_action)
            {
                case "update":
                    Update(shain);
                    break;
                default: break;
            }
            var delete_action = Request.Form["delete_action"];
            switch (delete_action)
            {
                case "delete":
                    Delete(shain);
                    break;
                default: break;
            }
        }
        //�@����
        private void Search(M_Shain shain)
        {
            Koyokeitai_cd = Request.Form["koyokeitai_cd"];
            Koyokeitai_nm = Request.Form["koyokeitai_nm"];
            Valid_flg = Request.Form["valid_flg"];
            var no = from m_koyokeitai in _kintaiDbContext.m_koyokeitais
                     where m_koyokeitai.KigyoCd.Equals(shain.KigyoCd)
                     orderby m_koyokeitai.KoyokeitaiCd
                     select new { m_koyokeitai.KoyokeitaiCd, m_koyokeitai.KoyokeitaiNm, m_koyokeitai.ValidFlg };
            // �����ɂ�錟�����邱��(value��null�͌��������ɂȂ�Ȃ����ƁB)
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
        // �V�K
        private void Register(M_Shain shain)
        {
            string koyokeitai_cd1 = Request.Form["koyokeitai_cd1"];
            string koyokeitai_nm1 = Request.Form["koyokeitai_nm1"];
            string valid_flg1 = Request.Form["valid_flg1"];
            string kigyo_cd1 = "C001";
            if (koyokeitai_cd1 == "")
            {
                ErrorMessage += "�ٗp�`�ԃR�[�h����͂��Ă��������B";
            }
            if (koyokeitai_nm1 == "")
            {
                ErrorMessage += "�ٗp�`�Ԗ�����͂��Ă��������B";
            }
            if (valid_flg1 == null)
            {
                ErrorMessage += "�L��/�������`�F�b�N���Ă��������B";
            }
            if (valid_flg1 != null && koyokeitai_nm1 != "" && koyokeitai_cd1 != "")
            {
                M_Koyokeitai kykt = new(koyokeitai_cd1, koyokeitai_nm1, valid_flg1, kigyo_cd1);
                _kintaiDbContext.m_koyokeitais.Add(kykt);
                var a = _kintaiDbContext.SaveChanges();

                if (a < 0)
                {
                    Message = "�o�^�ł��܂���ł����B";
                }
                else
                {
                    Message = "�o�^�ł��܂����B";
                }
            }
        }
        //�@�X�V
        private void Update(M_Shain shain)
        {
            string koyokeitai_cd2 = Request.Form["koyokeitai_cd2"];
            string koyokeitai_nm2 = Request.Form["koyokeitai_nm2"];
            string valid_flg2 = Request.Form["valid_flg2"];
            if (koyokeitai_cd2 == "")
            {
                ErrorMessage += "����������͂��Ă��������B";
            }
            if (valid_flg2 == null)
            {
                ErrorMessage += "�L��/�������`�F�b�N���Ă��������B";
            }
            if (valid_flg2 != null && koyokeitai_nm2 != "")
            {
                M_Koyokeitai kykt = _kintaiDbContext.m_koyokeitais.Where(e => e.KoyokeitaiCd.Equals(koyokeitai_cd2)&& e.KigyoCd.Equals(shain.KigyoCd)).FirstOrDefault();
                kykt.KoyokeitaiNm = koyokeitai_nm2;
                kykt.ValidFlg = valid_flg2;
                _kintaiDbContext.m_koyokeitais.Update(kykt);
                var a = _kintaiDbContext.SaveChanges();
                if (a < 0)
                {
                    Message = "�X�V�ł��܂���ł����B";
                }
                else
                {
                    Message = "�X�V�ł��܂����B";
                }
            }

        }
        // �폜
        private void Delete(M_Shain shain)
        {
            string koyokeitai_cd2 = Request.Form["koyokeitai_cd2"];
            string koyokeitai_nm2 = Request.Form["koyokeitai_nm2"];
            string valid_flg2 = Request.Form["valid_flg2"];
            if (koyokeitai_nm2 == "")
            {
                ErrorMessage += "�ٗp�`�Ԗ�����͂��Ă��������B";
            }
            if (valid_flg2 == null)
            {
                ErrorMessage += "�L��/�������`�F�b�N���Ă��������B";
            }
            if (valid_flg2 != null && koyokeitai_nm2 != "")
            {
                M_Koyokeitai kykt = _kintaiDbContext.m_koyokeitais.Where(e => e.KoyokeitaiCd.Equals(koyokeitai_cd2) && e.KigyoCd.Equals(shain.KigyoCd)).FirstOrDefault();
                kykt.KoyokeitaiNm = koyokeitai_nm2;
                _kintaiDbContext.m_koyokeitais.Remove(kykt);
                var a = _kintaiDbContext.SaveChanges();
                if (a < 0)
                {
                    Message = "�폜�ł��܂���ł����B";
                }
                else
                {
                    Message = "�폜�ł��܂����B";
                }
            }
        }
    }
}
