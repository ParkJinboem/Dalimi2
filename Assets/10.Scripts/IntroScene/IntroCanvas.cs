using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroCanvas : MonoSingleton<IntroCanvas>
{
    private bool isLoading;

    private void Start()
    {
        DataManager.Instance.Init();
        PlayerDataManager.Instance.Init();
        SoundManager.Instance.Init();

        Statics.clearCharacterCount = PlayerDataManager.Instance.GetUserInfo().clearCharacterCount;
        if (Statics.clearCharacterCount == 0)
        {
            Statics.clearCharacterCount = 1;
        }
    }

    public void OnClickStart()
    {
        SoundManager.Instance.OnClickSoundEffect();        

        if (isLoading)
        {
            return;
        }
        isLoading = true;

        LoadingPanel.Instance.Show(() =>
        {
            StartCoroutine(LoadMainScene());
        });

        GameManager.Instance.Init(() =>
        {
            LoadingPanel.Instance.End();
        });
    }

    IEnumerator LoadMainScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Statics.mainSceneName, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
