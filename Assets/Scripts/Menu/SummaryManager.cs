using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SummaryManager : MonoBehaviour
{
    public TMP_Text[] objectiveTexts;
    public ObjectiveData objectiveData;


    void Awake()
    {
        FindObjectOfType<AudioManager>().Play("Lobby");
        objectiveTexts = GetComponentsInChildren<TMP_Text>();

        StartCoroutine(ShowObjective());
    }

    public void UpdateText()
    {
        StartCoroutine(ShowObjective());
    }

    IEnumerator ShowObjective()
    {
        for (int i = 0; i < 5; i++)
        {
            TMP_Text objectiveText = objectiveTexts[i];
            TMP_Text objectivePoint = objectiveTexts[i+5];

            string objective = objectiveData.objectiveName[i];
            int point = objectiveData.objectivePoint[i];

            objectiveText.text = "";

            foreach (char letter in objective)
            {
                objectiveText.text += letter;
                FindObjectOfType<AudioManager>().Play("Type");

                yield return new WaitForSeconds(0.05f);
            }
            
            for (int p = 0; p <= point; p++)
            {
                yield return new WaitForSeconds(0.4f);

                objectivePoint.text = p.ToString();
                FindObjectOfType<AudioManager>().Play("Type");
            }

            if (point == 0)
            {
                FindObjectOfType<AudioManager>().Play("Failed");
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("Passed");
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
