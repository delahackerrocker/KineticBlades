using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    public OVROverlay overlay_Background;
    public OVROverlay overlay_LoadingText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);

        overlay_Background.enabled = false;
        overlay_LoadingText.enabled = false;
    }
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        //StartCoroutine(ShowOverlayAndLoad(sceneName));
    }

    IEnumerator ShowOverlayAndLoad(string sceneName)
    {
        overlay_Background.enabled = true;
        overlay_LoadingText.enabled = true;

        GameObject centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
        overlay_LoadingText.gameObject.transform.position = centerEyeAnchor.transform.position + new Vector3(0f, 0f, 0f);

        // wait for x seconds to prevent pop of new scene
        yield return new  WaitForSeconds(5f);

        //
        AsyncOperation aSceneWillLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        while (!aSceneWillLoad.isDone)
        {
            yield return null;
        }

        // disable the overlays now that the loading is done
        overlay_Background.enabled = false;
        overlay_LoadingText.enabled = false;

        yield return null;
    }
}
