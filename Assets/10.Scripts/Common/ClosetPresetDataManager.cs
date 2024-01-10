using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public partial class DataManager : MonoBehaviour
{
    private List<ClosetPresetData> closetPresetDatas;
    public List<ClosetPresetData> ClosetPresetDatas
    {
        get { return closetPresetDatas; }
    }

    public void InitClosetPresetData()
    {
        closetPresetDatas = JsonConvert.DeserializeObject<List<ClosetPresetData>>(closetPresetData.ToString());
        GetClosetPresetData();
    }

    private void GetClosetPresetData()
    {
        foreach (var item in closetPresetDatas)
        {
            ClosetPresetData data = new ClosetPresetData();
            data.id = item.id;
            data.closetId = item.closetId;
            data.kind = Enum.Parse<ClosetKind>(item.kind.ToString());
            closetPresetInfoData.Add(data);
        }
    }
}