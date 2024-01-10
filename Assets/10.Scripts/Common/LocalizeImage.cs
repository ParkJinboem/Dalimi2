using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

public class LocalizeImage : MonoBehaviour
{
    public enum LocalizeLanguage
    {
        English,
        Korean
    }

    [Serializable]
    public class Data
    {
        public LocalizeLanguage language;
        public Sprite sp;
    }

    [SerializeField] List<Data> datas;

    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();

        SetImage();

        LocalizationManager.OnLocalizeEvent += OnLocalize;
    }

    private void OnDestroy()
    {
        LocalizationManager.OnLocalizeEvent -= OnLocalize;
    }

    private void OnLocalize()
    {
        SetImage();
    }

    private void SetImage()
    {
        Data data = datas.Find(x => x.language.ToString() == PlayerDataManager.Instance.language);
        if (data != null)
        {
            img.sprite = data.sp;
        }
        else
        {
            img.sprite = datas[0].sp;
        }
        img.SetNativeSize();
    }
}
