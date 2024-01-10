using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.Loc;

public class AttendanceEvent
{
    public delegate void AttendenceActions();
    public static AttendenceActions AttendenceHandler;
    public static void UpdateAttendence() 
    {
        AttendenceHandler?.Invoke();
    }
}

public class AttendanceUI : MonoBehaviour
{
    [SerializeField] private Button rewardBtn;
    [SerializeField] private Button adRewardBtn;
    [SerializeField] private TextMeshProUGUI clearText;
    [SerializeField] private Button exitBtn;
    [SerializeField] private List<GameObject> objDailyItem;
    [SerializeField] private List<AttendanceData> attendanceData;
    [SerializeField] private int day;
    public GameObject notification;
    public GameObject particleObj;

    private void Awake()
    {
        AttendanceEvent.AttendenceHandler += GetBonus;
    }

    private void OnDestroy()
    {
        AttendanceEvent.AttendenceHandler -= GetBonus;
    }

    public void Init()
    {
        day = PlayerDataManager.Instance.AttendanceDay;
        attendanceData = new List<AttendanceData>();
        attendanceData = DataManager.Instance.AttendanceDatas;

        if(!PlayerDataManager.Instance.GetUserInfo().attendanceData.adGet)
        {
            adRewardBtn.interactable = true;
        }
        else if(PlayerDataManager.Instance.GetUserInfo().attendanceData.adGet)
        {
            adRewardBtn.interactable = false;
        }

        gameObject.SetActive(true);

        //출석보상 아이템 중복제거
        List<int> DailyItemCount = new List<int>();
        foreach (var item in attendanceData)
        {
            DailyItemCount.Add(item.day);
        }

        //출석보상 아이템 날짜별초기화
        for (int i = 0; i < DailyItemCount.Count; i++)
        {
            objDailyItem[i].GetComponent<DailyItem>().Init(attendanceData[i]);
        }

        //버튼 On/Off 설정
        if (PlayerDataManager.Instance.GetUserInfo().attendanceData.isGet)
        {
            ShowRewardBtn(true); 
        }
        else if(!PlayerDataManager.Instance.GetUserInfo().attendanceData.isGet)
        {
            ShowRewardBtn(false);
        }
    }

    public void ClaimBtn()
    {
        SoundManager.Instance.OnClickSoundEffect();
        notification.SetActive(false);
        GetReward(day);
        ShowRewardBtn(true);
    }

    public void AdClaimBtn()
    {
        SoundManager.Instance.OnClickSoundEffect();
        AdsManager.Instance.WatchVideoWithAttendance(acs =>
        {
            switch (acs)
            {
                case AdsManager.AdsCallbackState.Success:
                    notification.SetActive(false);
                    GetReward(day);
                    GetReward(day + 1);
                    ShowRewardBtn(true);
                    break;
                case AdsManager.AdsCallbackState.Loading:
                    MainSceneController.Instance.ShowAlarmUI(LocalizationManager.GetTermTranslation("LoadAd"));
                    break;
            }
        });
    }

    private void GetReward(int day)
    {
        if (day > objDailyItem.Count)
        {
            return;
        }

        objDailyItem[day - 1].GetComponent<DailyItem>().GetReward(particleObj);
    }

    private void ShowRewardBtn(bool show)
    {
        rewardBtn.gameObject.SetActive(!show);
        adRewardBtn.gameObject.SetActive(!show && day + 1 < objDailyItem.Count);
        clearText.gameObject.SetActive(show);
    }

    public void GetBonus()
    {
        PlayerDataManager.Instance.Attendance();
    }

    public void ExitBtn()
    {
        SoundManager.Instance.OnClickSoundEffect();
        this.gameObject.SetActive(false);
    }
}
