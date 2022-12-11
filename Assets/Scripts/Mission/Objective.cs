using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Objective : MonoBehaviour
{
    [SerializeField] protected int[] score = new int[3];
    protected CompleteLevel level;
    public ObjectiveData objectiveData;
    public int objectiveIdx;

    public TMP_Text uiText;

    protected string color = "#FF0000";

    public abstract void UpdateText();

    public virtual void Promotions()
    {
        if (level != CompleteLevel.Third) level++;

        if (level == CompleteLevel.Fail) color = "#FF0000";
        else if (level == CompleteLevel.First) color = "#FFA500";
        else if (level == CompleteLevel.Second) color = "#FFFF00";
        else color = "#00FF00";

        UpdateObjectiveData();
    }

    public virtual void Relagations()
    {
        if (level != CompleteLevel.Fail) level--;

        if (level == CompleteLevel.Fail) color = "#FF0000";
        else if (level == CompleteLevel.First) color = "#FFA500";
        else if (level == CompleteLevel.Second) color = "#FFFF00";
        else color = "#00FF00";

        UpdateObjectiveData();
    }

    public void UpdateObjectiveData()
    {
        objectiveData.UpdateScore(objectiveIdx, GetScore());
    }

    public CompleteLevel GetLevel()
    {
        return level;
    }

    public int GetScore()
    {
        Debug.Log(level);
        if (level == CompleteLevel.Fail) return 0;
        return score[((int) level) - 1];
    }
}

public enum CompleteLevel
{
    Fail,
    First,
    Second,
    Third,
}