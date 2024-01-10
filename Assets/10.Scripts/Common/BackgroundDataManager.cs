using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public partial class DataManager : MonoBehaviour
{
    private List<BackgroundData> backgroundDatas;
    public List<BackgroundData> BackgroundDatas
    {
        get { return backgroundDatas; }
    }

    public void InitBackGroundData()
    {
        backgroundDatas = JsonConvert.DeserializeObject<List<BackgroundData>>(backgroundData.ToString());
        GetBackGroundData();
    }

    private void GetBackGroundData()
    {
        foreach (var item in backgroundDatas)
        {
            BackgroundData data = new BackgroundData();
            data.id = item.id;
            data.name = item.name;
            data.type = Enum.Parse<BackgroundType>(item.type.ToString());
            data.kind = Enum.Parse<BackgroundKind>(item.kind.ToString());
            backgroundInfoDatas.Add(data);
        }
    }
}
