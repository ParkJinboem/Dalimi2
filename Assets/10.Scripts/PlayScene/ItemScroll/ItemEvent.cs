public class ItemEvent
{
    public delegate void ItemDeselect();
    public static event ItemDeselect itemDelectedHandler;
    public static void UpdateItemDeSelected()
    {
        itemDelectedHandler?.Invoke();
    }
}
