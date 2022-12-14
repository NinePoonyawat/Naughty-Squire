using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject MenuCanvas;
    [SerializeField] private GameObject SettingCanvas;
    private bool isMenuOpen = false;
    private bool isSetting = false;

    // Start is called before the first frame update
    void Start()
    {
        CloseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuOpen) CloseMenu();
            else OpenMenu();
        }
    }

    public void OpenMenu()
    {
        isMenuOpen = true;
        isSetting = false;
        MenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseMenu()
    {
        isMenuOpen = false;
        isSetting = false;
        MenuCanvas.SetActive(false);
        SettingCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToSetting()
    {
        MenuCanvas.SetActive(false);
        SettingCanvas.SetActive(true);
        isSetting = true;
    }

    public void BackToMain()
    {
        MenuCanvas.SetActive(true);
        SettingCanvas.SetActive(false);
        isSetting = false;
    }
}
