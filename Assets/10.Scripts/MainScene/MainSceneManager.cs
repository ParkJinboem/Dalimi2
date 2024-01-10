using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button play;
    [SerializeField] private Button album;
    [SerializeField] private Button option;
    [SerializeField] private Button dailyReward;
    [SerializeField] private Button store;

    [Header("GameObject")]
    [SerializeField] private AttendanceUI attendanceUI;
    [SerializeField] private GameObject optionPrefabs;

    public void Start()
    {
        int dailyItemCount = DataManager.Instance.AttendanceDatas.FindAll(x => x.day == PlayerDataManager.Instance.AttendanceDay).Count;
        dailyReward.gameObject.SetActive(dailyItemCount > 0);
    }

    public void ClickedOption()
    {
        SoundManager.Instance.OnClickSoundEffect();
        GameObject obj;
        obj = Instantiate(optionPrefabs,this.transform);
        obj.GetComponent<OptionUI>().Init();
    }

    public void ClickedStore()
    {
        
    }

    public void ClickedDailyReward()
    {
        SoundManager.Instance.OnClickSoundEffect();
        attendanceUI.Init();
    }

    public void ClickedAlbum()
    {
        MainSceneController.Instance.ShowAlbumScene();
    }

    public void ClicekdAdRemove()
    {
        SoundManager.Instance.OnClickSoundEffect();
        MainSceneController.Instance.BuyAdRemove();
    }
    
    public void ClickedPlay()
    {
        MainSceneController.Instance.ShowPlayScene();
    }

}
