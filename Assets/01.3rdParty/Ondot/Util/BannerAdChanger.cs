using UnityEngine;

public class BannerAdChanger : MonoBehaviour, IBannerAdHandler
{
    private RectTransform rectTransform;
    private float positionY;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        UpdatePositionY();
    }

    public void SetPositionY(float positionY)
    {
        this.positionY = positionY;

        UpdatePositionY();
    }

    private void UpdatePositionY()
    {
        if (rectTransform == null)
        {
            return;
        }

        Vector2 position = rectTransform.offsetMax;
        position.y = positionY;
        rectTransform.offsetMax = position;
    }
}
