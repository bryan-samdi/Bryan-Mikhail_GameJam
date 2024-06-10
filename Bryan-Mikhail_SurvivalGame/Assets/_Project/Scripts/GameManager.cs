using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;

    }

    private void Awake()
    {
        Time.timeScale = 1f;

    }
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

    }


    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

   
}