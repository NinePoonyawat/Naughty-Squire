using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SummaryManager : MonoBehaviour
{
    public TMP_Text[] objectiveTexts;
    public ObjectiveData objectiveData;
    
    public Sprite[] gradeList;
    public Image gradeImage;

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
        
        string grade;
        if (sum == 0) { grade = "D"; gradeImage.sprite = gradeList[0]; }
        else if (sum <= 8) { grade = "C"; gradeImage.sprite = gradeList[1]; }
        else if (sum <= 10) { grade = "B"; gradeImage.sprite = gradeList[2]; }
        else if (sum <= 12) { grade = "A"; gradeImage.sprite = gradeList[3]; }
        else if (sum <= 16) { grade = "S"; gradeImage.sprite = gradeList[4]; }
        else if (sum <= 19) { grade = "SS"; gradeImage.sprite = gradeList[5]; }
        else { grade = "SSS"; gradeImage.sprite = gradeList[6]; }

        float textspeed = 0.4f;
        for (int p = 0; p <= sum; p++)
        {
            yield return new WaitForSeconds(textspeed);
            textspeed *= 0.9f;

            objectiveTexts[10].text = p.ToString();
            FindObjectOfType<AudioManager>().Play("Type");
        }
        //objectiveTexts[11].text = grade;
        FindObjectOfType<AudioManager>().Play("Passed");
        gradeImage.color = new Color(gradeImage.color.r, gradeImage.color.g, gradeImage.color.b, 1f);
    }
}
