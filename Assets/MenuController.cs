using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject MenuCanvas;
    private bool isMenuOpen = false;
    // Start is called before the first frame update

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
        MenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseMenu()
    {
        isMenuOpen = false;
        MenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
