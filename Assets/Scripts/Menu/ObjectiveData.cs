using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectiveData : ScriptableObject
{
    public string[] objectiveName;
    public int[] objectivePoint;

    public void updateObjective(int idx, string name, int point)
    {
        objectiveName[idx] = name;
        if (point < 0) point = 0;
        objectivePoint[idx] = point;
    }
}
