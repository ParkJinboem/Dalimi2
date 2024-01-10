using UnityEngine;

namespace OnDot.Util
{
    public class UIPoolItem : ObjectPool<UIPoolItem, UIObjectItem, PoolItemData>
    {

    }

    public class UIObjectItem : PoolObject<UIPoolItem, UIObjectItem, PoolItemData>
    {
        IPoolItem poolItem;

        protected override void SetReferences()
        {
            instance.transform.localScale = Vector3.one;
            poolItem = instance.GetComponent<IPoolItem>();
        }

        public override void WakeUp(PoolItemData poolItemData)
        {
            poolItem.SetPoolObject(this);
            poolItem.SetPoolItemData(poolItemData);
            poolItem.Init();
            instance.SetActive(true);
        }

        public override void Sleep()
        {
            instance.SetActive(false);
        }
    }
}