using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    private Coroutine loadingScene = null;
    private float time;

    public void StartCoroutine(SceneLoad sceneType)
    {
        slider.value = 0;
        time = 0.0f;
        if (loadingScene == null) { loadingScene = StartCoroutine(LoadingSceneCoroutine(sceneType)); }

    }

    public void StopLoadingCoroutine()
    {
        if(loadingScene != null)
        {
            StopCoroutine(loadingScene);
        }
        loadingScene = null;
        this.gameObject.SetActive(false);
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
        if (slider.value >= operation.progress) { time = 0f; }

        while (!operation.isDone)
        {
            yield return null;
            slider.value = operation.progress;

            time += Time.deltaTime; 
            if (operation.progress < 0.9f) 
            { slider.value = Mathf.Lerp(slider.value, operation.progress, time); } 
            else 
            {
                slider.value = Mathf.Lerp(slider.value, 1f, time); 
                if (slider.value == 1.0f) 
                { operation.allowSceneActivation = true; yield break; } 
            }
        }
        yield return null;
    }
}
