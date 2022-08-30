using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster
{
    public class KoyokeitaiModel : BasePageModel
    {
        public string? Message { get; set; }
        public string? ValidFlg { get; set; }
        public List<M_Koyokeitai>? _searchList { get; set; }
        public string? _searchListString { get; set; }
        [BindProperty]
        public M_Koyokeitai? SearchData { get; set; }
        [BindProperty]
        public M_Koyokeitai? CRUDData { get; set; }
        public KoyokeitaiModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
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
        //�@����
        private void Search(M_Shain shain)
        {
            _searchList = _kintaiDbContext.m_koyokeitais
                .Where(e => e.KigyoCd.Equals(shain.KigyoCd))
                .WhereIf(SearchData.KoyokeitaiCd != null, e => e.KoyokeitaiCd.Equals(SearchData.KoyokeitaiCd))
                .WhereIf(SearchData.KoyokeitaiNm != null, e => e.KoyokeitaiNm.Equals(SearchData.KoyokeitaiNm))
                .WhereIf(SearchData.ValidFlg != null, e => e.ValidFlg.Equals(SearchData.ValidFlg))
                .OrderBy(e => e.KoyokeitaiNm)
                .OrderBy(e => e.KoyokeitaiCd)
                .ToList();
            ValidFlg = SearchData.ValidFlg;
            foreach(var item in _searchList)
            {
                item.Shains = null;
            }
            _searchListString = JsonConvert.SerializeObject(_searchList);
            if (_searchList == null)
                Message = "�������ʂ�����܂���B";
        }
        // �V�K
        private void Insert(M_Shain shain)
        {
            if (CRUDData == null)
                return;

            CRUDData.KigyoCd = shain.KigyoCd;
            try
            {
                _kintaiDbContext.Add(CRUDData);
                _kintaiDbContext.SaveChanges();
                Message = "�ٗp�`�ԃ}�X�^�̓o�^���o���܂����B";
            }
            catch
            {
                Message = "�ٗp�`�ԃ}�X�^�̓o�^�Ɏ��s���܂����B";
            }
        }
        //�@�X�V
        private void Update(M_Shain shain)
        {
            if (CRUDData == null)
                return;

            CRUDData.KigyoCd = shain.KigyoCd;
            try
            {
                _kintaiDbContext.Update(CRUDData);
                _kintaiDbContext.SaveChanges();
                Message = "�ٗp�`�ԃ}�X�^�̍X�V���o���܂����B";
            }
            catch
            {
                Message = "�ٗp�`�ԃ}�X�^�̍X�V�Ɏ��s���܂����B";
            }
        }
        // �폜
        private void Delete(M_Shain shain)
        {
            if (CRUDData == null)
                return;

            CRUDData.KigyoCd = shain.KigyoCd;
            try
            {
                _kintaiDbContext.Remove(CRUDData);
                _kintaiDbContext.SaveChanges();
                Message = "�ٗp�`�ԃ}�X�^�̍폜���o���܂����B";
            }
            catch
            {
                Message = "�ٗp�`�ԃ}�X�^�̍폜�Ɏ��s���܂����B";
            }
        }
    }
}
