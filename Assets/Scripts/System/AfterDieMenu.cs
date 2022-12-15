using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterDieMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    public void Dead()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        menu.SetActive(true);
    }
}
