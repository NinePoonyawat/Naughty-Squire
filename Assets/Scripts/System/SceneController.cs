using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string currentScene;

    public ObjectiveData objectiveData;
    
    /*public void Update()
    {
        if (!isGameScene) return;

        if (Input.GetKey(KeyCode.Escape))
        {
            GoToScene("InventoryLoadout");
        }
    }

    public void GoToScene(string scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
        yield break;
    }*/

    public void PlayStage()
    {
        int stage = objectiveData.stage;
        if (stage == 1)
        {
            SceneManager.LoadScene ("Test FPS");
        }
        if (stage == 2)
        {
            SceneManager.LoadScene ("Test FPS 1");
        }
    }

    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
