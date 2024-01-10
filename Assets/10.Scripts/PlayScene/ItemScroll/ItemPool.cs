using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class ItemPool : ObjectPool<ItemPool, ItemObject, Vector2>
{
   
}

public class ItemObject : PoolObject<ItemPool, ItemObject, Vector2>
{
    public Item item;

    protected override void SetReferences()
    {
        item = instance.GetComponent<Item>();
        item.PoolObject = this;
    }

    public override void WakeUp(Vector2 info)
    {
        instance.transform.localScale = new Vector3(1, 1, 1);
        instance.transform.position = info;
        instance.SetActive(true);
    }

    public override void Sleep()
    {
        instance.SetActive(false);
    }
}
