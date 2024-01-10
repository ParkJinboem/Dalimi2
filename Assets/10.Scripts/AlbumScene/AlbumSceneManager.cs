using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlbumSceneManager : MonoBehaviour
{
    public GameObject albumPrefabs;
    public Transform panelParent;
    public Transform destroyParent;
    [SerializeField] private Transform returnParent;
    [SerializeField] private GameObject albumPanelPagePrefabs;
    [SerializeField] private GameObject albumCharacterSlotPrefabs;
    [SerializeField] private List<SaveCharacter> albumSaveCharacters;

    [Header("UI")]
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button prevBtn;
    [SerializeField] private List<GameObject> mainPanel = new List<GameObject>();
    [SerializeField] private List<GameObject> albumCharacter = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI nowText;
    [SerializeField] private TextMeshProUGUI MaxText;

    private GameObject panelObj;
    [SerializeField] private int mainPanelIndex;
    private int prevPos = -1065;
    private int midPos = 0;
    private int nextPos = 1065;

    public void Init()
    {
        mainPanelIndex = 0;
        AlbumSetUp();
    }

    public void AlbumSetUp()
    {
        albumSaveCharacters = PlayerDataManager.Instance.AlbumSaveCharacters();
        albumCharacter.Clear();
        mainPanel.Clear();
        CreateAlbum();
        ShowButton();
    }

    public void CreateAlbum()
    {
        int panelNum = 0;
        if (albumSaveCharacters.Count % 4 == 0 && mainPanelIndex != 0)
        {
            mainPanelIndex--;
        }
        for (int i = 0; i < albumSaveCharacters.Count; i++)
        {
            //MainPanel생성
            if (i % 4 == 0)
            {
                panelObj = Instantiate(albumPanelPagePrefabs, panelParent);
                {
                    if (panelNum < mainPanelIndex)
                    {
                        panelObj.GetComponent<RectTransform>().localPosition = new Vector2(prevPos, 75);
                    }
                    else if (panelNum == mainPanelIndex)
                    {
                        panelObj.GetComponent<RectTransform>().localPosition = new Vector2(0, 75);
                    }
                    else if (panelNum > mainPanelIndex)
                    {
                        panelObj.GetComponent<RectTransform>().localPosition = new Vector2(nextPos, 75);
                    }
                    panelObj.GetComponent<RectTransform>().sizeDelta = new Vector2(970, 1225);
                    panelNum++;
                }
            }

            //SlotData 생성
            if (returnParent.childCount != 0)
            {
                GameObject returnObj = returnParent.transform.GetChild(0).gameObject;

                returnObj.GetComponent<AlbumCharacterSlot>().Init(albumSaveCharacters[i]);
                returnObj.transform.SetParent(panelObj.transform);
            }
            else
            {
                GameObject slotObj;
                slotObj = Instantiate(albumCharacterSlotPrefabs, panelObj.transform);
                slotObj.GetComponent<AlbumCharacterSlot>().Init(albumSaveCharacters[i]);
                albumCharacter.Add(slotObj);
            }
        }

        for (int i = 0; i < panelParent.transform.childCount; i++)
        {
            mainPanel.Add(panelParent.GetChild(i).gameObject);
        }

        //페이지 표시 설정
        nowText.text = (mainPanelIndex + 1).ToString();
        MaxText.text = panelParent.transform.childCount.ToString();
        if (MaxText.text == "0")
        {
            MaxText.text = "1";
        }
    }

    public void NextClikd()
    {
        SoundManager.Instance.OnClickSoundEffect();
        nextBtn.interactable = false;
        prevBtn.interactable = false;
        DotweenManager.Instance.DoLocalMoveX(prevPos, 0.5f, mainPanel[mainPanelIndex].GetComponent<RectTransform>());
        DotweenManager.Instance.DoLocalMoveX(midPos, 0.5f, mainPanel[mainPanelIndex + 1].GetComponent<RectTransform>());
        mainPanelIndex++;
        nowText.text = (mainPanelIndex + 1).ToString();
        ShowButton();
    }

    public void PrevClikd()
    {
        SoundManager.Instance.OnClickSoundEffect();
        nextBtn.interactable = false;
        prevBtn.interactable = false;
        DotweenManager.Instance.DoLocalMoveX(nextPos, 0.5f, mainPanel[mainPanelIndex].GetComponent<RectTransform>());
        DotweenManager.Instance.DoLocalMoveX(midPos, 0.5f, mainPanel[mainPanelIndex - 1].GetComponent<RectTransform>());
        mainPanelIndex--;
        nowText.text = (mainPanelIndex + 1).ToString();
        ShowButton();
    }

    public void ShowButton()
    {
        if(mainPanelIndex + 1 == 1)
        {
            prevBtn.gameObject.SetActive(false);
            nextBtn.gameObject.SetActive(true);
        }
        else if (mainPanelIndex + 1 == panelParent.transform.childCount)
        {
            nextBtn.gameObject.SetActive(false);
            prevBtn.gameObject.SetActive(true);
        }
        else
        {
            prevBtn.gameObject.SetActive(true);
            nextBtn.gameObject.SetActive(true);
        }
        if(panelParent.transform.childCount == 0 || panelParent.transform.childCount == 1)
        {
            prevBtn.gameObject.SetActive(false);
            nextBtn.gameObject.SetActive(false);
        }
        StartCoroutine(BtnWatiTime());
    }

    IEnumerator BtnWatiTime()
    {
        yield return new WaitForSeconds(0.7f);
        nextBtn.interactable = true;
        prevBtn.interactable = true;
    }

    public void ExitClicked()
    {
        for (int i = 0; i < albumCharacter.Count; i++)
        {
            albumCharacter[i].transform.SetParent(returnParent);
        }
        mainPanel.Clear();
        mainPanelIndex = 0;
        foreach (Transform child in panelParent)
        {
            Destroy(child.gameObject);
        }
        MainSceneController.Instance.ShowScreenShotCanvas(false);
        MainSceneController.Instance.ShowMainScene();
    }


    

    //public void CreateAlbum()
    //{
    //    for (int i = 0; i < albumSaveCharacters.Count; i++)
    //    {
    //        if (i % 4 == 0)
    //        {
    //            panelObj = Instantiate(albumPanelPagePrefabs, panelParent);
    //            if(albumFirstPage)
    //            {
    //                albumFirstPage = false;
    //            }
    //            else
    //            {
    //                panelObj.GetComponent<RectTransform>().localPosition = new Vector2(1065, 75);
    //                panelObj.GetComponent<RectTransform>().sizeDelta = new Vector2(970, 1225);
    //            }
    //        }

    //        if(returnParent.childCount != 0)
    //        {
    //            GameObject returnObj = returnParent.transform.GetChild(0).gameObject;

    //            returnObj.GetComponent<AlbumCharacterSlot>().Init(albumSaveCharacters[i]);
    //            returnObj.transform.SetParent(panelObj.transform);
    //        }
    //        else
    //        {
    //            GameObject slotObj;
    //            slotObj = Instantiate(albumCharacterSlotPrefabs, panelObj.transform);
    //            slotObj.GetComponent<AlbumCharacterSlot>().Init(albumSaveCharacters[i]);
    //            albumCharacter.Add(slotObj);
    //        }

    //    }
    //}
}
