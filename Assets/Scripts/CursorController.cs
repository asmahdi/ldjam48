using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D defaultPointer, grabPointer;



    public void Grab()
    {
        Cursor.SetCursor(grabPointer, new Vector2(0, 0),CursorMode.Auto);
    }

    public void Default()
    {
        Cursor.SetCursor(defaultPointer, new Vector2(0, 0), CursorMode.Auto);
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Grab();
        }
        else
        {
            Default();
        }
    }
}
