using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGuardianAngel : MonoBehaviour
{
    [SerializeField] private ObjectiveData objectiveData;
    [SerializeField] private InventoryLoadoutSystem loadoutData;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
