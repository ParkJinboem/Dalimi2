using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public partial class DataManager : MonoBehaviour
{
    private List<ClosetData> closetDatas;
    public List<ClosetData> ClosetDatas
    {
        get { return closetDatas; }
    }

    public void InitClosetData()
    {
        closetDatas = JsonConvert.DeserializeObject<List<ClosetData>>(closetData.ToString());
        GetClosetData();
    }

    private void GetClosetData()
    {
        foreach (var item in closetDatas)
        {
            ClosetData data = new ClosetData();
            data.id = item.id;
            data.name = item.name;
            data.type = Enum.Parse<ClosetType>(item.type.ToString());
            data.kind = Enum.Parse<ClosetKind>(item.kind.ToString());
            data.linkId = item.linkId;
            data.posX = item.posX;
            data.posY = item.posY;
            data.characterName = item.characterName;
            closetInfoDatas.Add(data);
        }
    }
}