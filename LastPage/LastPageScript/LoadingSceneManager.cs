using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] Slider slider;
    private Coroutine loadingScene = null;

    public void StartCoroutine(SceneLoad sceneType)
    {
        slider.value = 0;
        if (loadingScene == null) { loadingScene = StartCoroutine(LoadingSceneCoroutine(sceneType)); }

    }

    private IEnumerator LoadingSceneCoroutine(SceneLoad sceneType)
    {
        AsyncOperation operation;

        switch (sceneType)
        {
            case SceneLoad.MAIN:
                operation = SceneManager.LoadSceneAsync("Main");
                break;
            case SceneLoad.STAGE1:
                operation = SceneManager.LoadSceneAsync("Stage1");
                break;
            case SceneLoad.STAGE2:
                operation = SceneManager.LoadSceneAsync("Stage2");
                break;
            case SceneLoad.STAGE3:
                operation = SceneManager.LoadSceneAsync("Stage3");
                break;
            default:
                operation = SceneManager.LoadSceneAsync("Main");
                break;
        }

        while (!operation.isDone)
        {
            slider.value = operation.progress;
        }
        StopCoroutine(loadingScene);
        loadingScene = null;
        yield return null;
    }
}
