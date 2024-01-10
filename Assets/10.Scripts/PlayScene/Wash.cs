using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wash : MonoBehaviour
{
    Vector2 resetSize;
    Sprite origin;
    PaintTools toolselector;

    [Header("WashObject")]
    public List<GameObject> washToolObj;
    public GameObject beautyTool;
    [SerializeField] private GameObject playingWashTool;
    [Header("TargetMask_Wash")]
    public GameObject maskTarget;
    public GameObject maskPackTarget;
    public BoxCollider[] targetCol;
    public GameObject[] cucumberTarget;
    public Texture2D dirtyMask;
    public Texture2D bubbleMask;
    public Texture2D greenMask;
    public Texture2D creamMask;
    public Texture2D clearTex;
    public Texture2D dropMask;
    public List<Texture2D> cheeks;
    public Material clearMat;

    [SerializeField] private int bubbleDrawSize = 32;
    [SerializeField] private int showerDrawSize = 70;
    [SerializeField] private int massagePackDrawSize = 32;
    [SerializeField] private int creamDrawSize = 24;
    [SerializeField] private int towelDrawSize = 70;
    [SerializeField] private int cheekDrawSize = 32;

    public AdvancedMobilePaint.AdvancedMobilePaint paint;
    public WashConfirm washConfirm;
    public bool firstCheek = false;

    public ParticleSystem clearParticle;
    public Animator guideHandanim;
    public AnimationCurve maskColorAlpha;
    public GameObject toolParticle;
    public int cheekNum
    {
        get; set;
    }

    void Awake()
    {
        paint = maskTarget.gameObject.GetComponent<AdvancedMobilePaint.AdvancedMobilePaint>();
        paint.gameObject.GetComponent<MeshCollider>().enabled = true;
    }

    public void Init()
    {
        WashToolHide();
        HideMaskCucumberGuide();

        washToolObj[4].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        beautyTool.GetComponent<RectTransform>().anchoredPosition = new Vector3(-145, -280, 0);
        beautyTool.SetActive(false);

        toolParticle.transform.SetParent(this.transform);
        toolParticle.transform.localPosition = new Vector3(0, 0, 2);

        firstCheek = false;
        if (maskPackTarget.transform.childCount != 1)
        {
            Destroy(maskPackTarget.transform.GetChild(1).gameObject);
        }
        if (cucumberTarget[0].transform.childCount != 1)
        {
            Destroy(cucumberTarget[0].transform.GetChild(1).gameObject);
        }
        if (cucumberTarget[1].transform.childCount != 1)
        {
            Destroy(cucumberTarget[1].transform.GetChild(1).gameObject);
        }

        ChangeTex(dirtyMask);
        washToolObj[0].SetActive(true);
        toolParticle.SetActive(true);
        guideHandanim.gameObject.SetActive(true);
        guideHandanim.SetBool("Wash", true);
    }

    public void MaskInit()
    {
        if (washToolObj[4].GetComponent<Image>().color == new Color(0, 0, 0, 0))
        {
            maskPackTarget.GetComponent<BoxCollider>().enabled = true;
            Destroy(washToolObj[4].transform.GetChild(1).gameObject);
            washToolObj[4].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

        if (!washToolObj[4].GetComponent<MaskTool>().trTarget.transform.GetChild(0).gameObject.activeSelf)
        {
            ShowMaskGuide();
        }
    }

    public void CucumberInit()
    {
        ShowCucumberGuide();
        if (maskPackTarget.transform.childCount != 1 && maskPackTarget.transform.GetChild(1).gameObject.GetComponent<Image>().color == new Color(1, 1, 1, 1))
        {
            Destroy(maskPackTarget.transform.GetChild(1).gameObject);
        }
        if (cucumberTarget[0].transform.childCount != 1)
        {
            Destroy(cucumberTarget[0].transform.GetChild(1).gameObject);
        }
        if (cucumberTarget[1].transform.childCount != 1)
        {
            Destroy(cucumberTarget[1].transform.GetChild(1).gameObject);
        }
        CucumbleRemover cucumbleRemover = FindObjectOfType<CucumbleRemover>();
        if (cucumbleRemover != null)
        {
            Destroy(cucumbleRemover.gameObject);
        }
        this.transform.GetChild(5).GetComponent<CucumbleTool>().clearCount = 0;
    }

    public void WashToolHide()
    {
        //마스크 및 오이 타켓 콜라이더 false로 변경
        for (int i = 0; i < targetCol.Length; i++)
        {
            targetCol[i].enabled = false;
        }

        //wash툴 모두 안보이게 변경
        for (int i = 0; i < washToolObj.Count; i++)
        {
            washToolObj[i].SetActive(false);
        }

        washToolObj[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -130, 0);
        washToolObj[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(50, -155, 0);
        washToolObj[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -65, 0);
        washToolObj[3].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -100, 0);
        washToolObj[4].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        washToolObj[5].GetComponent<RectTransform>().anchoredPosition = new Vector3(-6, -46, 0);
        washToolObj[6].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50, 0);

        toolParticle.SetActive(false);
    }

    public void SetTools(int tool) => toolselector = (PaintTools)tool;

    public void OnPointerDown(RectTransform target)
    {
        SoundManager.Instance.OnPlayOneShot("ve_01");
        guideHandanim.gameObject.SetActive(false);
        resetSize = target.rect.size;
        origin = target.gameObject.GetComponent<Image>().sprite;
        Sprite tool = null;
        playingWashTool = target.gameObject;
        toolParticle.SetActive(false);

        Color c = Color.clear;
        switch (toolselector)
        {
            case PaintTools.Soap:
                guideHandanim.SetBool("Wash", false);
                tool = DataManager.Instance.GetMenuHolderSprite("Soap");
                paint.SetVectorBrush(AdvancedMobilePaint.VectorBrush.Circle, bubbleDrawSize, c, bubbleMask, false, true, false, false);
                paint.brushMode = AdvancedMobilePaint.BrushProperties.Pattern;
                paint.drawMode = AdvancedMobilePaint.DrawMode.Pattern;
                paint.drawEnabled = true;
                paint.SetDrawPoint(target.GetChild(0));
                paint.SetDrawPos(new Vector3());
                maskTarget.gameObject.GetComponent<RawImage>().texture = paint.tex;
                break;

            case PaintTools.Shower:
                tool = DataManager.Instance.GetMenuHolderSprite("Shower");
                target.gameObject.GetComponentInChildren<ParticleSystem>().Play();
                paint.SetVectorBrush(AdvancedMobilePaint.VectorBrush.Circle, showerDrawSize, c, dropMask, false, true, false, false);
                paint.brushMode = AdvancedMobilePaint.BrushProperties.Pattern;
                paint.drawMode = AdvancedMobilePaint.DrawMode.Pattern;
                paint.drawEnabled = true;
                paint.SetDrawPoint(target.GetChild(0));
                paint.SetDrawPos(new Vector3(-240f, 170f));
                break;

            case PaintTools.Towel:
                tool = DataManager.Instance.GetMenuHolderSprite("Towel");
                c = Color.white;
                c.a = 0f;
                paint.SetVectorBrush(AdvancedMobilePaint.VectorBrush.Circle, towelDrawSize, c, null, false, true, false, false);
                paint.useLockArea = false;
                paint.useMaskLayerOnly = false;
                paint.useThreshold = false;
                paint.drawEnabled = true;
                paint.SetDrawPoint(target.GetChild(0));
                paint.SetDrawPos(new Vector3());
                maskTarget.gameObject.GetComponent<RawImage>().texture = paint.tex;
                break;

            case PaintTools.MassagePack:
                tool = DataManager.Instance.GetMenuHolderSprite("Massage");
                paint.SetVectorBrush(AdvancedMobilePaint.VectorBrush.Circle, massagePackDrawSize, c, greenMask, false, true, false, false);
                paint.brushMode = AdvancedMobilePaint.BrushProperties.Pattern;
                paint.drawMode = AdvancedMobilePaint.DrawMode.Pattern;
                paint.drawEnabled = true;
                paint.SetDrawPoint(target.GetChild(0));
                paint.SetDrawPos(new Vector3());
                maskTarget.gameObject.GetComponent<RawImage>().texture = paint.tex;
                break;

            case PaintTools.Mask:
                playingWashTool = gameObject.transform.GetChild(4).gameObject;
                //별도의 스크립트로 구현
                break;

            case PaintTools.Cucumber:
                playingWashTool = gameObject.transform.GetChild(5).gameObject;
                //별도의 스크립트로 구현
                break;

            case PaintTools.Cream:
                tool = DataManager.Instance.GetMenuHolderSprite("Cream");
                paint.SetVectorBrush(AdvancedMobilePaint.VectorBrush.Circle, creamDrawSize, c, creamMask, false, true, false, false);
                paint.brushMode = AdvancedMobilePaint.BrushProperties.Pattern;
                paint.drawMode = AdvancedMobilePaint.DrawMode.Pattern;
                paint.drawEnabled = true;
                paint.SetDrawPoint(target.GetChild(0));
                paint.SetDrawPos(new Vector3());
                maskTarget.gameObject.GetComponent<RawImage>().texture = paint.tex;
                break;

            case PaintTools.Cheek:
                guideHandanim.SetBool("Beauty", false);
                tool = DataManager.Instance.GetMenuHolderSprite("Cheek");
                paint.SetVectorBrush(AdvancedMobilePaint.VectorBrush.Circle, cheekDrawSize, c, cheeks[cheekNum], false, true, false, false);
                paint.brushMode = AdvancedMobilePaint.BrushProperties.Pattern;
                paint.drawMode = AdvancedMobilePaint.DrawMode.Pattern;
                paint.drawEnabled = true;
                paint.SetDrawPoint(target.GetChild(0));
                paint.SetDrawPos(new Vector3());
                maskTarget.gameObject.GetComponent<RawImage>().texture = paint.tex;
                break;
        }
        target.gameObject.GetComponent<Image>().sprite = tool;
        target.sizeDelta = new Vector2(tool.rect.xMax, tool.rect.yMax);

        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        target.gameObject.transform.position = newPosition;
    }

    public void OnPointerDrag(RectTransform target)
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        target.gameObject.transform.position = newPosition;
    }

    public void OnPointerUp(RectTransform target)
    {
        if (toolselector == PaintTools.Shower)
        {
            target.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
        }
        paint.drawEnabled = false;
        target.position = target.gameObject.transform.position;
        target.sizeDelta = new Vector2(resetSize.x, resetSize.y);
        target.gameObject.GetComponent<Image>().sprite = origin;

        if (playingWashTool.name == "Cheek")
        {
            if (target.anchoredPosition.y < -300)
            {
                target.anchoredPosition = new Vector3(-145, -280, 0);
            }
        }

        bool confirm = washConfirm.WashConfirms(toolselector, paint, this);

        if (confirm)
        {
            SoundManager.Instance.OnPlayOneShot("ve_02");
            ChangeTex();
            playingWashTool.SetActive(false);
            ClearParticlePlay();
            PlayManager.Instance.UpdateGameStepInWash();
            PlayManager.Instance.ShowNextButton(true);
        }
    }

    public void ChangeTex()
    {
        GameStep gameStep = PlayerDataManager.Instance.GetUserInfo().playingStep;

        switch (gameStep)
        {
            case GameStep.Soap:
                ChangeTex(bubbleMask);
                break;

            case GameStep.Shower:
                ChangeTex(dropMask);
                break;

            case GameStep.Towel:
                ChangeTex(clearTex);
                break;

            case GameStep.MassagePack:
                ChangeTex(greenMask);
                break;

            case GameStep.Mask:
                ChangeTex(clearTex);
                break;

            case GameStep.Cucumber:
                ChangeTex(clearTex);
                break;

            case GameStep.Cream:
                ChangeTex(clearTex);
                PlayManager.Instance.ChangeHair();
                break;
            case GameStep.Cheek:
                beautyTool.SetActive(false);
                ChangeTex(cheeks[cheekNum]);
                break;
        }
    }

    public void ChangeTex(Texture2D texture2D)
    {
        if (texture2D == null || paint == null)
        {
            return;
        }
        if (paint.tex == null)
        {
            paint.InitializeEverything();
        }
        paint.ReadCurrentCustomPattern2(texture2D);
        paint.tex.LoadRawTextureData(paint.patternBrushBytes);
        paint.tex.Apply(false);

        paint.GetComponent<RawImage>().texture = paint.tex;
        maskTarget.gameObject.GetComponent<RawImage>().texture = paint.tex;
        paint.pixels = paint.patternBrushBytes;
    }

    public void ShowWashTools(GameStep gameStep)
    {
        foreach (var item in washToolObj)
        {
            item.SetActive(false);
        }
        playingWashTool = this.transform.GetChild((int)gameStep).gameObject;
        int nextTool = washToolObj.FindIndex(x => x.gameObject == playingWashTool);
        toolParticle.SetActive(true);
        this.transform.GetChild(nextTool).gameObject.SetActive(true);
    }

    public void ShowBeautyTool()
    {
        beautyTool.SetActive(true);
        toolParticle.SetActive(true);
        toolParticle.transform.SetParent(beautyTool.transform);
        toolParticle.transform.localPosition = new Vector3(-64, -71, 2);
    }

    public IEnumerator RemoveMask()
    {
        float time = 0;

        while (time < 1.0f)
        {
            time += Time.deltaTime;
            maskPackTarget.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, maskColorAlpha.Evaluate(time));
            yield return null;
        }
    }

    public IEnumerator RemoveCucumber()
    {
        float time = 0;

        while (time < 1.0f)
        {
            time += Time.deltaTime;
            cucumberTarget[0].transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, maskColorAlpha.Evaluate(time));
            cucumberTarget[1].transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, maskColorAlpha.Evaluate(time));
            yield return null;
        }
    }

    public void ShowMaskGuide()
    {
        maskPackTarget.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ShowCucumberGuide()
    {
        cucumberTarget[0].transform.GetChild(0).gameObject.SetActive(true);
        cucumberTarget[1].transform.GetChild(0).gameObject.SetActive(true);
    }

    private void HideMaskCucumberGuide()
    {
        maskPackTarget.transform.GetChild(0).gameObject.SetActive(false);
        cucumberTarget[0].transform.GetChild(0).gameObject.SetActive(false);
        cucumberTarget[1].transform.GetChild(0).gameObject.SetActive(false);
    }

    public void DestroyCucumber()
    {
        if (cucumberTarget[0].transform.childCount != 1)
        {
            Destroy(cucumberTarget[0].transform.GetChild(1).gameObject);
        }
        if (cucumberTarget[1].transform.childCount != 1)
        {
            Destroy(cucumberTarget[1].transform.GetChild(1).gameObject);
        }
    }

    public void ClearParticlePlay()
    {
        clearParticle.Play();
    }
}
