using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour
{
    public ObjectiveData objectiveData;

    public void PlayStage(int stage)
    {
        objectiveData.stage = stage;
        if (stage == 0)
        {
            SceneManager.LoadScene ("Tutorial");
        }
        else
        {
            SceneManager.LoadScene ("InventoryLoadout");
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene ("LobbyScene");
    }

    public void SummaryMenu()
    {
        SceneManager.LoadScene ("SummaryScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
