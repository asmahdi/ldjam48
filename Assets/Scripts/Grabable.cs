using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{
    public Camera camera;
    public AudioSource grabAudio;

    private float startPosX, startPosY;

    private bool isBeingHeld = false;


    void Update()
    {
        if (isBeingHeld == true)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = camera.ScreenToWorldPoint(mousePos);

            transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);
        }
        
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = camera.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - transform.localPosition.x;
            startPosY = mousePos.y - transform.localPosition.y;

            isBeingHeld = true;
            grabAudio.Play();
        }
    }

    private void OnMouseUp()
    {
        isBeingHeld = false;
        grabAudio.Play();
    }
}
