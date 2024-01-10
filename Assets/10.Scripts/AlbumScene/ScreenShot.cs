using System.Collections;
using UnityEngine;
using System.IO;
using System;
using OnDot.System;
using I2.Loc;

public class ScreenShot : MonoBehaviour
{
    public Camera albumCharacterCamera;
    public ScreenShotCharacter screenShotCharacter;

    private int resWidth;
    private int resHeight;

    void Awake()
    {
        //resWidth = Screen.width;
        //resHeight = Screen.height;
        //resWidth = 960;
        //resHeight = 1520;
        resWidth = (int)screenShotCharacter.bgImage.GetComponent<RectTransform>().sizeDelta.x;
        resHeight = (int)screenShotCharacter.bgImage.GetComponent<RectTransform>().sizeDelta.y;
        
        albumCharacterCamera.cullingMask = 1 << LayerMask.NameToLayer("AlbumCharacter");
    }

    public void ClickScreenShot()
    {
        SoundManager.Instance.OnPlayOneShot("ve_08");
        TakeScreenShot();
    }

    private void TakeScreenShot()
    {
        StartCoroutine(IRequestNativeGalleryPermission());
    }

    private IEnumerator IRequestNativeGalleryPermission()
    {
        yield return new WaitForEndOfFrame();

        NativeGallery.Permission permission = NativeGallery.RequestPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
        if (permission == NativeGallery.Permission.Granted)
        {
            StartCoroutine(Capture());
        }
    }

    private IEnumerator Capture()
    {
        //albumCharacterCamera.cullingMask = ~(1 << LayerMask.NameToLayer("UI"));
        yield return null;

        RenderTexture rt = RenderTexture.GetTemporary(resWidth, resHeight, 24);
        albumCharacterCamera.targetTexture = rt;
        albumCharacterCamera.Render();
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rect = new Rect(0, 0, screenShot.width, screenShot.height);

        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        string path = OnDotManager.Instance.CaptureFolderName;

        yield return null;

        SaveTextureAsPNG(screenShot, path, fileName);

        RenderTexture.active = null;
        albumCharacterCamera.targetTexture = null;
        RenderTexture.ReleaseTemporary(rt);
        albumCharacterCamera.cullingMask = -1;
    }

    public void SaveTextureAsPNG(Texture2D texture, string path, string name)
    {
        byte[] bytes = texture.EncodeToPNG();
        string title;
        #if UNITY_EDITOR
            if (!Directory.Exists(Application.dataPath + "/ScreenShot"))
                Directory.CreateDirectory(Application.dataPath + "/ScreenShot");
            File.WriteAllBytes(Application.dataPath + "/ScreenShot/" + name, bytes);
            title = Application.dataPath + "/ScreenShot/" + name + "으로 저장되었습니다.";
        #elif UNITY_IOS
            NativeGallery.SaveImageToGallery(texture, path, name);
            title = string.Format(LocalizationManager.GetTermTranslation("SaveAlbum"), path);
        #elif UNITY_ANDROID
            NativeGallery.SaveImageToGallery(texture, path, name);
            title = string.Format(LocalizationManager.GetTermTranslation("SavePath"), path, name);
        #endif
        MainSceneController.Instance.ShowAlarmUI(title) ;
    }
}