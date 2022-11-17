using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingObjective : Objective
{
    [SerializeField] private int[] time = new int[3];
    private float timeCounter;
    private float levelTime;
    private int levelRank;
    // Start is called before the first frame update
    void Start()
    {
        color = "#00FF00";
        
        if (score[0] == 0 && score[1] == 0 && score[2] == 0)
        {
            score[0] = 100;
            score[1] = 150;
            score[2] = 200;
        }
        if (time[0] == 0 && time[1] == 0 && time[2] == 0)
        {
            time[0] = 600;
            time[1] = 500;
            time[2] = 400;
        }
        timeCounter = 0;
        levelTime = time[2];
        levelRank = 2;

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;
        TimeUpdate();
        UpdateText();
    }

    void TimeUpdate()
    {
        if (level == CompleteLevel.Fail) return;
        if (timeCounter >= levelTime)
        {
            Relagations();
            levelRank--;
            levelTime = time[levelRank];
        }
    }

    public int GetTime()
    {
        return ((int) timeCounter);
    }

    public override void UpdateText()
    {
        uiText.text = "clear mission in " + levelTime + " seconds <color=" + color + "> (" + GetTime() + " / " + levelTime + " )</color>";  
    }
}
