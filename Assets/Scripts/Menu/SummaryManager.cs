using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SummaryManager : MonoBehaviour
{
    public TMP_Text[] objectiveTexts;
    public ObjectiveData objectiveData;
    public Button[] buttons;
    
    public Sprite[] gradeList;
    public Image gradeImage;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        FindObjectOfType<AudioManager>().Play("Lobby");
        objectiveTexts = GetComponentsInChildren<TMP_Text>();
        buttons = GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        Debug.Log(objectiveData.inventoryScore);

        StartCoroutine(ShowObjective());
    }

    public void UpdateText()
    {
        StartCoroutine(ShowObjective());
    }

    IEnumerator ShowObjective()
    {
        int minutes = objectiveData.time/60;
        int seconds = objectiveData.time%60;
        if(minutes < 10) objectiveTexts[11].text = "0";
        objectiveTexts[11].text += minutes.ToString() + ":";
        if(seconds < 10) objectiveTexts[11].text += "0";
        objectiveTexts[11].text += seconds.ToString();



        int sum = 0;
        for (int i = 0; i < 5; i++)
        {
            TMP_Text objectiveText = objectiveTexts[i];
            TMP_Text objectivePoint = objectiveTexts[i+5];

            string objective = objectiveData.objectiveName[i];
            int point = objectiveData.objectivePoint[i];
            sum += point;

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

            if (point == 0) FindObjectOfType<AudioManager>().Play("Failed");
            else FindObjectOfType<AudioManager>().Play("Passed");

            yield return new WaitForSeconds(1f);
        }
        
        if (sum == 0) { gradeImage.sprite = gradeList[0]; }
        else if (sum <= 8) { gradeImage.sprite = gradeList[1]; }
        else if (sum <= 10) { gradeImage.sprite = gradeList[2]; }
        else if (sum <= 12) { gradeImage.sprite = gradeList[3]; }
        else if (sum <= 16) { gradeImage.sprite = gradeList[4]; }
        else if (sum <= 19) { gradeImage.sprite = gradeList[5]; }
        else { gradeImage.sprite = gradeList[6]; }

        float textspeed = 0.4f;
        for (int p = 0; p <= sum; p++)
        {
            yield return new WaitForSeconds(textspeed);
            textspeed *= 0.9f;

            objectiveTexts[10].text = p.ToString();
            FindObjectOfType<AudioManager>().Play("Type");
        }
        FindObjectOfType<AudioManager>().Play("Passed");
        gradeImage.color = new Color(gradeImage.color.r, gradeImage.color.g, gradeImage.color.b, 1f);
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }
}
