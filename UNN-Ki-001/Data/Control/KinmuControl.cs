using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Data.Control
{
    public class KinmuControl
    {
        //private const string NULL_CHAR = "N/A";
        /// <summary>
        /// KintaiDbContextクラス
        /// </summary>
        private readonly KintaiDbContext _context;
        private T_Kinmu kinmu;
        private T_Kyukei kyukei;
        public KinmuControl(T_Kinmu _kinmu, KintaiDbContext context)
        {
            kinmu = _kinmu;
            _context = context;
        }
        private M_Kinmu? mKinmu
        {
            get
            {
                if (kinmu.KinmuCd == null)
                    return null;
                return _context.m_kinmus
                    .Where(e => e.KigyoCd.Equals(kinmu.KigyoCd) && e.KinmuCd.Equals(kinmu.KinmuCd))
                    .FirstOrDefault();
            }
        }

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
        public T_Kinmu DakokuEnd()
        {
            // 出勤済みか確認する。
            if (kinmu.DakokuFrDt == null || kinmu.DakokuFrTm == null)
                throw new Exception("退勤打刻を行うには、先に出勤打刻が必要です。");

            DateTime now = DateTime.Now;
            DakokuEndWriter(now, true);
            return kinmu;
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
                dc = (mKinmu == null) ? dc : dc.MarumeProcess(mKinmu.KinmuToMarumeTm, mKinmu.KinmuToMarumeKbn);
            }

            // 実績記録を保存
            kinmu.KinmuToDt = dc.Date;
            kinmu.KinmuToTm = dc.Time;
            DateTime kinmuFr = DateTime.ParseExact(kinmu.KinmuFrDt + kinmu.KinmuFrTm, "yyyyMMddHHmm", null);
            DateTime kinmuTo = DateTime.ParseExact(kinmu.KinmuToDt + kinmu.KinmuToTm, "yyyyMMddHHmm", null);
            if(kinmuFr > kinmuTo)
            {
                kinmu.KinmuToDt = kinmu.KinmuFrDt;
                kinmu.KinmuToTm = kinmu.KinmuFrTm;
                kinmuTo = kinmuFr;
            }
            Calculation(kinmuFr, kinmuTo);
        }
        private void Calculation(DateTime kinmuFr, DateTime kinmuTo)
        {
            if (mKinmu == null)
                return;

            //　所定時間
            kinmu.Shotei = int.Parse(mKinmu.ShoteiTm) < 60 ? int.Parse(mKinmu.ShoteiTm) * 60 : int.Parse(mKinmu.ShoteiTm);
            //　休憩時間
            kinmu.Kyukei = 0;
            if (mKinmu.KyukeiAutoFlg != null && mKinmu.KyukeiAutoFlg.Equals("1"))
            {
                if(mKinmu.Kyukei1FrTm != null && mKinmu.Kyukei1ToTm != null)
                {
                    kyukei = new T_Kyukei(
                                            kinmu.KigyoCd,
                                            kinmu.ShainNo,
                                            kinmu.KinmuDt,
                                            null,
                                            null,
                                            kinmu.KinmuFrDt,
                                            mKinmu.Kyukei1FrTm,
                                            null,
                                            kinmu.KinmuToDt,
                                            mKinmu.Kyukei1ToTm
                                        );
                    kyukei.kyukeiCal();
                    kinmu.Kyukei += kyukei.Kyukei;
                    _context.t_kyukei.Add(kyukei);
                }
                if (mKinmu.Kyukei2FrTm != null && mKinmu.Kyukei2ToTm != null)
                {
                    kyukei = new T_Kyukei(
                                            kinmu.KigyoCd,
                                            kinmu.ShainNo,
                                            kinmu.KinmuDt,
                                            2,
                                            null,
                                            kinmu.KinmuFrDt,
                                            mKinmu.Kyukei2FrTm,
                                            null,
                                            kinmu.KinmuToDt,
                                            mKinmu.Kyukei2ToTm
                                        );
                    kyukei.kyukeiCal();
                    kinmu.Kyukei += kyukei.Kyukei;
                    _context.t_kyukei.Add(kyukei);
                }
                if (mKinmu.Kyukei3FrTm != null && mKinmu.Kyukei3ToTm != null)
                {
                    kyukei = new T_Kyukei(
                                            kinmu.KigyoCd,
                                            kinmu.ShainNo,
                                            kinmu.KinmuDt,
                                            3,
                                            null,
                                            kinmu.KinmuFrDt,
                                            mKinmu.Kyukei3FrTm,
                                            null,
                                            kinmu.KinmuToDt,
                                            mKinmu.Kyukei3ToTm
                                        );
                    kyukei.kyukeiCal();
                    kinmu.Kyukei += kyukei.Kyukei;
                    _context.t_kyukei.Add(kyukei);
                }
            }

            //　総労働時間
            if (int.Parse(kinmu.KinmuFrTm) < 1200 && int.Parse(kinmu.KinmuToTm) > 1300)
                kinmu.Sorodo = (int)(kinmuTo - kinmuFr).TotalMinutes - kinmu.Kyukei;
            else
                kinmu.Sorodo = (int)(kinmuTo - kinmuFr).TotalMinutes;
            //　控除時間

            kinmu.Kojo = kinmu.Shotei - kinmu.Sorodo;
            //　法定内時間

            kinmu.Hoteinai = 0;
            //　法定外時間

            kinmu.Hoteigai = 0;
            //　深夜時間

            kinmu.Shinya = 0;
            //　法定休日時間

            kinmu.Hoteikyu = 0;
        }
    }
}
