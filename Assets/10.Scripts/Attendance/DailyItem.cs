using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyItem : MonoBehaviour
{
    [SerializeField] private GameObject rootCheck;
    [SerializeField] private Image vMark;
    public int day;
    public Image itemImage;
    public ClosetData dailyCloset;
    public List<ClosetData> specialDailyCloset;

    public void Init(AttendanceData attendanceData)
    {
        dailyCloset = DataManager.Instance.GetClosetDataWithId(attendanceData.closetId);
        itemImage.sprite = DataManager.Instance.GetCharacterPartUISprite(DataManager.Instance.GetClosetDataWithId(attendanceData.closetId).name);
        itemImage.SetNativeSize();
        rootCheck.SetActive(day <= PlayerDataManager.Instance.GetRewardDailyItemCount());
    }

    public void SpcialInit(List<AttendanceData> attendanceData)
    {
        specialDailyCloset = new List<ClosetData>();
        for (int i = 0; i < attendanceData.Count; i++)
        {
            specialDailyCloset.Add(DataManager.Instance.GetClosetDataWithId(attendanceData[i].closetId));
        }

        rootCheck.SetActive(day <= PlayerDataManager.Instance.GetRewardDailyItemCount());
    }

    public void DailyEvent()
    {
        AttendanceEvent.UpdateAttendence();
    }

    //public void GetReward()
    //{
    //    //V마크 활성화
    //    rootCheck.SetActive(true);
    //    //획득한 아이템을 유저데이터에 저장
    //    if(day == 7)
    //    {
    //        foreach (ClosetData closetData in specialDailyCloset)
    //        {
    //            PlayerDataManager.Instance.AddRewardCloset(closetData);
    //        }
    //    }
    //    else
    //    {
    //        PlayerDataManager.Instance.AddRewardCloset(dailyCloset);
    //    }

    //    PlayerDataManager.Instance.GetUserInfo().attendanceData.isGet = true;
    //    PlayerDataManager.Instance.SaveData();
    //}

    public void GetReward(GameObject particle)
    {
        //획득한 아이템을 유저데이터에 저장
        //day 7 보상 삭제_20230907 박진범
        //if (day == 7)
        //{
        //    StartCoroutine(ShowParticle(particle, 50));
        //    foreach (ClosetData closetData in specialDailyCloset)
        //    {
        //        PlayerDataManager.Instance.AddRewardCloset(closetData);
        //    }
        //}
        //else
        {
            StartCoroutine(ShowParticle(particle,20));
            PlayerDataManager.Instance.AddRewardCloset(dailyCloset);
        }

        PlayerDataManager.Instance.GetUserInfo().attendanceData.day = day;
        PlayerDataManager.Instance.GetUserInfo().attendanceData.isGet = true;
        PlayerDataManager.Instance.SaveData();
    }

    public IEnumerator ShowParticle(GameObject particle, int size)
    {
        //GameObject obj = Instantiate(particle, this.transform);
        //obj.transform.localScale = new Vector2(size, size);

        //yield return new WaitForSeconds(0.5f);
        rootCheck.SetActive(true);
        rootCheck.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        DotweenManager.Instance.DoSizeImage(1.8f, 1, 0.5f, vMark.gameObject);
        yield return new WaitForSeconds(0.5f);

        DotweenManager.Instance.DoFadeImage(0, 0.5f, 1, rootCheck.gameObject);
        //DotweenManager.Instance.DoSizeImage(0, 1, 1, vMark.gameObject);
        //DotweenManager.Instance.DoLocalMoveX(18.3f, 1, vMark.gameObject.GetComponent<RectTransform>());
        //DotweenManager.Instance.DoLocalMoveY(-18.6f, 1, vMark.gameObject.GetComponent<RectTransform>());
    }
}
