using UnityEngine;

public class ImageStretch : MonoBehaviour
{
    [SerializeField] private bool activeWidth = true;
    [SerializeField] private bool activeHeight = true;

    private int lastScreenWidth;
    private int lastScreenHeight;

    private void Awake()
    {
        Resize();
    }

    void Update()
    {
        Resize();
    }

    private void Resize()
    {
        if (lastScreenWidth == Screen.width &&
            lastScreenHeight == Screen.height)
        {
            return;
        }

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        // 기본 해상도 비율
        float fixedAspectRatio = 9f / 16f;

        // 현재 해상도 비율
        float currentAspectRatio = (float)lastScreenWidth / lastScreenHeight;
        float resizeScale;
        if (currentAspectRatio > fixedAspectRatio)
        {
            // 가로 설정
            resizeScale = activeWidth ? currentAspectRatio / fixedAspectRatio : 1f;
        }
        else
        {
            // 세로 설정
            resizeScale = activeHeight ? fixedAspectRatio / currentAspectRatio : 1f;
        }
        transform.localScale = new Vector3(resizeScale, resizeScale, transform.localScale.z);
    }
}
