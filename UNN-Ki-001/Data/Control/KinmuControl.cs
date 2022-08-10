using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Data.Control
{
    public class KinmuControl
    {
        private const string NULL_CHAR = "N/A";
        /// <summary>
        /// KintaiDbContextクラス
        /// </summary>
        private readonly KintaiDbContext _context;
        private T_Kinmu kinmu;
        public KinmuControl(T_Kinmu _kinmu, KintaiDbContext context)
        {
            kinmu = _kinmu;
            _context = context;
        }
        private M_Kinmu? mKinmu
        {
            get
            {
                if (mKinmuBack == null)
                {
                    if (kinmu.KinmuCd == null)
                        return null;
                    mKinmuBack = _context.m_kinmus
                        .Where(e => e.KigyoCd.Equals(kinmu.KigyoCd) && e.KinmuCd.Equals(kinmu.KinmuCd))
                        .FirstOrDefault();
                    if (mKinmuBack == null)
                        return null;
                }
                return mKinmuBack;
            }
        }
        private M_Kinmu? mKinmuBack { get; set; }

        public T_Kinmu DakokuStart()
        {
            // 最新の勤務記録を参照し、退勤済みか確認する。
            T_Kinmu? record = _context.t_kinmus
                .Where(e => e.KigyoCd.Equals(kinmu.KigyoCd) && e.KinmuDt.Equals(kinmu.KinmuDt) && e.ShainNo.Equals(kinmu.ShainNo) && e.DakokuFrTm != null)
                .OrderByDescending(e => e.KinmuDt)
                .FirstOrDefault();
            if (record != null && (record.DakokuToDt == null || record.DakokuToTm == null))
                throw new Exception("出勤打刻を行うには、退勤打刻が必要です。");

            DateTime now = DateTime.Now;
            DakokuStartWriter(now, true);
            return kinmu;
        }

        public T_Kinmu DakokuEnd()
        {
            // 出勤済みか確認する。
            if (kinmu.DakokuFrDt == null || kinmu.DakokuFrTm == null)
                throw new Exception("退勤打刻を行うには、先に出勤打刻が必要です。");

            DateTime now = DateTime.Now;
            DakokuEndWriter(now, true);
            return kinmu;
        }

        public void DakokuStartWriter(DateTime dateTime, Boolean andKinmuStartWrite = false)
        {
            // 打刻記録のフォーマット
            DateControl dc = new DateControl(dateTime);

            // 打刻記録を保存
            kinmu.DakokuFrDt = dc.Date;
            kinmu.DakokuFrTm = dc.Time;

            // 勤務記録も保存
            if (andKinmuStartWrite)
            {
                KinmuStartWriter(dc, true);
            }
        }

        public void DakokuEndWriter(DateTime dateTime, Boolean andKinmuEndWrite = false)
        {
            // 打刻記録のフォーマット
            DateControl dc = new DateControl(dateTime);

            // 打刻記録を保存
            kinmu.DakokuToDt = dc.Date;
            kinmu.DakokuToTm = dc.Time;

            // 勤務記録も保存
            if (andKinmuEndWrite)
            {
                KinmuEndWriter(dc, true);
            }
        }

        public void KinmuStartWriter(DateTime dateTime, Boolean marumeProcess = false)
        {
            // 実績記録のフォーマット
            DateControl dc = new DateControl(dateTime);
            KinmuStartWriter(dc, marumeProcess);
        }
        private void KinmuStartWriter(DateControl dc, Boolean marumeProcess = false)
        {
            // 丸め処理の実行
            if (marumeProcess)
            {
                dc = (mKinmu == null) ? dc : dc.MarumeProcess(mKinmu.KinmuFrMarumeTm, mKinmu.KinmuFrMarumeKbn);
                if (mKinmu != null && mKinmu.KinmuFrCtrlFlg != null && mKinmu.KinmuFrCtrlFlg.Equals("0") && mKinmu.KinmuFrTm != null)
                {
                    DateControl kinmuFrDc = new DateControl(kinmu.KinmuDt, mKinmu.KinmuFrTm);
                    if (dc.Origin < kinmuFrDc.Origin)
                        dc = kinmuFrDc;
                }
            }

            // 実績記録を保存
            kinmu.KinmuFrDt = dc.Date;
            kinmu.KinmuFrTm = dc.Time;
        }

        public void KinmuEndWriter(DateTime dateTime, Boolean marumeProcess = false)
        {
            // 実績記録のフォーマット
            DateControl dc = new DateControl(dateTime);
            KinmuEndWriter(dc, marumeProcess);
        }
        private void KinmuEndWriter(DateControl dc, Boolean marumeProcess = false)
        {
            // 丸め処理の実行
            if (marumeProcess)
            {
                dc = (mKinmu == null) ? dc : dc.MarumeProcess(mKinmu.KinmuFrMarumeTm, mKinmu.KinmuFrMarumeKbn);
            }

            // 実績記録を保存
            kinmu.KinmuToDt = dc.Date;
            kinmu.KinmuToTm = dc.Time;
        }
    }
}
