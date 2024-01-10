using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour
{
    public ItemScroll itemScroll;
    public void BeautyInit()
    {
        itemScroll.ClosetInit(ClosetKind.Cheek);
        PlayManager.Instance.itemScrollShow(true);
    }

    public void ClosetInit()
    {
        itemScroll.ClosetInit(ClosetKind.BackHair);
        PlayManager.Instance.itemScrollShow(true);
    }

    public void AccessoryInit()
    {
        itemScroll.ClosetInit(ClosetKind.Earring);
        PlayManager.Instance.itemScrollShow(true);
    }

    public void ChangeScrollItem(GameStep step)
    {
        ClosetKind closetKind = System.Enum.Parse<ClosetKind>(step.ToString());
        itemScroll.ClosetInit(closetKind);
    }
}
