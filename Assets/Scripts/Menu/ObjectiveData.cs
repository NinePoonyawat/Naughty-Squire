using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectiveData : ScriptableObject
{
    public int time;

    public string[] objectiveName;
    public int[] objectivePoint;
    public int inventoryScore;

    public void updateObjective(int idx, string name, int point)
    {
        objectiveName[idx] = name;
        if (point < 0) point = 0;
        objectivePoint[idx] = point;
    }

    public void UpdateScore(int idx,int point)
    {
        objectivePoint[idx] = point;
    }

    public void UpdateTime(int t)
    {
        time = t;
    }

    public void SetInventoryScore(int newInventoryScore)
    {
        inventoryScore = newInventoryScore;
    } 
}
