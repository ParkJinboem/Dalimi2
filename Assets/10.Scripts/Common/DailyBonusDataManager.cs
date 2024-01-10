using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public partial class DataManager : MonoBehaviour
{
    private List<AttendanceData> attendanceDatas;
    public List<AttendanceData> AttendanceDatas
    {
        get { return attendanceDatas; }
    }

    public void InitDailyBonusData()
    {
        attendanceDatas = JsonConvert.DeserializeObject<List<AttendanceData>>(dailyBonusData.ToString());
        GetDailyBonusData();
    }

    private void GetDailyBonusData()
    {
        foreach (var item in attendanceDatas)
        {
            AttendanceData data = new AttendanceData();
            data.date = item.date;
            data.day = item.day;
            data.closetId = item.closetId;
            data.isGet = false;
            attendanceInfoDatas.Add(data);
        }
    }
}
