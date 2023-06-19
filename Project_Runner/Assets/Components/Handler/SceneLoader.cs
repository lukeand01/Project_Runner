using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]GameObject transitionUI;
    [SerializeField] int currentScene;



    public void ChangeScene(int load)
    {
        StartCoroutine(ChangeSceneProcess(load));
    }

    IEnumerator ChangeSceneProcess(int load)
    {
        transitionUI.SetActive(true);

        GameHandler.instance.StopGame();
               
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentScene);

        while (!asyncUnload.isDone)
        {
           yield return null;
        }

        Time.timeScale = 1;

        yield return new WaitForSeconds(0.5f);
        transitionUI.SetActive(false);
        yield return new WaitForSeconds(1);
        currentScene = load;
        GameHandler.instance.StartGame();

    }

}
