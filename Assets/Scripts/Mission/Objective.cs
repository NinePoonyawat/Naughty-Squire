using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Objective : MonoBehaviour
{
    [SerializeField] protected int[] score = new int[3];
    protected CompleteLevel level;

    public TMP_Text text;

    public abstract void UpdateText();

    public virtual void Promotions()
    {
        if (level != CompleteLevel.Third) level++;
    }

    public virtual void Relagations()
    {
        if (level != CompleteLevel.Fail) level--;
    }

    public CompleteLevel GetLevel()
    {
        return level;
    }

    public int GetScore()
    {
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