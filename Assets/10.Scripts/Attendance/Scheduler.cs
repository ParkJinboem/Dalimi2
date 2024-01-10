using UnityEngine;
using System;

public class SchedulerEvent
{
    public delegate void ChangeDateHandler();
    public static event ChangeDateHandler OnChangeDate;
    public static void ChangeDate()
    {
        OnChangeDate?.Invoke();
    }
}
public class Scheduler : MonoBehaviour
{
    public enum SchedulerType
    {
        None, // 없음
        Daily, // 일일 초기화
    }

    [Serializable]
    public class S
    {
        private SchedulerType schedulerType;

        private DateTime checkDate;

        private DateTime scheduleDate;
        public DateTime ScheduleDate
        {
            get { return scheduleDate; }
        }

        public TimeSpan RemainTime
        {
            get { return scheduleDate - UnbiasedTime.Instance.Now().ToUniversalTime(); }
        }

        public string RemainTimeToString(string format, string zeroTime)
        {
            if (RemainTime.TotalSeconds > 0)
            {
                return RemainTime.ToString(format);
            }
            else
            {
                return zeroTime;
            }
        }

        public float RemainTimeToRatio()
        {
            float ratio = (float)((UnbiasedTime.Instance.Now().ToUniversalTime() - checkDate).TotalSeconds / (scheduleDate - checkDate).TotalSeconds);
            return ratio > 0.9f ? 1f : ratio;
        }

        public bool IsAfter
        {
            get
            {
                if (schedulerType == SchedulerType.None)
                {
                    return false;
                }
                else
                {
                    DateTime now = UnbiasedTime.Instance.Now().ToUniversalTime();
                    int result = DateTime.Compare(now, scheduleDate);
                    return result > 0;
                }
            }
        }

        public S(SchedulerType schedulerType, string date, float add = 0)
        {
            this.schedulerType = schedulerType;

            Update(date, add);

            Debug.Log("checkDate : " + checkDate);
            Debug.Log("scheduleDate : " + scheduleDate);
        }

        private void Update(string date, float add)
        {
            if (string.IsNullOrEmpty(date))
            {
                checkDate = UnbiasedTime.Instance.Now().ToUniversalTime();
            }
            else
            {
                checkDate = ParseStringToDateTime(date).ToUniversalTime();
            }

            if (schedulerType == SchedulerType.Daily)
            {
                scheduleDate = checkDate.Date.AddDays(1);
                // 테스트
                //scheduleDate = UnbiasedTime.Instance.Now().ToUniversalTime().AddSeconds(10);
            }
        }
    }

    private DateTime ReadTimestamp(string key, DateTime defaultValue)
    {
        long tmp = Convert.ToInt64(PlayerPrefs.GetString(key, "0"));
        if (tmp == 0)
        {
            WriteTimestamp(key, defaultValue);
            return defaultValue;
        }
        return DateTime.FromBinary(tmp);
    }

    private void WriteTimestamp(string key, DateTime time)
    {
        PlayerPrefs.SetString(key, time.ToBinary().ToString());
    }

    public static string GetNowToString()
    {
        return UnbiasedTime.Instance.Now().ToUniversalTime().ToBinary().ToString();
    }

    public static DateTime ParseStringToDateTime(string date)
    {
        return DateTime.FromBinary(Convert.ToInt64(date));
    }
}
