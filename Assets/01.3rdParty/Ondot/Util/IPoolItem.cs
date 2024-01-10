namespace OnDot.Util
{
    public class PoolItemData
    {

    }

    public class PoolItemData<T> : PoolItemData
    {
        public T value;

        public PoolItemData(T value)
        {
            this.value = value;
        }
    }

    public interface IPoolItem
    {
        public void SetPoolObject(UIObjectItem poolObject);
        public void SetPoolItemData(PoolItemData poolItemData);
        public void Init();
    }
}