using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour
{
    public Texture2D cursorArrow;

    void Awake()
    {
        Cursor.visible = false;
    }

    public void Active()
    {
        Cursor.visible = true;
    }

    public void DeActive()
    {
        Cursor.visible = false;
    }
}
