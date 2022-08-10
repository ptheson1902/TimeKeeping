using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Security.Claims;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Data
{
    public class KinmuManager
    {
        private readonly KintaiDbContext _context;

        public KinmuManager(KintaiDbContext context)
        {
            _context = context;
        }

        // T勤務
        // 
        // 
        // savechange時に集計を自動計算
        // ・所定時間
        // ・総労働時間
        // ・控除時間
        // ・休憩時間
        // ・法廷内時間
        // ・法定外時間
        // ・法定休日時間
        // savechange
        // ・ T勤務の修正時
        // ・ M勤務の修正時
        // ・ 

        private class KinmuData
        {
            private readonly T_Kinmu _tKinmu;
            private M_Kinmu? _mKinmu; 

            private KinmuData(T_Kinmu tKinmu, M_Kinmu? mKinmu = null)
            {
                _tKinmu = tKinmu;
                _mKinmu = mKinmu;
            }
        }
    }
}
